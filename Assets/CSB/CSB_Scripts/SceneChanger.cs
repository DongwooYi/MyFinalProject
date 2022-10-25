using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class SceneChanger : MonoBehaviour
{
    // ID InputField
    public InputField inputID;

    // Enter Button
    public Button enterButton;


    void Start()
    {
        inputID.onValueChanged.AddListener(OnValueChanged);

        // inpunNickName 에서 Enter 키 누르면 호출되는 함수 등록
        inputID.onSubmit.AddListener(OnSubmit);

        // inputNickName 에서 Focusing 이 사라졌을ㄹ 때 호출되는 함수 등록
        inputID.onEndEdit.AddListener(OnEndEdit);

    }

    void Update()
    {
    }

    void OnValueChanged(string str)
    {
        enterButton.interactable = str.Length > 0;
    }

    void OnSubmit(string str)
    {
        if (str.Length > 0)
        {
            OnClickConnect();
        }
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);

    }

    public void OnClickConnect()
    {
        //SceneManager.LoadScene("CSB_MyProfile");
        SceneManager.LoadScene(1);
    }

    #region HTTP 로그인 
    public InputField id;
    public InputField pw;

    public void OnClickUserLogin()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = gameObject.AddComponent<HttpRequester>();

        ///post/1, GET, 완료되었을 때 호출되는 함수
        requester.url = "";

        LoginData ldata = new()
        {
            id = id.text,
            pw = pw.text
        };

        requester.data = JsonUtility.ToJson(ldata, true);
        requester.requestType = RequestType.POST;
        requester.onComplete = OnComplteLogin;

        //응답을 받아서 출력하자
        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester);
    }

    public void OnComplteLogin(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        bool type = (bool)jObject["results"]["type"];
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        string token = (string)jObject["results"]["data"]["token"];

        // 통신 성공
        if (type)
        {
            // 1. PlayerPref에 key는 jwt, value는 token 바로 씬으로 이동
            PlayerPrefs.SetString("jwt", token);
            SceneManager.LoadScene(1);
        }
    }

    #endregion
}
