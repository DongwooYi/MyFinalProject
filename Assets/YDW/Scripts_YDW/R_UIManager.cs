using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class R_UIManager : MonoBehaviour
{

    public InputField id;
    public InputField pw;
    public InputField cpw;
    public InputField nickname;
    public GameObject rgiOk;


    private void Update()
    {
        if(id.text.Length>0&&pw.text.Length>0&&cpw.text.Length>0&&nickname.text.Length>0)
        {
            rgiOk.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            rgiOk.gameObject.GetComponent<Button>().interactable = false;
        }
    }
    public void OnClickUserRegister()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        ///post/1, GET, 완료되었을 때 호출되는 함수
        requester.url = "";

        UserData data = new UserData();
        data.id = id.text;
        data.pw = pw.text;
        data.cpw = cpw.text;
        data.nickname = nickname.text;

        requester.body = JsonUtility.ToJson(data, true);
        requester.requestType = RequestType.POST;
        requester.onComplete = OnCompleteGetPost;
        requester.onFailed = OnPostFailed;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester);
    }

    public void OnCompleteGetPost(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        bool type = (bool)jObject["results"]["type"];
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        string token = (string)jObject["results"]["data"]["token"];

        // 통신 성공
        if (type)
        {
            // 1. 회원 가입 성공했습니다. ui
            rgiOk.SetActive(true);
            print("통신 성공");
            // 2. PlayerPref에 key는 jwt, value는 token
            PlayerPrefs.SetString("jwt", token);
        }
    }

    void OnPostFailed()
    {
        print("OnPostFailed, 통신 실패");
    }
    public void ChangeScene()
    { 
        //3 . 확인을 누르면 씬으로 이동한다. 
        SceneManager.LoadScene("");
    }
}