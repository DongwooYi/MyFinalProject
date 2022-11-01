using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_YDW : MonoBehaviour
{
    [Header("따라갈 오브젝트 정보")]
    public Transform objectTofollow;
    [Header("따라갈 스피드")]
    public float followSpeed = 10f;
    [Header("마우스 감도 / 회전 속도")]
    public float sensitivity = 100f;
    [Header("마우스 각도 제한 값")]
    public float clampAngleMin = 70f;
    public float clampAngleMax = 70f;

    //마우스 인풋
    private float rotX;
    private float rotY;

    [Header("카메라 정보")]
    public Transform realCamera;
    [Header("방향")]
    public Vector3 dirNormalized;
    [Header("최종 정해진 방향")]
    public Vector3 finalDir;

    // 방해물이 있을 시 필요한 변수
    [Header("최소 거리")]
    public float minDistance;
    [Header("최대 거리")]
    public float maxDistance;
    [Header("최종 거리")]
    public float finalDistance;
    [Header("카메라 부드럽게 움직이기")]
    public float smoothness = 10;

    [Header("마우스 휠")]
    //줌 속도 변수
    public float zoomSpeed = 0f;
    //줌 한계 위치값
    public float zoomMax = 0;
    public float zoomMin = 0f;

    [Header("캐릭터 회전 속도")]
    public float rotSpeed = 100f;

    //realCamera 처음 위치 저장
    Vector3 savedPos;
    //캐릭터 카메라 거리를 위한
    float char2cam = 0;
    //점프 시 카메라 움직임 제한을 위한 변수
    CharacterController cc;

    void Start()
    {
        // 마우스 인풋 초기화
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        // 필요 없는 ?
        // 백터 값 초기화 (normalized : 크기가 0 -> 방향만 저장 / magnitude : 크기)
        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        //처음 카메라 위치 저장
        savedPos = realCamera.position;
        //처음 최대거리 저장
        char2cam = maxDistance;

        //FollowCam 동적으로 컴포넌트가져오기
        objectTofollow = GameObject.FindObjectOfType<CharacterController>().GetComponent<Transform>();//GameObject.Find("FollowCam").GetComponent<Transform>(); 
        cc = objectTofollow.parent.GetComponent<CharacterController>();
    }

    void Update()
    {
        // 마우스 커서가 활성화 되어있다면 나간다!
        if (Cursor.visible == true) return;

        // 기본 카메라 움직임
        CameraMove();

        // 카메라 자동 움직임
        CameraAutoRotation();

        // 카메라 스크롤 줌 아웃
        CameraZoom();

        //캐릭터 위치에서 앞방향으로 -(최대거리)만큼 (Ray를 쏘기위해)
        savedPos = objectTofollow.position + transform.forward * -char2cam;
    }

    // 벽 통과 방지 
    // 카메라 움직임 (Update가 끝난 다음에 실행)
    private void LateUpdate()
    {
        //오브젝트를 따라가야 함
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);

        // 방해물 오브젝트의 정보를 저장 변수
        RaycastHit hit;

        // 방해물 존재 시
        // 카메라 위치, FollowCam위치까지, Player를 빼고
        if (Physics.Linecast(savedPos, objectTofollow.position, out hit, ~(1<<6)))
        {
            //최종거리는 제한을 둔다
            //finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            finalDistance = Mathf.Clamp(hit.distance, minDistance, zoomMax);
        }

        // 방해물 존재가 않는다면
        else
        {
            //최종거리는 0
            finalDistance = 0;
        }
        
        //마지막 위치 = 내 위치에서 + 앞방향으로 * 최종거리만큼
        Vector3 finalPosition = savedPos + realCamera.forward * finalDistance;

        realCamera.position = Vector3.Lerp(realCamera.position, finalPosition, Time.deltaTime * smoothness);

        #region 방해물 존재 시 카메라 움직임 (초기값)
        // 방해물 존재 시 카메라 움직임 (초기값)
        //realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
        //realCamera.position = finalPosition;

        //땅이라면 Lerp로 이동
        //if (cc.isGrounded)
        //{
        //    realCamera.position = Vector3.Lerp(realCamera.position, finalPosition, Time.deltaTime * smoothness);
        //}

        ////땅이 아니라면
        //else
        //{
        //    realCamera.position = finalPosition;
        //}
        #endregion
    }
    
    // 기본 카메라 움직임
    void CameraMove()
    {
        // 마우스 왼쪽버튼을 누른다면
        if (Input.GetMouseButton(0))
        {
            // 매프레임마다 인풋을 받기
            rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            // 각도 제한
            rotX = Mathf.Clamp(rotX, -clampAngleMin, clampAngleMax);
            // 회전시키기
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            transform.rotation = rot;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //rotY = 0;
        }
    }

    // 카메라 스크롤 줌/아웃
    void CameraZoom()
    {
        //앞으로 굴리면 -1, 뒤로 굴리면 1이 리턴
        float zoomDirection = Input.GetAxis("Mouse ScrollWheel");
      
        //카메라 위치 += 카메라 정면벡터 * 방향 * 스피드
        Vector3 futurePos = realCamera.position + realCamera.transform.forward * zoomDirection * zoomSpeed;
        float dist = Vector3.Distance(futurePos, objectTofollow.position);

        if (dist >= zoomMin && dist <= zoomMax)
        {
            realCamera.position = futurePos;
            // 0이 아니라면 다시 카메라 돌아가는 코드
            if(zoomDirection != 0)
            {
                char2cam = dist;
            }
        }
    }

    // 카메라 자동 회전
    void CameraAutoRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, Vector3.down, Time.deltaTime * rotSpeed);
        }

        else if (Input.GetKeyUp(KeyCode.Q))
        {
            rotX = transform.eulerAngles.x;
            rotY = transform.eulerAngles.y;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotSpeed);
        }

        else if (Input.GetKeyUp(KeyCode.E))
        {
            rotX = transform.eulerAngles.x;
            rotY = transform.eulerAngles.y;
        }
    }
}
