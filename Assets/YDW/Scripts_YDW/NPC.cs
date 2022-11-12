using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public GameObject textSpeechBubble;
    public Button btnCreatRoom;
    public Button btnJoinRoom;
    string sceneName;
    /*    public enum PickType
        {
            CreateRoom,
            JoinedRoom
        }
        public PickType pickType;*/
    // Start is called before the first frame update
    void Start()
    {
       
        textSpeechBubble.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
      /* if(Input.GetKeyDown(KeyCode.A))
        {
            ChattingStart();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            
            OnCreateRoom();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {

            OnCreateRoom();
        }*/

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            textSpeechBubble.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject)
        {
            textSpeechBubble.SetActive(false);
        }
    }
    public void  ChattingStart()
    {        
        textSpeechBubble.SetActive(true);
        textSpeechBubble.gameObject.GetComponentInChildren<Text>().text = "ç���� �����غ���?";
    /*    switch (pickType)
        {
            case PickType.CreateRoom:
                OnCreateRoom();
                break;
            case PickType.JoinedRoom:
                OnJoinedRoom();
                break;
        }*/
    }
    public void GotoLobby()
    {
        textSpeechBubble.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public bool SetactiveCreatRoomPopUP;
    public bool SetactiveJoinedRoomPopUP;
    
    public void OnCreateRoom()
    {
        SetactiveCreatRoomPopUP = true;
        SetactiveJoinedRoomPopUP = false;
        btnCreatRoom.gameObject.SetActive(false);
        btnJoinRoom.gameObject.SetActive(false);
        textSpeechBubble.gameObject.GetComponentInChildren<Text>().text = "�˰ھ�!!";
        if(textSpeechBubble.gameObject.GetComponentInChildren<Text>().text == "�˰ھ�!!")
        {
            Debug.Log("OnCreateRoom == " + textSpeechBubble.gameObject.GetComponentInChildren<Text>().text);
            Invoke("GotoLobby",1.0f);
        }
    }
    public void OnJoinedRoom()
    {
        SetactiveCreatRoomPopUP = false;
        SetactiveJoinedRoomPopUP = true;
        btnCreatRoom.gameObject.SetActive(false);
        btnJoinRoom.gameObject.SetActive(false);
        textSpeechBubble.gameObject.GetComponentInChildren<Text>().text = "���ϴ� ç���� �����ٰ�";
        if (textSpeechBubble.gameObject.GetComponentInChildren<Text>().text == "���ϴ� ç���� �����ٰ�")
        {
            Debug.Log("OnJoinedRoom == " + textSpeechBubble.gameObject.GetComponentInChildren<Text>().text);
            Invoke("GotoLobby", 1.0f);
        }
    }
}
