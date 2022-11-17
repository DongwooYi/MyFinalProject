using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [Header("NPC 채팅")]
    public Text textNPC;
    public GameObject NPCSpeaking;

    [Header("조이스틱")]
    public GameObject joyStickMove;

    [Header("버튼")]
    public GameObject btnCraeteRoom;
    public GameObject btnOnlist;
    public GameObject btnNo;
    public GameObject btnGoBack;
    [Header("로비씬 룸리스트및 방생성")]
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
            textNPC.text = "챌린지 만들어 보지 않을래?";
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
        textNPC.text = "그러면 너가 원하는 챌린지 볼래?";
        btnCraeteRoom.SetActive(false);
        btnNo.SetActive(false);
        btnOnlist.SetActive(true);
        btnGoBack.SetActive(true);
    }
    public void OnClickBack()
    {
        textNPC.text = "...알겠어... 다음에 또와";
        Invoke("EndNPCSpeaking", 1.0f);
    }
    public void EndNPCSpeaking()
    {
        NPCSpeaking.SetActive(false);
        joyStickMove.SetActive(true);
    }

}
