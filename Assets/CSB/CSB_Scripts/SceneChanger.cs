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

        // inpunNickName ���� Enter Ű ������ ȣ��Ǵ� �Լ� ���
        inputID.onSubmit.AddListener(OnSubmit);

        // inputNickName ���� Focusing �� ��������� �� ȣ��Ǵ� �Լ� ���
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

        requester.data = JsonUtility.ToJson(ldata, true);
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
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        string token = (string)jObject["results"]["data"]["token"];

        // ��� ����
        if (type)
        {
            // 1. PlayerPref�� key�� jwt, value�� token �ٷ� ������ �̵�
            PlayerPrefs.SetString("jwt", token);
            SceneManager.LoadScene(1);
        }
    }

    #endregion
}
