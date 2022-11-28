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


    Touch touchZero;
    Touch touchOne;

    Scene sceneName;
    void Start()
    {
        sceneName = SceneManager.GetActiveScene();
        animator = characterBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            myself.SetActive(false);
        }
        else
        {
            myself.SetActive(true);
        }
        
    }
    private void FixedUpdate()
    {
        GetTouchInput();
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
        //bool isMove = joystick2DLobby.isInput;
        // 입력이 발생하는 중이라면 이동 애니메이션 재생
        animator.SetBool("IsMove", isMove);
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
                transform.position += moveDir * Time.deltaTime * 6f;
        }
    }
    public Text log;
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

        Camera.main.fieldOfView += deltaMagnitudeDiff * zoomIn * 0.1f;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 100f);

    }

    Vector2 startPos;
    void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            switch(t.phase)
            {
                case TouchPhase.Began:
                    touchZero = Input.GetTouch(0);
                    startPos = t.position;
                    break;
                case TouchPhase.Moved:
                    if (EventSystem.current.IsPointerOverGameObject(/*Input.GetTouch(0).fingerId*/) == false)
                    {
                        if (Input.touchCount == 1)
                        {
                            Vector2 delta = t.position - startPos;
                            rotX += delta.y * Time.deltaTime * 0.5f;
                            rotX = Mathf.Clamp(rotX, 5f, 20f);
                            rotY += delta.x * Time.deltaTime * 0.5f;
                            cameraArm.eulerAngles = new Vector3(rotX, rotY, 0);
                        }
                        else if (Input.touchCount == 2)
                        {
                            touchOne = Input.GetTouch(1);
                            GetTouchZoomInOut();
                        }
                        log.text = $"touchCount:{Input.touchCount},\r\n rotX: {rotX}, \r\n rotY: {rotY}";
                    }
                        break;
                case TouchPhase.Stationary:
                       cameraArm.eulerAngles = Vector3.zero;
                    break;
                case TouchPhase.Ended:
                                      break;
            }
        }
    }

    float rotX, rotY;

}