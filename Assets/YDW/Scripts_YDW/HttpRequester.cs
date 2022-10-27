using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


[System.Serializable]
public class LoginData
{
    public string id;
    public string pw;

    public static explicit operator LoginData(JToken v)
    {
        
        throw new NotImplementedException();
    }
}


public enum RequestType
{
    POST,
    GET
}

public class HttpRequester : MonoBehaviour
{
    //url
    public string url;
    //��û Ÿ��: Get, post)
    public RequestType requestType;
    public byte[] data;


    //������ ���� �� ȣ������ �Լ� (Action)
    //Action �Լ��� ���� �� �֤��� �ڷ��� 
    public Action<DownloadHandler> onComplete;
}