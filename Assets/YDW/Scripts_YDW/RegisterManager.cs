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
    [Header("ȸ������")]
    public InputField id;
    public InputField pw;
    public InputField cpw;
    public InputField nickname;
    public InputField digitNumber;
    public GameObject rgiOk;
    public Button buttongforregister;
    public GameObject Fail;
    public GameObject registerImage;
    
    //Regex regex = new Regex(@"^010[0-9]{4}[0-9]{4}$");

    private void Start()
    {
       buttongforregister.interactable = false;
       // digitNumber.characterLimit = 11;
       print(registerImage.transform.GetChild(0).gameObject.name);
    }

    private void Update()
    {
        if (cpw.text.Length > 0)
        {
            if (pw.text != cpw.text)
            {
                registerImage.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.red;
               
            }
            else
            {
                registerImage.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.black;

            }
        }

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
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        ///post/1, GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://172.16.20.50:8080/v1/members";

        UserData data = new UserData();
        data.memberId = id.text;
        data.memberPwd = pw.text;
       // data.number = digitNumber.text;
        data.name = nickname.text;

        requester.body = JsonUtility.ToJson(data, true);
        requester.requestType = RequestType.POST;
        requester.onComplete = OnCompleteGetPost;

        //HttpManager���� ��û
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
        // ��� ����
        if (type == 201)
        {
            // 1. ȸ�� ���� �����߽��ϴ�. ui
            rgiOk.SetActive(true);
            print("��� ����");
        }
        else
        {
            Fail.SetActive(true);
        }
    }

    void OnPostFailed()
    {
        print("OnPostFailed, ��� ����");
    }
    
    
    IEnumerator BinkText()
    {
        while (true)
        {
            id.text = "";
            yield return new WaitForSeconds(.5f);
            id.text = "Spacebar to Start";
            yield return new WaitForSeconds(.5f);
        }
    }
}