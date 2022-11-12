using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPun
{
    public GameObject Player;
    public float speed = 5f;
   
    public Transform camPos;
    string sceneName;
    public void Start()
    {
        
        sceneName  = SceneManager.GetActiveScene().name;        
    }
    private void Update()
    {
        if(sceneName != "LobbyScene")
        {
            Player.SetActive(false);
        }
        else
        {
            Player.SetActive(true);
        }
    }
    // 플레이어 이동
    public void Move(Vector2 inputDir)
    {
        
            // 이동 방향
           Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);
            // 플레이어 이동
            transform.position += moveDir * Time.deltaTime * speed;
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

