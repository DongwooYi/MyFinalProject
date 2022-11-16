using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("�븮��Ʈ")]
    public Text welcomeText;
    public Text lobbyInfoText;
    public Text welcomeTextInPersonalNote;
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
    [Header("ĳ���� ����")]
  public  YDW_CharacterController YDW_CharacterController;
    //���� ������   
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //�� ����Ʈ Content
    public Transform trListContent;

   public LoadGallery loadGallery;

    //map Thumbnail
    public GameObject[] Picture;

    [Header("�游��� �� �� ����Ʈ")]
    public GameObject setRoom;
    public GameObject setRoomlist;
  public  DataManager DataManager;

    [Header("ç���� �Ⱓ")]
    public UnityCalendar unityCalendar;
    public Text textCalendar;
    void Start()
    {
     
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
        welcomeTextInPersonalNote.text = PhotonNetwork.LocalPlayer.NickName + " �� ��Ʈ";
            /*        if (DataManager == null || DataManager.isActiveAndEnabled == false)
                    {
                        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
                        return;
                    }*/
            if(DataManager== null || DataManager.isActiveAndEnabled == false)
        {
            return;
        }
            DataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        if (DataManager.SetActiveMakingRoom)
        {
            setRoom.SetActive(true);
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        }
        else if (DataManager.ShowRoomlist)
        {
            setRoomlist.SetActive(false);
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        }
        
        
        // ���̸�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // ���ο�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);
        string[] s = Microphone.devices;
    }
    private void Update()
    {
        lobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� /" + PhotonNetwork.CountOfPlayers + "����";
        if (YDW_CharacterController.ischeckDoor)
        {
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);
            CreateChatroom();
        }
        if(Input.GetKeyDown(KeyCode.F10))
        {
            PlayerPrefs.DeleteAll();
        }
        if(Input.GetKeyDown(KeyCode.F9))
        {
            setRoom.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.F8))
        {
            setRoomlist.SetActive(true);
        }
    }
    public void OnRoomNameValueChanged(string s)
    {
        //����
        btnJoin.interactable = s.Length > 0;
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
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom("ChatRoom", roomOptions, null);
        YDW_CharacterController.ischeckDoor = false;
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
        
        hash["desc"] = inputFieldRoomDescription.text;
        hash["map_id"] = UnityEngine.Random.Range(0, Picture.Length);
        hash["room_name"] = inputRoomName.text;
        hash["password"] = inputPassword.text;
        roomOptions.CustomRoomProperties = hash;
        // custom ������ �����ϴ� ����
        roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "desc", "map_id", "room_name", "password"
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
        if (currentRoomname == "ChatRoom")
        {
            PhotonNetwork.LoadLevel("SB_Player_Photon");
        }
        else
        {
            PhotonNetwork.LoadLevel("SB_Player_Photon");

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

    public GameObject roomItemFactory;
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

       /* //���࿡ ���� �� Thumbnail�� Ȱ��ȭ�� �Ǿ��ִٸ�
        if (prevMapId > -1)
        {
            //���� �� Thumbnail�� ��Ȱ��ȭ
            mapThumbs[prevMapId].SetActive(false);
        }

        //�� Thumbnail ����
        mapThumbs[map_id].SetActive(true);*/

        //���� �� id ����
        prevMapId = map_id;
    }

    public void OnClick_GetDate()
    {
        DateTime dt = unityCalendar.GetDate();
        textCalendar.text ="~"+ dt.ToString("yyyy-MM-dd");
    }

    public void OnClick_Clear()
    {
        textCalendar.text = string.Empty;
        unityCalendar.Init();
    }
}
