using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{

    public InputField id;
    public InputField pw;
    public InputField cpw;
    public InputField nickname;
    public InputField digitNumber;
    public GameObject rgiOk;
    public Button buttongforregister;
    public GameObject Fail;
    //Regex regex = new Regex(@"^010[0-9]{4}[0-9]{4}$");

    private void Start()
    {
        buttongforregister.interactable = false;
       // digitNumber.characterLimit = 11;
    }

    private void Update()
    {
        
        if (pw.text == cpw.text && nickname.text.Length>0)
        {
            buttongforregister.interactable = true;
        }
        else
        {
            buttongforregister.interactable = false;
        }
    }
    public void OnClickUserRegister()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        ///post/1, GET, 완료되었을 때 호출되는 함수
        requester.url = "http://172.16.20.50:8080/v1/members";

        UserData data = new UserData();
        data.memberId = id.text;
        data.memberPwd = pw.text;
       // data.number = digitNumber.text;
        data.name = nickname.text;

        requester.body = JsonUtility.ToJson(data, true);
        requester.requestType = RequestType.POST;
        requester.onComplete = OnCompleteGetPost;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
    }
    public void OnCompleteGetPost(DownloadHandler handler)
    {
       JObject jObject = JObject.Parse(handler.text);
        
        //print(jObject + "jobj");
       int type = (int)jObject["status"];
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        // string token = (string)jObject["results"]["data"]["token"];
        print(type);
        // 통신 성공
        if (type == 201)
        {
            // 1. 회원 가입 성공했습니다. ui
            rgiOk.SetActive(true);
            print("통신 성공");
            // 2. PlayerPref에 key는 jwt, value는 token
            //PlayerPrefs.SetString("jwt", );
        }
        else
        {
            Fail.SetActive(true);
        }
    }

    void OnPostFailed()
    {
        print("OnPostFailed, 통신 실패");
    }
    
    public void backtoLogin()
    {
        SceneManager.LoadScene("Login_YDW");
    }
}