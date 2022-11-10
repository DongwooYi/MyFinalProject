using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public float speed = 5f;

    ChatManager chatManager;

    public Transform camPos;
    public void Start()
    {

        #region 이동우 포톤 카메라 수정 부분
        #endregion
        if (photonView.IsMine)
        {
            camPos.gameObject.SetActive(true);

        }
    }

    void Update()
    {
      
      
    }
   
    // 플레이어 이동
    public void Move(Vector2 inputDir)
    {
        // 이동 방향키 입력 값 가져오기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            // 이동 방향
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // 플레이어 이동
            transform.position += moveDir * Time.deltaTime * speed;
        }


    }

    public void LookAround(Vector3 inputDir)
    {
        //마우스 이동 값 검출
        Vector3 mouseDelta = inputDir;
        // 카메라의 원래 각도를 오일러 각의 저장
        Vector3 camAngle = camPos.rotation.eulerAngles;
        // 카메라 의 피치 값 계산
        float x = camAngle.x - mouseDelta.y;

        // 카메라의 피치값을 위쪽으로 70도 아래쪽은 25도 이상 움직이게 못하게 제한
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        //카메라 회전
        camPos.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }


}

