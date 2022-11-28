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
    public InputField inputBookName;
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

    public GameObject roomDetailDesc;
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
        doneCreateRoomCount = 0;
        timeSelectionPannel.SetActive(false);
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
        textPayernameinreafermatePannel.text = PhotonNetwork.LocalPlayer.NickName + "����\r\n���� ����Ʈ��?";
        dropdown.onValueChanged.AddListener(delegate { HandleInputData(dropdown.value); });
        // ���̸�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // ���ο�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);
        CreateRoomFunctionOkayPannel.SetActive(false);
        string[] s = Microphone.devices;
        Debug.Log(Application.persistentDataPath);
        AMPM.onValueChanged.AddListener(delegate { AMPMToggleCheck(AMPM); });
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
        ToggleCheck();

    }

    #region Photon �游���
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

        hash["desc"] = inputFieldRoomDescription.text;
        hash["map_id"] = img;
        hash["room_name"] = inputRoomName.text;
        hash["date"] = textCalendar.text;
        hash["descShortForm"] = inputFieldRoomDescriptionShortForm.text;
        hash["DDay"] = startDate;
        hash["roomHost_Name"] = PhotonNetwork.LocalPlayer.NickName;
        hash["book_Name"] = inputBookName.text;
        hash["meetingTime"] = textTimeforMeeting.text;
        hash["dayOfWeeks"] = $"ȸ�� ����: {monText}{tueText}{wedText}{thuText}{friText}{sunText}{sunText}";
        roomOptions.CustomRoomProperties = hash;

        // custom ������ �����ϴ� ����
        roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "desc", "map_id", "room_name", "date", "descShortForm", "DDay","roomHost_Name", "book_Name", "meetingTime", "dayOfWeeks"
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
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }

    //�� ������ �Ϸ� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameObject game = GameObject.FindWithTag("RoomDesc");
        DestroyImmediate(game);
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
    void SetRoomName(string room)
    {
        //���̸� ����
        inputRoomName.text = room;
    }
    #endregion
    #region ��������Ʈ setactive

    [Header("��������Ʈ ��õ")]
    public Text textPayernameinreafermatePannel;

   
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
    public List<GameObject> remate;
    public void OnCompleteGetPostAll(DownloadHandler downloadHandler)
    {

        //List<>
        MemeberDataDetail array = JsonUtility.FromJson<MemeberDataDetail>(downloadHandler.text);
        int okay = array.status;
        if (okay == 200)
        {
            readerMate.SetActive(false);
            readerMateImage.SetActive(true);
            for (int i = 0; i < remate.Count; i++)
            {

                remate[i].GetComponentInChildren<Text>().text = $"{array.data[i].name}\r\n'{array.data[i].records[i].bookName}'�� �д� ��";
                print(array.data[i].name + "\n" + array.data[i].records[i].bookName);

            }

        }

        print("��ȸ �Ϸ�");
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
        form.AddField("bookName", inputBookName.text);
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
            CreateRoomFunctionOkayPannel.SetActive(true);
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
    #region ç���� ���� �ð�
    [Header("ç���� ���� �ð�")]
    public GameObject timeSelectionPannel;
    public Button[] buttonforSelcetedime;
    public Button[] buttonforSelcetedEndTime;
    public Button[] buttoonforSelectedMin;
    public Button[] buttoonforSelectedEndMin;
    public Toggle AMPM;
    public void OnClickEventTImeSelection()
    {
        timeSelectionPannel.SetActive(true);
        AMPM.isOn = false;
        time();
    }
    int indexstartHour = 1;
    int indexstartMin = 0;
    int indexendMin = 0;
    void time()
    {
        indexstartHour = 1;
        indexstartMin = 0;
        indexendMin = 0;
        for (int i = 0; i < buttonforSelcetedime.Length; i++)
        {

            buttonforSelcetedime[i].GetComponentInChildren<Text>().text = indexstartHour + "��";
            int index = i;
            buttonforSelcetedime[index].onClick.AddListener(() => TaskOnClickstartHour(index));
            indexstartHour++;
        }
        for (int i = 0; i < buttoonforSelectedMin.Length; i++)
        {
            buttoonforSelectedMin[i].GetComponentInChildren<Text>().text = indexstartMin + "��";
            int index = i;
            buttoonforSelectedMin[index].onClick.AddListener(() => TaskOnClickstartMin(index));
            indexstartMin += 15;
        }
        for (int i = 0; i < buttoonforSelectedEndMin.Length; i++)
        {
            buttoonforSelectedEndMin[i].GetComponentInChildren<Text>().text = indexendMin + "��";
            int index = i;
            buttoonforSelectedEndMin[index].onClick.AddListener(() => TaskOnClickMinEnd(index));
            indexendMin += 15;
        }

    }
    public string startHour, startMin, endHour, endMin, AMPMCheckString;
    int startHourint, endHourint, startMinInt, endMinInt;
    public void TaskOnClickstartHour(int buttonIndex)
    {
        startHour = buttonforSelcetedime[buttonIndex].GetComponentInChildren<Text>().text + "��";
        startHourint = buttonIndex + 1;
        if (startHourint == 10)
        {
            buttonforSelcetedEndTime[0].GetComponentInChildren<Text>().text = $"{11}";
            buttonforSelcetedEndTime[1].GetComponentInChildren<Text>().text = $"{12}";
            buttonforSelcetedEndTime[2].GetComponentInChildren<Text>().text = $"{1}";
        }
        else if (startHourint == 11)
        {
            buttonforSelcetedEndTime[0].GetComponentInChildren<Text>().text = $"{12}";
            buttonforSelcetedEndTime[1].GetComponentInChildren<Text>().text = $"{1}";
            buttonforSelcetedEndTime[2].GetComponentInChildren<Text>().text = $"{2}";
        }
        else if (startHourint == 12)
        {
            buttonforSelcetedEndTime[0].GetComponentInChildren<Text>().text = $"{1}";
            buttonforSelcetedEndTime[1].GetComponentInChildren<Text>().text = $"{2}";
            buttonforSelcetedEndTime[2].GetComponentInChildren<Text>().text = $"{3}";
        }
        else
        {
            buttonforSelcetedEndTime[0].GetComponentInChildren<Text>().text = $"{startHourint + 1}";
            buttonforSelcetedEndTime[1].GetComponentInChildren<Text>().text = $"{startHourint + 2}";
            buttonforSelcetedEndTime[2].GetComponentInChildren<Text>().text = $"{startHourint + 3}";
        }
        for (int i = 0; i < buttonforSelcetedEndTime.Length; i++)
        {
            int index = i;
            buttonforSelcetedEndTime[index].onClick.AddListener(() => TaskOnClickHourEnd(index));
        }
    }
    public void TaskOnClickstartMin(int buttonIndex)
    {
        indexendMin = 0;
        startMin = buttoonforSelectedMin[buttonIndex].GetComponentInChildren<Text>().text;
        startMinInt = buttonIndex * 15;

        if (ButtonforTaskOnClickHourEnd == 2)
        {
            if (startMinInt == 0)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "0��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = $"--";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = $"--";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = $"--";

            }
            else if (startMinInt == 15)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "00��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = "15��";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = $"--";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = $"--";
            }
            else if (startMinInt == 30)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "00��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = "15��";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = "30��";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = $"--";
            }
            else if (startMinInt == 45)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "00��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = "15��";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = "30��";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = "45��";
            }
        }
    }
    int ButtonforTaskOnClickHourEnd;
    public void TaskOnClickHourEnd(int buttonIndex)
    {
        indexendMin = 0;
        ButtonforTaskOnClickHourEnd = buttonIndex;
        endHour = buttonforSelcetedEndTime[buttonIndex].GetComponentInChildren<Text>().text;
        endHourint = int.Parse(endHour);
        print(ButtonforTaskOnClickHourEnd);
        if (buttonIndex == 2)
        {
            if (startMinInt == 0)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "0��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = $"--";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = $"--";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = $"--";

            }
            else if (startMinInt == 15)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "00��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = "15��";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = $"--";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = $"--";
            }
            else if (startMinInt == 30)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "00��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = "15��";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = "30��";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = $"--";
            }
            else if (startMinInt == 45)
            {
                buttoonforSelectedEndMin[0].GetComponentInChildren<Text>().text = "00��";
                buttoonforSelectedEndMin[1].GetComponentInChildren<Text>().text = "15��";
                buttoonforSelectedEndMin[2].GetComponentInChildren<Text>().text = "30��";
                buttoonforSelectedEndMin[3].GetComponentInChildren<Text>().text = "45��";
            }
        }
        else
        {
            for (int i = 0; i < buttoonforSelectedEndMin.Length; i++)
            {
                buttoonforSelectedEndMin[i].GetComponentInChildren<Text>().text = indexendMin + "��";
                int index = i;
                buttoonforSelectedEndMin[index].onClick.AddListener(() => TaskOnClickMinEnd(index));
                indexendMin += 15;
            }
        }
    }
    public void TaskOnClickMinEnd(int buttonIndex)
    {
        endMin = buttoonforSelectedEndMin[buttonIndex].GetComponentInChildren<Text>().text;
        endMinInt = buttonIndex * 15;
        print(endMin);
    }
    public void AMPMToggleCheck(Toggle toggle)
    {
        if (toggle.isOn)
        {
            AMPMCheckString = "AM";
        }
        else
        {
            AMPMCheckString = "PM";
        }
        print(AMPMCheckString);
    }
    public Text textTimeforMeeting;
    public void GetTime()
    {
        timeSelectionPannel.SetActive(false);
        if (endMin == "--")
        {
            textTimeforMeeting.text = "3�ð��� �ʰ��� �ð��Դϴ�.\r\n�ٽ� �ð��� �������ּ���.";
        }
        else if (AMPM.isOn)
        {
            if (startMinInt == 0)
            {
                if (endMinInt == 0)
                {
                    if (startHourint == 10)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~PM{endHourint}:0{endMinInt}";
                    }
                    else if (startHourint == 11)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~PM{endHourint}:0{endMinInt}";
                    }
                    else if (startHourint == 12)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~PM{endHourint}:0{endMinInt}";
                    }
                    else
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~{AMPMCheckString}{endHourint}:0{endMinInt}";
                    }
                }
                else
                {

                    if (startHourint == 10)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~PM{endHourint}:{endMinInt}";
                    }
                    else if (startHourint == 11)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~PM{endHourint}:{endMinInt}";
                    }
                    else if (startHourint == 12)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~PM{endHourint}:{endMinInt}";
                    }
                    else
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~{AMPMCheckString}{endHourint}:{endMinInt}";
                    }
                }
            }
            else if (startMinInt != 0 && endMinInt == 0)
            {
                if (startHourint == 10)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~PM{endHourint}:0{endMinInt}";
                }
                else if (startHourint == 11)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~PM{endHourint}:0{endMinInt}";
                }
                else if (startHourint == 12)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~PM{endHourint}:0{endMinInt}";
                }
                else
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~{AMPMCheckString}{endHourint}:0{endMinInt}";
                }
            }
            else
            {
                if (startHourint == 10)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~PM{endHourint}:{endMinInt}";
                }
                else if (startHourint == 11)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~PM{endHourint}:{endMinInt}";
                }
                else if (startHourint == 12)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~PM{endHourint}:{endMinInt}";
                }
                else
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~{AMPMCheckString}{endHourint}:{endMinInt}";
                }
            }
        }
        else
        {
            if (startMinInt == 0)
            {
                if (endMinInt == 0)
                {
                    if (startHourint == 10)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~AM{endHourint}:0{endMinInt}";
                    }
                    else if (startHourint == 11)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~AM{endHourint}:0{endMinInt}";
                    }
                    else if (startHourint == 12)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~AM{endHourint}:0{endMinInt}";
                    }
                    else
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~{AMPMCheckString}{endHourint}:0{endMinInt}";
                    }
                }
                else
                {

                    if (startHourint == 10)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~AM{endHourint}:{endMinInt}";
                    }
                    else if (startHourint == 11)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~AM{endHourint}:{endMinInt}";
                    }
                    else if (startHourint == 12)
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~AM{endHourint}:{endMinInt}";
                    }
                    else
                    {
                        textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:0{startMinInt}~{AMPMCheckString}{endHourint}:{endMinInt}";
                    }
                }
            }
            else if (startMinInt != 0 && endMinInt == 0)
            {
                if (startHourint == 10)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~AM{endHourint}:0{endMinInt}";
                }
                else if (startHourint == 11)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~AM{endHourint}:0{endMinInt}";
                }
                else if (startHourint == 12)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~AM{endHourint}:0{endMinInt}";
                }
                else
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~{AMPMCheckString}{endHourint}:0{endMinInt}";
                }
            }
            else
            {
                if (startHourint == 10)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~AM{endHourint}:{endMinInt}";
                }
                else if (startHourint == 11)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~AM{endHourint}:{endMinInt}";
                }
                else if (startHourint == 12)
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~AM{endHourint}:{endMinInt}";
                }
                else
                {
                    textTimeforMeeting.text = $"{AMPMCheckString}{startHourint}:{startMinInt}~{AMPMCheckString}{endHourint}:{endMinInt}";
                }
            }
            Debug.Log("��� AM/PM : " + AMPM.isOn + textTimeforMeeting.text);

        }
    }
    IEnumerator BlinkToggle()
    {
        int count = 0;
        while (count < 3)
        {
            AMPM.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            AMPM.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
    }
    #endregion
    #region �̺�Ʈ �Լ�
    [Header("���� ���� ��� �ǳ�")]
    public GameObject CancelPannel;
    public GameObject CreateRoomFunctionOkayPannel;
    public GameObject readerMatePannel;
    public void OnclickCancelRoomCreation()
    {
        CancelPannel.SetActive(true);
    }
    public void OnClickGoBacktoCreation()
    {
        CancelPannel.SetActive(false);
    }

    int doneCreateRoomCount = 0;
    public void GoBacktoMainWorld()
    {
        if(doneCreateRoomCount>0)
        {
            return;
        }
        setRoomlist.SetActive(false);
        CreateChatroom();
        doneCreateRoomCount++;
    }
    public void SetactiveRoomCreatationPannel()
    {

        setRoom.SetActive(true);
    }

    public void SetDeactiveRoomCreationPannel()
    {
        setRoom.SetActive(false);
        CancelPannel.SetActive(false);
        setRoomlist.SetActive(true);
    }
    public void BacktoRoomList()
    {
        readerMatePannel.SetActive(false);
        setRoomlist.SetActive(true);
    }
    public void OnclickOpenReaderMatePannel()
    {
        readerMate.SetActive(true);
    }
    public void OnclickOpenReaderMatePannelBack()
    {
        readerMate.SetActive(false);
    }
    #endregion
}







