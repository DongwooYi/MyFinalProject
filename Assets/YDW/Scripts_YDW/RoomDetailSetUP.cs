using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomDetailSetUP : MonoBehaviour
{
    public Text roomName;
    public Text bookName;
    public Text challengeDay;
    public Text MeetingDayofweeks;
    public Text meetingTime;
    public Text hostName;
    public Text roomPlayerInfo;
    public Text roomShortDesc;
    public Text roomDesc;
    public RawImage RawImage;
    public Button onClickJoin;
    public LobbyManager lobbyManager;
    // Start is called before the first frame update
    void Start()
    {
        lobbyManager = GameObject.FindObjectOfType<LobbyManager>();
        onClickJoin.onClick.AddListener(delegate { lobbyManager.JoinRoom(); });
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
