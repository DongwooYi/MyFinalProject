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
    #region HTTP �α��� 
    [Header("�α��� �� ȸ������")]
    public GameObject loginImage;
    public GameObject registerImage;
    [Header("�α���")]
    public InputField id;
    public InputField pw;
    private void Start()
    {
        registerImage.SetActive(false);
    }
    public void OnClickUserLogin()
    {
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����

        //HttpRequester requester = new HttpRequester();

        HttpRequester requester = gameObject.AddComponent<HttpRequester>();

        ///post/1, GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:8080/v1/auths/login";

        LoginData ldata = new()
        {
            memberId = id.text,
            memberPwd = pw.text
        };

        requester.body = JsonUtility.ToJson(ldata, true);
        requester.requestType = RequestType.LOGIN;
        requester.onComplete = OnComplteLogin;

        //������ �޾Ƽ� �������
        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }
    public void OnComplteLogin(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];
        string token = (string)jObject["data"]["accessToken"];
        
        // ��� ����
        if (type==200)
        {
            HttpManager.instance.nickName = (string)jObject["data"]["membersName"];
            print("��ż���");
            // 1. PlayerPref�� key�� jwt, value�� token
            PlayerPrefs.SetString("jwt", token);
            print("token��" + token);
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
    //������ ���� ���Ӽ����� ȣ��(Lobby�� ������ �� ���� ����)
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //�κ� ���� ��û
        PhotonNetwork.JoinLobby();
    }
    //�κ� ���� ������ ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    #endregion
}