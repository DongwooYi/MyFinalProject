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
[System.Serializable]
public class ImageData
{
    public byte[] imageDatas;
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
    //요청 타입: Get, post)
    public RequestType requestType;
    public string data;


    //응답이 왔을 때 호출해줄 함수 (Action)
    //Action 함수를 담을 수 있ㄴ느 자료형 
    public Action<DownloadHandler> onComplete;
}