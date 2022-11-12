using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
[System.Serializable]
public class PlayerControllerPhoton : MonoBehaviourPun
{

    public float speed = 5f;
    public InputField inputFieldChatting;
    public GameObject canvas;
    public GameObject speechBubble;
    public Transform camPos;
    public Text textSpeechBubble;
    public ChatManager chatManager;
    public void Start()
    {
        chatManager = GameObject.FindObjectOfType<ChatManager>();
        speechBubble.SetActive(false);
        #region �̵��� ���� ī�޶� ���� �κ�
        #endregion
        if (photonView.IsMine)
        {
            canvas = GameObject.Find("Canvas");
            inputFieldChatting = canvas.transform.GetChild(0).transform.GetChild(0).GetComponent<InputField>();
            //inputFieldChatting = gameObject.transform.Find("Chatting").GetComponent<InputField>();
            camPos.gameObject.SetActive(true);
            
        }
    }

    void Update()
    {   
    }
    
    // �÷��̾� �̵�
    public void Move(Vector2 inputDir)
    {
        // �̵� ����Ű �Է� �� ��������
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            // �̵� ����
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // �÷��̾� �̵�
            transform.position += moveDir * Time.deltaTime * speed;
        }


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

