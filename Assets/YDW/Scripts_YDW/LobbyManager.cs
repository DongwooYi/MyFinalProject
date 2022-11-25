using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.IO;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("�븮��Ʈ")]
    public Text welcomeText;

    [Header("���� ����")]
    // ���ο���

    [Header("���� �� ���� �ʿ� ���")]
    //�漳�� InputField
    public InputField inputFieldRoomDescription;

    //������ �漳��
    public InputField inputFieldRoomDescriptionShortForm;
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

    [Header("�游��� �� �� ����Ʈ")]
    public GameObject setRoom;
    public GameObject setRoomlist;
    public GameObject FailCreateaRoom;

    [Header("ç���� �Ⱓ")]
    public Text textCalendar;
    public UnityCalendar unityCalendar;
    public Dropdown dropdown;

    [Header("���� �ð�")]
    [Header("���� �ð�")]
    public string startDate;
    DateTime dateTime;
    DateTime dateTime1;

    public byte[] img;
    public WorldManager2D worldManager2D;


    public GameObject readerMate;
    public GameObject readerMateImage;

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
        textPayernameinreafermatePannel.text = PhotonNetwork.LocalPlayer.NickName + "����\r\n���� ����Ʈ��?";
        dropdown.onValueChanged.AddListener(delegate { HandleInputData(dropdown.value); });
        // ���̸�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // ���ο�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);
        string[] s = Microphone.devices;
        Debug.Log(Application.persistentDataPath);
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
            CreateChatroom();
            doorCheck.GotoMainWorld = false;
            Debug.Log("CreateChatroom");
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            setRoom.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            setRoomlist.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            setRoomlist.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CreateChatroom();
        }
        ToggleCheck();
    }

    public void SetactiveRoomCreatationPannel()
    {
        setRoom.SetActive(true);
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

        PhotonNetwork.JoinOrCreateRoom("Room", roomOptions, null);

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
        hash["map_id"] = img;
        hash["room_name"] = inputRoomName.text;
        hash["date"] = textCalendar.text;
        hash["descShortForm"] = inputFieldRoomDescriptionShortForm.text;
        hash["DDay"] = startDate;

        roomOptions.CustomRoomProperties = hash;
        // custom ������ �����ϴ� ����
        roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "desc", "map_id", "room_name", "date", "descShortForm", "DDay"
        };

        print("img�迭��" + img.Length);
        // �� ���� ��û (�ش� �ɼ��� �̿��ؼ�)
        PhotonNetwork.CreateRoom(inputRoomName.text, roomOptions);
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
            print(desc);
        }
    }
    //���� Thumbnail id
    int prevMapId = -1;
    void SetRoomName(string room)
    {
        //���̸� ����
        inputRoomName.text = room;
    }
    #region ��������Ʈ setactive

    [Header("��������Ʈ ��õ")]
    public Text textPayernameinreafermatePannel;
    public Transform transformInReadermateImg;
    public void OnclickOpenReaderMatePannel()
    {
        readerMate.SetActive(true);
    }
    public void ReaderRecommendation()
    {
        HttpRequester requester = new HttpRequester();
        requester.url = "http://15.165.28.206:80/v1/friends";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetPostAll;

        //HttpManager���� ��û
        Debug.Log("Get ����");
        HttpManager.instance.SendRequest(requester, "application/json");

    }
    public void OnCompleteGetPostAll(DownloadHandler downloadHandler)
    {

        //List<>
        MemeberDataDetail array = JsonUtility.FromJson<MemeberDataDetail>(downloadHandler.text);
        int okay = array.status;
        if (okay == 200)
        {
            readerMate.SetActive(false);
            readerMateImage.SetActive(true);
            for (int i = 0; i < 4; i++)
            {
                transformInReadermateImg.transform.GetChild(i).GetComponent<Text>().text = $"{array.data[i].name}\r\n{array.data[i].records[i].bookName}";
                print(array.data[i].name + "\n" + array.data[i].records[i].bookName);

            }

        }

        print("��ȸ �Ϸ�");
    }
    public void GoBacktoMainWorld()
    {
        setRoomlist.SetActive(false);
        CreateChatroom();
    }
    #endregion
    #region ��¥
    DateTime dt;
    public void OnClick_GetDate()
    {
        dt = unityCalendar.GetDate();
        if (dt >= dateTime)
        {
            textCalendar.text = startDate + "~" + dt.ToString("yyyy-MM-dd");
        }
        else
        {
            textCalendar.text = "���� �����Ⱓ�� �������ּ���";
        }
        //print("dt"+dt+"dateTime:"+dateTime);
    }
    public void OnClick_Clear()
    {
        textCalendar.text = string.Empty;
        unityCalendar.Init();
    }
    #endregion
    #region �����Ⱓ
    public string recruitDate;
    public void HandleInputData(int val)
    {
        if (val == 0)
        {

            dateTime1 = dateTime.AddHours(24);
            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            recruitDate = dateTime.ToString("yyyy-MM-ddTHH:mm");
            Debug.Log("1��:" + startDate);

        }
        if (val == 1)
        {

            dateTime1 = dateTime.AddHours(72);

            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            recruitDate = dateTime.ToString("yyyy-MM-ddTHH:mm");
            print("72");
            Debug.Log("3��:" + startDate);

        }
        if (val == 2)
        {

            dateTime1 = dateTime.AddHours(168);
            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            recruitDate = dateTime.ToString("yyyy-MM-ddTHH:mm");
            print("168");
            Debug.Log("7��" + startDate);

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
    #endregion
    #region �̹���
    [Header("�̹���")]
    public GameObject lobbyManager;
    public RawImage image;
    public void OnClickImageLoad()
    {

        NativeGallery.GetImageFromGallery((file) =>
        {
            //�̹��� ����
            FileInfo selected = new FileInfo(file);

            //�̹��� �뷮 ����(������ ���� ������ֱ⿡ ����)
            if (selected.Length > 5000000)
            {
                return;
            }

            if (!string.IsNullOrEmpty(file))
            {
                // �ҷ�����
                StartCoroutine(LoadImage(file));
            }

        });
    }
    IEnumerator LoadImage(string path)
    {
        yield return null;
        byte[] fileData = File.ReadAllBytes(path);
        // Ȯ������ �̸� �� �ʿ����
        string fileName = Path.GetFileName(path).Split('.')[0];
        //������ �̹���
        string savePath = Application.persistentDataPath + "/Image";
        // ���� ���� ������ ���� ��ΰ� ���ٸ� ���� ��θ� ������
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        // ���� ���ϴ� ����� PNG ������ ���� �̸����� ����
        File.WriteAllBytes(savePath + fileName + ".png", fileData);

        var temp = File.ReadAllBytes(savePath + fileName + ".png");
        img = File.ReadAllBytes(savePath + fileName + ".png");
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(temp);
        image.texture = tex;

    }
    #endregion
    #region Http Web
    public void SendRoomData()
    {
        StartCoroutine("SendRoomDataCoroutine");
    }

    IEnumerator SendRoomDataCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("clubName", inputRoomName.text);
        form.AddField("bookName", "�̵���");
        form.AddField("clubIntro", inputFieldRoomDescription.text);
        form.AddField("numberOfMember", inputMaxPlayer.text);
        form.AddField("recruitStartDate", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"));
        form.AddField("recruitEndDate", recruitDate);
        form.AddField("startDate", recruitDate);
        form.AddField("endDate", dt.ToString("yyyy-MM-ddTHH:mm"));
        print("recruitStartDate" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm") + "\r\n" + "startDate" + startDate + "\r\n" + dt.ToString("yyyy-MM-ddTHH:mm"));

        form.AddField("dayOfWeeks", $"{monText}{tueText}{wedText}{thuText}{friText}{satText}{sunText}");
        form.AddBinaryData("imgFile", img);

        // UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.11:8080/v1/clubs", form);
        UnityWebRequest www = UnityWebRequest.Post("http://15.165.28.206:80/v1/clubs", form);
        www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("jwt"));
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            StartCoroutine(SetactiveRoomCreation());
        }
        else
        {
            print("Post����");
            CreateRoom();
        }
    }
    IEnumerator SetactiveRoomCreation()
    {
        FailCreateaRoom.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        FailCreateaRoom.SetActive(false);
    }
    #endregion
    #region HTTP Json
    public void SendRoomDataFunction()
    {

        RoomData roomData = new RoomData();
        roomData.clubName = inputRoomName.text;
        roomData.bookName = "�̵���";
        roomData.clubIntro = inputFieldRoomDescription.text;
        roomData.numberOfMember = inputMaxPlayer.text;
        roomData.recruitStartDate = DateTime.Now.ToString();
        roomData.recruitEndDate = startDate;
        roomData.startDate = startDate;
        roomData.endDate = dt.ToString("yyyy-MM-dd");
        roomData.dayOfWeeks = $"{monText}{tueText}{wedText}{thuText}{friText}{satText}{sunText}";
        roomData.imgFile = img;

        /*roomData.clubName = "Ŭ���̸�";
        roomData.bookName = "å�̸�";
        roomData.clubIntro = "�漳��";
        roomData.numberOfMember = "10";
        roomData.recruitStartDate = "2022-11-20T14:00";
        roomData.recruitEndDate = "2022-11-22T14:00";
        roomData.startDate = "2022-11-22T14:00";
        roomData.endDate = "2022-11-30T14:00";
        roomData.dayOfWeeks = "������";
        roomData.imgFile = img;*/


        HttpRequester requester = new HttpRequester();
        requester.url = "http://192.168.0.11:8080/v1/clubs";
        requester.requestType = RequestType.POST;
        requester.body = JsonUtility.ToJson(roomData, true);
        requester.onComplete = OnCompletePostRoomData;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
        //HttpManager.instance.SendRequest(www, "application/x-www-form-urlencoded");

    }
    public void OnCompletePostRoomData(DownloadHandler downloadHandler)
    {
        JObject jObject = JObject.Parse(downloadHandler.text);

        int type = (int)jObject["status"];
        if (type == 200)
        {
            Debug.Log("����");
            Debug.Log(downloadHandler.text);

        }
    }
    #endregion
    #region �������� �� 
    public void MyRoomJoinedList()
    {
        StartCoroutine(GetRequest("http://192.168.0.11:80/v1/clubs?option=1"));
    }

    IEnumerator GetRequest(string URL)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("jwt"));
            yield return webRequest.SendWebRequest();

            if (webRequest.error == null)  // ������ ���� ������ ����.
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("error");
            }
        }
    }
    #endregion
  
}





