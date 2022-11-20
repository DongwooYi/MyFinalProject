using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("룸리스트")]
    public Text welcomeText;

    [Header("메인 광장")]
    // 메인월드

    [Header("포톤 방 생성 필요 목록")]
    //방설명 InputField
    public InputField inputFieldRoomDescription;
    //방이름 InputField
    public InputField inputRoomName;
    //비밀번호 InputField
    public InputField inputPassword;
    //총인원 InputField
    public InputField inputMaxPlayer;
    //방참가 Button
    public Button btnJoin;
    //방생성 Button
    public Button btnCreate;

    public MakingChattingRoom doorCheck;
    //방의 정보들   
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //룸 리스트 Content

    [Header("룸 리스트")]
    public GameObject roomItemFactory;
    public Transform trListContent;

    public LoadGallery loadGallery;

    //map Thumbnail
    public GameObject[] mapThumbs;

    [Header("방만들기 및 방 리스트")]
    public GameObject setRoom;
    public GameObject setRoomlist;

    [Header("챌린지 기간")]
    public Text textCalendar;
    public UnityCalendar unityCalendar;
    public Dropdown dropdown;

    [Header("시작 시간")]
    [Header("시작 시간")]
    public string startDate;
    DateTime dateTime;
    DateTime dateTime1;
    #region 요일 선택
    [Header("요일 선택")]
    public Toggle toggleMon;
    public Toggle toggleTue;
    public Toggle toggleWed;
    public Toggle toggleThu;
    public Toggle toggleFri;
    public Toggle toggleSat;
    public Toggle toggleSun;
    public bool isCheckMon;
    public bool isCheckTue;
    public bool isCheckWed;
    public bool isCheckThu;
    public bool isCheckFri;
    public bool isCheckSat;
    public bool isCheckSun;
    public string monText;
    public string tueText;
    public string wedText;
    public string thuText;
    public string friText;
    public string satText;
    public string sunText;
    #endregion

    void Start()
    {
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        dropdown.onValueChanged.AddListener(delegate { HandleInputData(dropdown.value); });
        // 방이름(InputField)이 변경될때 호출되는 함수 등록
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // 총인원(InputField)이 변경될때 호출되는 함수 등록
        inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);
        string[] s = Microphone.devices;

    }
    private void Update()
    {
        dateTime = DateTime.Now;
        if (NPC.isTiggerEnter)
        {
            setRoom.SetActive(true);
        }
        if (doorCheck.GotoMainWorld == true)
        {
            doorCheck.GotoMainWorld = false;
            CreateChatroom();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            setRoom.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            setRoom.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
           setRoomlist.SetActive(true);
        }
        ToggleCheck();
    }

    public void OnRoomNameValueChanged(string s)
    {
        //참가
        //  btnJoin.interactable = s.Length > 0;
        //생성
        btnCreate.interactable = s.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    public void OnMaxPlayerValueChanged(string s)
    {
        //생성
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0;
    }

    public void CreateChatroom()
    {
        //mapThumbs.texture = loadGallery.gameObject.GetComponent<RawImage>().texture;
        // 방 옵션을 설정
        RoomOptions roomOptions = new RoomOptions();
        // 최대 인원 (0이면 최대인원)
        roomOptions.MaxPlayers = 4;
        // 룸 리스트에 보이지 않게? 보이게?
        roomOptions.IsVisible = false;
        
        PhotonNetwork.JoinOrCreateRoom("Room",roomOptions, null);
       
    }
        
    //방 생성
    public void CreateRoom()
    {
        //mapThumbs.texture = loadGallery.gameObject.GetComponent<RawImage>().texture;
        // 방 옵션을 설정
        RoomOptions roomOptions = new RoomOptions();
        // 최대 인원 (0이면 최대인원)
        roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text);
        // 룸 리스트에 보이지 않게? 보이게?
        roomOptions.IsVisible = true;

        // custom 정보를 셋팅
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

        hash["desc"] = $"회의 요일: {monText}{tueText}{wedText}{thuText}{friText}{sunText}{sunText}\r\n방 설명: {inputFieldRoomDescription.text}";
        hash["map_id"] = UnityEngine.Random.Range(0, mapThumbs.Length);
        hash["room_name"] = inputRoomName.text;
        hash["password"] = inputPassword.text;
        hash["date"] = textCalendar.text;
        hash["TimerData"] = startDate;


        roomOptions.CustomRoomProperties = hash;
        // custom 정보를 공개하는 설정
        roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "desc", "map_id", "room_name", "password", "date", "TimerData"
        };

        // 방 생성 요청 (해당 옵션을 이용해서)
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions);
    }

    //방이 생성되면 호출 되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        
        print("OnCreatedRoom");
    }

    //방 생성이 실패 될때 호출 되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);
    }

    //방 참가 요청
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
    }

    //방 참가가 완료 되었을 때 호출 되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        var currentRoomname = PhotonNetwork.CurrentRoom.Name;
        print(currentRoomname);
        if (currentRoomname.Contains("Room"))
        {
            PhotonNetwork.LoadLevel("SB_Player_Photon");
        }
        else if (currentRoomname == "TestRoom")
        {
            PhotonNetwork.LoadLevel("CamInteraction");
        }
        else
        {
            PhotonNetwork.LoadLevel("CamInteraction");
        }
    }

    //방 참가가 실패 되었을 때 호출 되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

    //방에 대한 정보가 변경되면 호출 되는 함수(추가/삭제/수정)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //룸리스트 UI 를 전체삭제
        DeleteRoomListUI();
        //룸리스트 정보를 업데이트
        UpdateRoomCache(roomList);
        //룸리스트 UI 전체 생성
        CreateRoomListUI();
    }

    void DeleteRoomListUI()
    {
        foreach (Transform tr in trListContent)
        {
            Destroy(tr.gameObject);
        }
    }
    void UpdateRoomCache(List<RoomInfo> roomList)
    {

        for (int i = 0; i < roomList.Count; i++)
        {
            // 수정, 삭제
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //만약에 해당 룸이 삭제된것이라면
                if (roomList[i].RemovedFromList)
                {
                    //roomCache 에서 해당 정보를 삭제
                    roomCache.Remove(roomList[i].Name);
                }
                //그렇지 않다면
                else
                {
                    //정보 수정
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            //추가
            else
            {
                roomCache[roomList[i].Name] = roomList[i];
            }
        }
    }
    void CreateRoomListUI()
    {
        foreach (RoomInfo info in roomCache.Values)
        {

            //룸아이템 만든다.
            GameObject go = Instantiate(roomItemFactory, trListContent);
            //룸아이템 정보를 셋팅(방제목(0/0))
            RoomItem item = go.GetComponent<RoomItem>();
            item.SetInfo(info);

            //roomItem 버튼이 클릭되면 호출되는 함수 등록
            item.onClickAction = SetRoomName;
            //람다식
            //item.onClickAction = (string room) => {
            //    inputRoomName.text = room;
            //};

            string desc = (string)info.CustomProperties["desc"];
            int map_id = (int)info.CustomProperties["map_id"];
            print(desc + ", " + map_id);
        }
    }
    //이전 Thumbnail id
    int prevMapId = -1;
    void SetRoomName(string room, int map_id)
    {
        //룸이름 설정
        inputRoomName.text = room;

        //만약에 이전 맵 Thumbnail이 활성화가 되어있다면
        if (prevMapId > -1)
        {
            //이전 맵 Thumbnail을 비활성화
            mapThumbs[prevMapId].SetActive(false);
        }

        //맵 Thumbnail 설정
        mapThumbs[map_id].SetActive(true);

        //이전 맵 id 저장
        prevMapId = map_id;
    }
    public void OnClick_GetDate()
    {
        DateTime dt = unityCalendar.GetDate();
        textCalendar.text = startDate + "~" + dt.ToString("yyyy-MM-dd");
    }
    public void OnClick_Clear()
    {
        textCalendar.text = string.Empty;
        unityCalendar.Init();
    }
    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            
            dateTime1 = dateTime.AddHours(24);
            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            print("24");
        }
        if (val == 1)
        {
           
            dateTime1 = dateTime.AddHours(72);
            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            print("72");
        }
        if (val == 2)
        {
           
            dateTime1 = dateTime.AddHours(168);
            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            print("168");
        }
    }
    public void ToggleCheck()
    {
        //Debug.Log("월: " + isCheckMon +"\r\n화: " + isCheckTue + "\r\n수: "+ isCheckWed+ "\r\n목: "+ isCheckThu+"\r\n금: "+isCheckFri+"\r\n토: "+isCheckSat+"\r\n일:"+isCheckSun);
        if (toggleMon.isOn)
        {
            isCheckMon = true;
            monText = "월";
        }
        else
        {
            isCheckMon = false;
            monText = "";
        }
        if (toggleTue.isOn)
        {
            isCheckTue = true;
            tueText = "화";
        }
        else
        {
            isCheckTue = false;
            thuText = "";
        }
        if (toggleWed.isOn)
        {
            isCheckWed = true;
            wedText = "수";
        }
        else
        {
            isCheckWed = false;
            wedText = "";
        }
        if (toggleThu.isOn)
        {
            isCheckThu = true;
            thuText = "목";
        }
        else
        {
            isCheckThu = false;
            thuText = "";
        }
        if (toggleFri.isOn)
        {
            isCheckFri = true;
            friText = "금";
        }
        else
        {
            isCheckFri = false;
            friText = "";
        }
        if (toggleSat.isOn)
        {
            isCheckSat = true;
            satText = "토";
        }
        else
        {
            isCheckSat = false;
            satText = "";
        }
        if (toggleSun.isOn)
        {
            isCheckSun = true;
            sunText = "일";
        }
        else
        {
            isCheckSun = false;
            sunText = "";
        }

    }

    #region Http
    public void SendRoomData()
    {
        HttpRequester requester = new HttpRequester();

        requester.url = "";
        requester.requestType = RequestType.POST;

        RoomData roomData = new RoomData();

        roomData.RoomName = inputRoomName.text;
        roomData.ThePeriodProject = textCalendar.text; 
        roomData.MeetingDate = $"{monText}{tueText}{wedText}{thuText}{friText}{sunText}{sunText}";
        
        requester.body = JsonUtility.ToJson(roomData, true);
        requester.onComplete = OnCompletePostRoomData;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    public void OnCompletePostRoomData(DownloadHandler downloadHandler)
    {
        JObject jObject = JObject.Parse(downloadHandler.text);

       // int type = (int)jObject["status"];
        //CreateRoom();
    }
    #endregion
}


