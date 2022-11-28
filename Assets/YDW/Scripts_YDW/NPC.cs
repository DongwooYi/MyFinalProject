using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [Header("NPC 채팅")]
    public Text textNPC;
    public TextMeshProUGUI textNPCSpeechBUbble;
    public GameObject NPCSpeaking;

    [Header("조이스틱")]
    public GameObject joyStickMove;

    [Header("버튼")]
    public GameObject pannelforNPC;
    public GameObject btnCraeteRoom;
    public GameObject btnOnlist;
    public GameObject btnNo;
    public GameObject btnGoBack;


    [Header("로비씬 룸리스트및 방생성")]
    public static bool isTriggershowRoomList;
    public static bool isTiggerEnter;

    private void Start()
    {
        joyStickMove = GameObject.FindGameObjectWithTag("Player");
        pannelforNPC.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {       
         if (other.tag == "Player")
            {
            Debug.Log("Hit");
  //          textNPC.text = "챌린지 만들어 보지 않을래?";
            StartCoroutine(TextPannel());
        }
        
    }
    IEnumerator TextPannel()
    {
        textNPCSpeechBUbble.text = "안녕!";
        yield return new WaitForSeconds(3.0f);
        textNPCSpeechBUbble.text = "독서 모임하고싶지 않아?";
        yield return new WaitForSeconds(3.0f);
        textNPC.text = "독서 모임하고싶지 않아?";
        pannelforNPC.SetActive(true);
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
