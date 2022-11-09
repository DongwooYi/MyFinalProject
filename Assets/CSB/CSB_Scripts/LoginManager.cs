using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;


public class LoginManager : MonoBehaviour
{
    #region HTTP �α��� 
    public InputField id;
    public InputField pw;

    public void OnClickUserLogin()
    {
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = gameObject.AddComponent<HttpRequester>();

        ///post/1, GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "";

        LoginData ldata = new()
        {
            id = id.text,
            pw = pw.text
        };

        requester.body = JsonUtility.ToJson(ldata, true);
        requester.requestType = RequestType.POST;
        requester.onComplete = OnComplteLogin;

        //������ �޾Ƽ� �������
        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester);
    }

    public void OnComplteLogin(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        bool type = (bool)jObject["results"]["type"];
        string token = (string)jObject["results"]["data"]["token"];

        // ��� ����
        if (type)
        {
            // 1. PlayerPref�� key�� jwt, value�� token �ٷ� ������ �̵�
            PlayerPrefs.SetString("jwt", token);
            SceneManager.LoadScene("ChallengeWorld");
        }
    }

    public void Toregister()
    {
        SceneManager.LoadScene("Register_YDW");
    }
    #endregion
}
