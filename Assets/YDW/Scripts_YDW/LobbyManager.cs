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
    [Header("�븮��Ʈ")]
    public Text welcomeText;

    [Header("���� ����")]
    // ���ο���

    [Header("���� �� ���� �ʿ� ���")]
    //�漳�� InputField
    public InputField inputFieldRoomDescription;
    //���̸� InputField
    public InputField inputRoomName;
    //��й�ȣ InputField
    public InputField inputPassword;
    //���ο� InputField
    public InputField inputMaxPlayer;
    //������ Button
    public Button btnJoin;
    //����� Button
    public Button btnCreate;

    public MakingChattingRoom doorCheck;
    //���� ������   
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //�� ����Ʈ Content

    [Header("�� ����Ʈ")]
    public GameObject roomItemFactory;
    public Transform trListContent;

    public LoadGallery loadGallery;

    //map Thumbnail
    public GameObject[] mapThumbs;

    [Header("�游��� �� �� ����Ʈ")]
    public GameObject setRoom;
    public GameObject setRoomlist;

    [Header("ç���� �Ⱓ")]
    public Text textCalendar;
    public UnityCalendar unityCalendar;
    public Dropdown dropdown;

    [Header("���� �ð�")]
    [Header("���� �ð�")]
    public string startDate;
    DateTime dateTime;
    DateTime dateTime1;
    #region ���� ����
    [Header("���� ����")]
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
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
        dropdown.onValueChanged.AddListener(delegate { HandleInputData(dropdown.value); });
        // ���̸�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // ���ο�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
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
        //����
        //  btnJoin.interactable = s.Length > 0;
        //����
        btnCreate.interactable = s.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    public void OnMaxPlayerValueChanged(string s)
    {
        //����
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0;
    }

    public void CreateChatroom()
    {
        //mapThumbs.texture = loadGallery.gameObject.GetComponent<RawImage>().texture;
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        roomOptions.MaxPlayers = 4;
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = false;
        
        PhotonNetwork.JoinOrCreateRoom("Room",roomOptions, null);
       
    }
        
    //�� ����
    public void CreateRoom()
    {
        //mapThumbs.texture = loadGallery.gameObject.GetComponent<RawImage>().texture;
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text);
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = true;

        // custom ������ ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

        hash["desc"] = $"ȸ�� ����: {monText}{tueText}{wedText}{thuText}{friText}{sunText}{sunText}\r\n�� ����: {inputFieldRoomDescription.text}";
        hash["map_id"] = UnityEngine.Random.Range(0, mapThumbs.Length);
        hash["room_name"] = inputRoomName.text;
        hash["password"] = inputPassword.text;
        hash["date"] = textCalendar.text;
        hash["TimerData"] = startDate;


        roomOptions.CustomRoomProperties = hash;
        // custom ������ �����ϴ� ����
        roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "desc", "map_id", "room_name", "password", "date", "TimerData"
        };

        // �� ���� ��û (�ش� �ɼ��� �̿��ؼ�)
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions);
    }

    //���� �����Ǹ� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        
        print("OnCreatedRoom");
    }

    //�� ������ ���� �ɶ� ȣ�� �Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);
    }

    //�� ���� ��û
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
    }

    //�� ������ �Ϸ� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
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

    //�� ������ ���� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

    //�濡 ���� ������ ����Ǹ� ȣ�� �Ǵ� �Լ�(�߰�/����/����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //�븮��Ʈ UI �� ��ü����
        DeleteRoomListUI();
        //�븮��Ʈ ������ ������Ʈ
        UpdateRoomCache(roomList);
        //�븮��Ʈ UI ��ü ����
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
            // ����, ����
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //���࿡ �ش� ���� �����Ȱ��̶��
                if (roomList[i].RemovedFromList)
                {
                    //roomCache ���� �ش� ������ ����
                    roomCache.Remove(roomList[i].Name);
                }
                //�׷��� �ʴٸ�
                else
                {
                    //���� ����
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            //�߰�
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

            //������� �����.
            GameObject go = Instantiate(roomItemFactory, trListContent);
            //������� ������ ����(������(0/0))
            RoomItem item = go.GetComponent<RoomItem>();
            item.SetInfo(info);

            //roomItem ��ư�� Ŭ���Ǹ� ȣ��Ǵ� �Լ� ���
            item.onClickAction = SetRoomName;
            //���ٽ�
            //item.onClickAction = (string room) => {
            //    inputRoomName.text = room;
            //};

            string desc = (string)info.CustomProperties["desc"];
            int map_id = (int)info.CustomProperties["map_id"];
            print(desc + ", " + map_id);
        }
    }
    //���� Thumbnail id
    int prevMapId = -1;
    void SetRoomName(string room, int map_id)
    {
        //���̸� ����
        inputRoomName.text = room;

        //���࿡ ���� �� Thumbnail�� Ȱ��ȭ�� �Ǿ��ִٸ�
        if (prevMapId > -1)
        {
            //���� �� Thumbnail�� ��Ȱ��ȭ
            mapThumbs[prevMapId].SetActive(false);
        }

        //�� Thumbnail ����
        mapThumbs[map_id].SetActive(true);

        //���� �� id ����
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
        //Debug.Log("��: " + isCheckMon +"\r\nȭ: " + isCheckTue + "\r\n��: "+ isCheckWed+ "\r\n��: "+ isCheckThu+"\r\n��: "+isCheckFri+"\r\n��: "+isCheckSat+"\r\n��:"+isCheckSun);
        if (toggleMon.isOn)
        {
            isCheckMon = true;
            monText = "��";
        }
        else
        {
            isCheckMon = false;
            monText = "";
        }
        if (toggleTue.isOn)
        {
            isCheckTue = true;
            tueText = "ȭ";
        }
        else
        {
            isCheckTue = false;
            thuText = "";
        }
        if (toggleWed.isOn)
        {
            isCheckWed = true;
            wedText = "��";
        }
        else
        {
            isCheckWed = false;
            wedText = "";
        }
        if (toggleThu.isOn)
        {
            isCheckThu = true;
            thuText = "��";
        }
        else
        {
            isCheckThu = false;
            thuText = "";
        }
        if (toggleFri.isOn)
        {
            isCheckFri = true;
            friText = "��";
        }
        else
        {
            isCheckFri = false;
            friText = "";
        }
        if (toggleSat.isOn)
        {
            isCheckSat = true;
            satText = "��";
        }
        else
        {
            isCheckSat = false;
            satText = "";
        }
        if (toggleSun.isOn)
        {
            isCheckSun = true;
            sunText = "��";
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

        //HttpManager���� ��û
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


