using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [Header("NPC ä��")]
    public Text textNPC;
    public TextMeshProUGUI textNPCSpeechBUbble;
    public GameObject NPCSpeechBubble;

    [Header("���̽�ƽ")]
    public GameObject joyStickMove;

    [Header("��ư")]
    public GameObject pannelforNPC;
    public GameObject btnCraeteRoom;
    public GameObject btnOnlist;
    public GameObject btnNo;
    public GameObject btnGoBack;


    [Header("�κ�� �븮��Ʈ�� �����")]
    public static bool isTriggershowRoomList;
    public static bool isTiggerEnter;

    private void Start()
    {
        joyStickMove = GameObject.FindGameObjectWithTag("Player");
        pannelforNPC.SetActive(false);
        NPCSpeechBubble.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {       
         if (other.tag == "Player")
            {
            Debug.Log("Hit");
            NPCSpeechBubble.SetActive(true);
            pannelforNPC.SetActive(true);
            StartCoroutine(TextPannel());
        }
        
    }
    IEnumerator TextPannel()
    {
        textNPCSpeechBUbble.text = "�ȳ�!";
        yield return new WaitForSeconds(5.0f);
        textNPCSpeechBUbble.text = "���� �����ϰ���� �ʾ�?";
        yield return new WaitForSeconds(5.0f);
        textNPC.text = "";
    }
    private void OnTriggerExit(Collider other)
    {
        NPCSpeechBubble.SetActive(false);
        joyStickMove.SetActive(true);
    }
    public void OnClickMakingRoom()
    {
        NPCSpeechBubble.SetActive(false);
        isTiggerEnter = true;
    }
    public void onClickShowRoomList()
    {
        NPCSpeechBubble.SetActive(false);
        isTriggershowRoomList = true;
    }

    public void OnClickNottoMakeaRoom()
    {
        textNPCSpeechBUbble.text = "�׷��� �ʰ� ���ϴ� ç���� ����?";
        btnCraeteRoom.SetActive(false);
        btnNo.SetActive(false);
        btnOnlist.SetActive(true);
        btnGoBack.SetActive(true);
    }
    public void OnClickBack()
    {
        textNPCSpeechBUbble.text = "...�˰ھ�... ������ �ǿ�";
        Invoke("EndNPCSpeaking", 1.0f);
    }
    public void EndNPCSpeaking()
    {
        NPCSpeechBubble.SetActive(false);
        joyStickMove.SetActive(true);
    }

}
