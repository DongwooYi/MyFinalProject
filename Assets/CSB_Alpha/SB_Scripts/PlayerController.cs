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
    // �÷��̾� �̵�
    public void Move(Vector2 inputDir)
    {
        
            // �̵� ����
           Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);
            // �÷��̾� �̵�
            transform.position += moveDir * Time.deltaTime * speed;
    }

    public void LookAround(Vector3 inputDir)
    {
        //���콺 �̵� �� ����
        Vector3 mouseDelta = inputDir;
        // ī�޶��� ���� ������ ���Ϸ� ���� ����
        Vector3 camAngle = camPos.rotation.eulerAngles;
        // ī�޶� �� ��ġ �� ���
        float x = camAngle.x - mouseDelta.y;

        // ī�޶��� ��ġ���� �������� 70�� �Ʒ����� 25�� �̻� �����̰� ���ϰ� ����
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        //ī�޶� ȸ��
        camPos.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }


}

