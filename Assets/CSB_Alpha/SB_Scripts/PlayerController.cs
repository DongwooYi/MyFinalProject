using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPun
{
    public Transform characterBody;
    public Transform camPos;


    public GameObject Player;
    public float speed = 5f;
   
    string sceneName;

    public Animator animator;
    public void Start()
    {
        
        sceneName  = SceneManager.GetActiveScene().name;
        animator = characterBody.GetComponent<Animator>();
    }
    private void Update()
    {
        //LookAround();
        //Move();
        if (sceneName != "LobbyScene")
        {
            Player.SetActive(false);
        }
        else
        {
            Player.SetActive(true);
        }
    }
    // 플레이어 이동
    public void Move(Vector2 InputDirection)
    {

        // 이동 방향 구하기 1
        //Debug.DrawRay(cameraArm.position, cameraArm.forward, Color.red);

        // 이동 방향 구하기 2
        //Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // 이동 방향키 입력 값 가져오기
        //Vector2 moveInput = InputDirection;
        // 이동 방향키 입력 판정 : 이동 방향 벡터가 0보다 크면 입력이 발생하고 있는 중
        bool isMove = moveInput.magnitude != 0;
        // 입력이 발생하는 중이라면 이동 애니메이션 재생
        animator.SetBool("isMove", isMove);
        print(isMove);
        if (isMove)
        {
            // 카메라가 바라보는 방향
            Vector3 lookForward = new Vector3(camPos.forward.x, 0f, camPos.forward.z).normalized;
            // 카메라의 오른쪽 방향
            Vector3 lookRight = new Vector3(camPos.right.x, 0f, camPos.right.z).normalized;
            // 이동 방향
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 이동할 때 카메라가 보는 방향 바라보기
            //characterBody.forward = lookForward;
            // 이동할 때 이동 방향 바라보기
            characterBody.forward = moveDir;
            // 이동
            transform.position += moveDir * Time.deltaTime;
        }
        
    }

    public void LookAround(Vector3 inputDirection)
    {
        // 마우스 이동 값 검출
        Vector2 mouseDelta = inputDirection;
        // 카메라의 원래 각도를 오일러 각으로 저장
        Vector3 camAngle = camPos.rotation.eulerAngles;
        // 카메라의 피치 값 계산
        float x = camAngle.x - mouseDelta.y;

        // 카메라 피치 값을 위쪽으로 70도 아래쪽으로 25도 이상 움직이지 못하게 제한
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        // 카메라 암 회전 시키기
        camPos.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }


}

