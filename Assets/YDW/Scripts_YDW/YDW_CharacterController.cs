using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YDW_CharacterController : MonoBehaviour
{
    public GameObject myself;
    public Transform characterBody;
    public Transform cameraArm;

    Animator animator;

    bool isCollisionCheck;
    [Range(0.1f, 1.0f)]
    public float rayDistance;

    [Header("카메라")]
    [Range(1, 50)]
    public float rotSpeed = 50f;
    [Range(0.1f, 1f)]
    public float zoomIn = 1f;
    Vector3 firstTouch;
    Vector3 secondTouch;
    Vector3 camAngle;

    Touch touchZero;
    Touch touchOne;

    Scene sceneName;
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene();
        animator = characterBody.GetComponent<Animator>();
        camAngle = cameraArm.rotation.eulerAngles;
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName.name != "MyRoomScene_Beta 1")
        {
            myself.SetActive(false);
        }
        else
        {
            myself.SetActive(true);
        }
        if (!EventSystem.current.IsPointerOverGameObject() == false) return;
        if (Input.touchCount > 0)
        {
            if (Input.touchCount ==1)
            {
                firstTouch = touchZero.position;
                touchZero = Input.GetTouch(0);
                if(touchZero.phase==TouchPhase.Moved)
                {
                    LookAround();

                }
            
            }
            if(Input.touchCount ==2)
            {
                touchOne = Input.GetTouch(1);
                GetTouchZoomInOut();
            }


            

        }

    }

    private void FixedUpdate()
    {
        CollisionCheck();
    }
    public void Move(Vector2 vector2)
    {
        // 이동 방향 구하기 1
        //Debug.DrawRay(cameraArm.position, cameraArm.forward, Color.red);

        // 이동 방향 구하기 2
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

        // 이동 방향키 입력 값 가져오기
        //Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 moveInput = vector2;
        // 이동 방향키 입력 판정 : 이동 방향 벡터가 0보다 크면 입력이 발생하고 있는 중
        bool isMove = moveInput.magnitude != 0;
        // 입력이 발생하는 중이라면 이동 애니메이션 재생
        animator.SetBool("isMove", isMove);
        if (isMove)
        {
            // 카메라가 바라보는 방향
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            // 카메라의 오른쪽 방향
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            // 이동 방향
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 이동할 때 카메라가 보는 방향 바라보기
            //characterBody.forward = lookForward;
            // 이동할 때 이동 방향 바라보기
           characterBody.forward = moveDir;
            // 이동
            if (!isCollisionCheck)
                transform.position += moveDir * Time.deltaTime * 3f;
        }
    }
    float x, y;
    public void LookAround()
    {
        secondTouch = touchOne.position;
        // 마우스 이동 값 검출
        // Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // 카메라의 원래 각도를 오일러 각으로 저장

        // 카메라의 피치 값 계산
        x += (camAngle.y + (secondTouch.y - firstTouch.y)) * Time.deltaTime * rotSpeed;
        y += (camAngle.x - (secondTouch.x - firstTouch.x)) * Time.deltaTime * rotSpeed;
        // 카메라 피치 값을 위쪽으로 70도 아래쪽으로 25도 이상 움직이지 못하게 제한
        // 위아래쪽으로 회전 (0~90도 사이)
        if (x < 180)
        {
            x = Mathf.Clamp(x, -1f, 30f);
        }
        // 카메라 회전 시키기
        cameraArm.rotation = Quaternion.Euler(x, y, camAngle.z);
    }

    void CollisionCheck()
    {
        Debug.DrawRay(transform.position, characterBody.forward * rayDistance, Color.black);
        isCollisionCheck = Physics.Raycast(transform.position, characterBody.forward, rayDistance, LayerMask.GetMask("CollisionCheck"));
       
    }


    // 카메라 줌인 줌아웃 관련
    // 두 손가락
    private void GetTouchZoomInOut()
    {


        //터치에 대한 이전 위치값을 각각 저장함
        //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // 각 프레임에서 터치 사이의 벡터 거리 구함
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        Camera.main.fieldOfView += deltaMagnitudeDiff * zoomIn;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 100f);

    }
    
}
