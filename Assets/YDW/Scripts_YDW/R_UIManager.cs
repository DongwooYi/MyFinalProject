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

    /*

    public void OnClickUserRegister()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        ///post/1, GET, 완료되었을 때 호출되는 함수
        requester.url = "http://192.168.1.19:8888/users/signup";

        UserData data = new UserData();
        data.id = id.text;
        data.pw = pw.text;
        data.cpw = cpw.text;
        data.nickname = nicknam.text;
        

        requester.data = JsonUtility.ToJson(data, true);
        requester.requestType = RequestType.POST;
        requester.onComplete = OnCompleteGetPost;

        //응답을 받아서 출력하자
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
            print("sss");
            // 2. PlayerPref에 key는 jwt, value는 token
            PlayerPrefs.SetString("jwt", token);
            //3 . 확인을 누르면 씬으로 이동한다. 

        }
    }*/

    public void onOkBtn()
    {
        SceneManager.LoadScene(2);
    }

}