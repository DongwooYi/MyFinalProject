using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [Header("NPC ä��")]
    public Text textNPC;
    public GameObject NPCSpeaking;

    [Header("���̽�ƽ")]
    public GameObject joyStickMove;

    [Header("��ư")]
    public GameObject btnCraeteRoom;
    public GameObject btnOnlist;
    public GameObject btnNo;
    public GameObject btnGoBack;
    [Header("�κ�� �븮��Ʈ�� �����")]
    public bool isTriggershowRoomList;
    public bool isTiggerEnter;

    public Joystick2DPhoton Joystick2DPhoton;
    //public GameObject playerPrefabs;
   // public PlayerControllerPhoton PlayerControllerPhoton;
    
    private void Start()
    {
        //PlayerControllerPhoton = playerPrefabs.GetComponent<PlayerControllerPhoton>();

        /*if (PlayerControllerPhoton == null || PlayerControllerPhoton.isActiveAndEnabled ==false)
        {
            return;
        }*/
        isTiggerEnter = false;
        isTriggershowRoomList = false;
    }
    private void OnTriggerEnter(Collider other)
    {       
         if (other.tag == "Player")
            {
            Debug.Log("Hit");
            textNPC.text = "ç���� ����� ���� ������?";
            NPCSpeaking.SetActive(true);
            btnCraeteRoom.SetActive(true);
            btnNo.SetActive(true);
            btnOnlist.SetActive(false);
            btnGoBack.SetActive(false);
            joyStickMove.SetActive(false);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        NPCSpeaking.SetActive(false);
        joyStickMove.SetActive(true);
    }
    public void OnClickMakingRoom()
    {
        NPCSpeaking.SetActive(false);
        isTiggerEnter = true;
    }
    public void onClickShowRoomList()
    {
        NPCSpeaking.SetActive(false);
        isTriggershowRoomList = true;


    }

    public void OnClickNottoMakeaRoom()
    {
        textNPC.text = "�׷��� �ʰ� ���ϴ� ç���� ����?";
        btnCraeteRoom.SetActive(false);
        btnNo.SetActive(false);
        btnOnlist.SetActive(true);
        btnGoBack.SetActive(true);
    }
    public void OnClickBack()
    {
        textNPC.text = "...�˰ھ�... ������ �ǿ�";
        Invoke("EndNPCSpeaking", 1.0f);
    }
    public void EndNPCSpeaking()
    {
        NPCSpeaking.SetActive(false);
        joyStickMove.SetActive(true);
    }

}
