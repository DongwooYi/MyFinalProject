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
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        ///post/1, GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
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

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester);
    }

    public void OnCompleteGetPost(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        bool type = (bool)jObject["results"]["type"];
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        string token = (string)jObject["results"]["data"]["token"];

        // ��� ����
        if (type)
        {
            // 1. ȸ�� ���� �����߽��ϴ�. ui
            rgiOk.SetActive(true);
            print("��� ����");
            // 2. PlayerPref�� key�� jwt, value�� token
            PlayerPrefs.SetString("jwt", token);
        }
    }

    void OnPostFailed()
    {
        print("OnPostFailed, ��� ����");
    }
    public void ChangeScene()
    { 
        //3 . Ȯ���� ������ ������ �̵��Ѵ�. 
        SceneManager.LoadScene("");
    }
}