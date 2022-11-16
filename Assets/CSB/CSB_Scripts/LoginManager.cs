using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Photon.Pun;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    #region HTTP 로그인 
    [Header("로그인 및 회원가입")]
    public GameObject loginImage;
    public GameObject registerImage;
    [Header("로그인")]
    public InputField id;
    public InputField pw;
    private void Start()
    {
        registerImage.SetActive(false);
    }
    public void OnClickUserLogin()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성

        //HttpRequester requester = new HttpRequester();

        HttpRequester requester = gameObject.AddComponent<HttpRequester>();

        ///post/1, GET, 완료되었을 때 호출되는 함수
        requester.url = "http://15.165.28.206:8080/v1/auths/login";

        LoginData ldata = new()
        {
            memberId = id.text,
            memberPwd = pw.text
        };

        requester.body = JsonUtility.ToJson(ldata, true);
        requester.requestType = RequestType.LOGIN;
        requester.onComplete = OnComplteLogin;

        //응답을 받아서 출력하자
        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
    }
    public void OnComplteLogin(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];
        string token = (string)jObject["data"]["accessToken"];
        
        // 통신 성공
        if (type==200)
        {
            HttpManager.instance.nickName = (string)jObject["data"]["membersName"];
            print("통신성공");
            // 1. PlayerPref에 key는 jwt, value는 token
            PlayerPrefs.SetString("jwt", token);
            print("token값" + token);
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void Toregister()
    {
        registerImage.SetActive(true);
    }
    public void OkaytoLogin()
    {
        registerImage.SetActive(false);
    }
    #endregion

    #region Photon
    //마스터 서버 접속성공시 호출(Lobby에 진입할 수 없는 상태)
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //내 닉네임 설정        
        //PhotonNetwork.NickName = 
        //로비 진입 요청
        PhotonNetwork.JoinLobby();
    }
    //로비 진입 성공시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        CreateChatroom();
    }
    public void CreateChatroom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom("ChatRoom", roomOptions, null);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        JoinRoom();
    }

    /* 방 참가 */
    public void JoinRoom()
    {
        // 1 방 참가 '요청'
        // PhotonNetwork.JoinRoom("XR_B반");
        PhotonNetwork.JoinRoom("ChatRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel("LobbyScene");
    }
    #endregion
}
