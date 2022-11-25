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
    [Header("룸리스트")]
    public Text welcomeText;

    [Header("메인 광장")]
    // 메인월드

    [Header("포톤 방 생성 필요 목록")]
    //방설명 InputField
    public InputField inputFieldRoomDescription;

    //간단한 방설명
    public InputField inputFieldRoomDescriptionShortForm;
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

    [Header("방만들기 및 방 리스트")]
    public GameObject setRoom;
    public GameObject setRoomlist;
    public GameObject FailCreateaRoom;

    [Header("챌린지 기간")]
    public Text textCalendar;
    public UnityCalendar unityCalendar;
    public Dropdown dropdown;

    [Header("시작 시간")]
    [Header("시작 시간")]
    public string startDate;
    DateTime dateTime;
    DateTime dateTime1;

    public byte[] img;
    public WorldManager2D worldManager2D;


    public GameObject readerMate;
    public GameObject readerMateImage;

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
        textPayernameinreafermatePannel.text = PhotonNetwork.LocalPlayer.NickName + "님의\r\n독서 메이트는?";
        dropdown.onValueChanged.AddListener(delegate { HandleInputData(dropdown.value); });
        // 방이름(InputField)이 변경될때 호출되는 함수 등록
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // 총인원(InputField)이 변경될때 호출되는 함수 등록
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

        PhotonNetwork.JoinOrCreateRoom("Room", roomOptions, null);

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
        hash["map_id"] = img;
        hash["room_name"] = inputRoomName.text;
        hash["date"] = textCalendar.text;
        hash["descShortForm"] = inputFieldRoomDescriptionShortForm.text;
        hash["DDay"] = startDate;

        roomOptions.CustomRoomProperties = hash;
        // custom 정보를 공개하는 설정
        roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "desc", "map_id", "room_name", "date", "descShortForm", "DDay"
        };

        print("img배열의" + img.Length);
        // 방 생성 요청 (해당 옵션을 이용해서)
        PhotonNetwork.CreateRoom(inputRoomName.text, roomOptions);
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
            print(desc);
        }
    }
    //이전 Thumbnail id
    int prevMapId = -1;
    void SetRoomName(string room)
    {
        //룸이름 설정
        inputRoomName.text = room;
    }
    #region 독서메이트 setactive

    [Header("독서메이트 추천")]
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

        //HttpManager에게 요청
        Debug.Log("Get 실행");
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

        print("조회 완료");
    }
    public void GoBacktoMainWorld()
    {
        setRoomlist.SetActive(false);
        CreateChatroom();
    }
    #endregion
    #region 날짜
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
            textCalendar.text = "먼저 모집기간을 설정해주세요";
        }
        //print("dt"+dt+"dateTime:"+dateTime);
    }
    public void OnClick_Clear()
    {
        textCalendar.text = string.Empty;
        unityCalendar.Init();
    }
    #endregion
    #region 모집기간
    public string recruitDate;
    public void HandleInputData(int val)
    {
        if (val == 0)
        {

            dateTime1 = dateTime.AddHours(24);
            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            recruitDate = dateTime.ToString("yyyy-MM-ddTHH:mm");
            Debug.Log("1일:" + startDate);

        }
        if (val == 1)
        {

            dateTime1 = dateTime.AddHours(72);

            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            recruitDate = dateTime.ToString("yyyy-MM-ddTHH:mm");
            print("72");
            Debug.Log("3일:" + startDate);

        }
        if (val == 2)
        {

            dateTime1 = dateTime.AddHours(168);
            startDate = $"{dateTime1.Year}-{dateTime1.Month}-{dateTime1.Day}";
            recruitDate = dateTime.ToString("yyyy-MM-ddTHH:mm");
            print("168");
            Debug.Log("7일" + startDate);

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
    #endregion
    #region 이미지
    [Header("이미지")]
    public GameObject lobbyManager;
    public RawImage image;
    public void OnClickImageLoad()
    {

        NativeGallery.GetImageFromGallery((file) =>
        {
            //이미지 정보
            FileInfo selected = new FileInfo(file);

            //이미지 용량 제한(나중의 문제 생길수있기에 예방)
            if (selected.Length > 5000000)
            {
                return;
            }

            if (!string.IsNullOrEmpty(file))
            {
                // 불러오기
                StartCoroutine(LoadImage(file));
            }

        });
    }
    IEnumerator LoadImage(string path)
    {
        yield return null;
        byte[] fileData = File.ReadAllBytes(path);
        // 확장자의 이름 은 필요없음
        string fileName = Path.GetFileName(path).Split('.')[0];
        //설정된 이미지
        string savePath = Application.persistentDataPath + "/Image";
        // 만약 내가 지정한 저장 경로가 없다면 지정 경로를 만들어라
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        // 내가 원하는 장소의 PNG 형식의 파일 이름으로 저장
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
        form.AddField("bookName", "이동우");
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
            print("Post성공");
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
        roomData.bookName = "이동우";
        roomData.clubIntro = inputFieldRoomDescription.text;
        roomData.numberOfMember = inputMaxPlayer.text;
        roomData.recruitStartDate = DateTime.Now.ToString();
        roomData.recruitEndDate = startDate;
        roomData.startDate = startDate;
        roomData.endDate = dt.ToString("yyyy-MM-dd");
        roomData.dayOfWeeks = $"{monText}{tueText}{wedText}{thuText}{friText}{satText}{sunText}";
        roomData.imgFile = img;

        /*roomData.clubName = "클럽이름";
        roomData.bookName = "책이름";
        roomData.clubIntro = "방설명";
        roomData.numberOfMember = "10";
        roomData.recruitStartDate = "2022-11-20T14:00";
        roomData.recruitEndDate = "2022-11-22T14:00";
        roomData.startDate = "2022-11-22T14:00";
        roomData.endDate = "2022-11-30T14:00";
        roomData.dayOfWeeks = "월수금";
        roomData.imgFile = img;*/


        HttpRequester requester = new HttpRequester();
        requester.url = "http://192.168.0.11:8080/v1/clubs";
        requester.requestType = RequestType.POST;
        requester.body = JsonUtility.ToJson(roomData, true);
        requester.onComplete = OnCompletePostRoomData;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
        //HttpManager.instance.SendRequest(www, "application/x-www-form-urlencoded");

    }
    public void OnCompletePostRoomData(DownloadHandler downloadHandler)
    {
        JObject jObject = JObject.Parse(downloadHandler.text);

        int type = (int)jObject["status"];
        if (type == 200)
        {
            Debug.Log("성공");
            Debug.Log(downloadHandler.text);

        }
    }
    #endregion
    #region 참여중인 방 
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

            if (webRequest.error == null)  // 에러가 나지 않으면 동작.
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





