using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [Header("NPC ä��")]
    public TextMeshProUGUI textNPCSpeechBUbble;
    public GameObject NPCSpeechBubble;

    [Header("���̽�ƽ")]
    public GameObject joyStickMove;

    [Header("��ư")]
    public GameObject pannelforNPC;
    public Button btnNo;
    public Button btnYes;

    [Header("�κ�� �븮��Ʈ�� �����")]
    public static bool isShowRoomList;

    private void Start()
    {
        isShowRoomList = false;
        joyStickMove = GameObject.FindGameObjectWithTag("Player");
        pannelforNPC.SetActive(false);
        NPCSpeechBubble.SetActive(false);
        btnNo.onClick.AddListener(OnClickNo);
        btnYes.onClick.AddListener(OnClickOut);
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NPCSpeechBubble.SetActive(true);
            pannelforNPC.SetActive(true);
            StartCoroutine(TextPannel());
        }
    }
    private void OnTriggerEnter(Collider other)
    {       
           if(i>0)
            {
            return;
            }

         if (other.tag == "Player")
            {
            Debug.Log("Hit");
            NPCSpeechBubble.SetActive(true);
            pannelforNPC.SetActive(true);
            StartCoroutine(TextPannel());
            i++;
        }
        
    }
    int i =0;
    IEnumerator TextPannel()
    {
        textNPCSpeechBUbble.text = "�ȳ�!";
        yield return new WaitForSeconds(5.0f);
        textNPCSpeechBUbble.text = "���� �����ϰ���� �ʾ�?";
        yield return new WaitForSeconds(5.0f);
        pannelforNPC.SetActive(false);

    }
    private void OnTriggerExit(Collider other)
    {
        NPCSpeechBubble.SetActive(false);
        pannelforNPC.SetActive(false);
        joyStickMove.SetActive(true);
        i = 0;
    }
    void OnClickOut()
    {
        isShowRoomList = true;
        NPCSpeechBubble.SetActive(false);
        pannelforNPC.SetActive(false);
        joyStickMove.SetActive(true);
    }
    void OnClickNo()
    {
        NPCSpeechBubble.SetActive(false);
        pannelforNPC.SetActive(false);
        joyStickMove.SetActive(true);
    }

}
