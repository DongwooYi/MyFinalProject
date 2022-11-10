using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

#region 채팅
[System.Serializable]
public class chatData
{
    public string chattingData;
    public static explicit operator chatData(JToken v)
    {
        throw new NotImplementedException();
    }
}
#endregion

#region 회원가입
[System.Serializable]
public class UserData
{
    public string memberId;
    public string memberPwd;
    public string cpw;
    public string name;
    public string number;
    public static explicit operator UserData(JToken v)
    {
        throw new NotImplementedException();
    }
}
#endregion
#region 로그인(아이디/ 비번)
[System.Serializable]
public class LoginData
{
    public string memberId;
    public string memberPwd;

    public static explicit operator LoginData(JToken v)
    {
        
        throw new NotImplementedException();
    }
}
#endregion
#region 이미지
[System.Serializable]
public class ImageData
{
    public byte[] imageDatas;
}
#endregion
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
    public string body;


    //응답이 왔을 때 호출해줄 함수 (Action)
    //Action 함수를 담을 수 있ㄴ느 자료형 
    public Action<DownloadHandler> onComplete;
    public Action onFailed;

    //실행
    public void OnComplete(DownloadHandler result)
    {
        if (onComplete != null) onComplete(result);
    }

    public void OnFailed()
    {
        if (onFailed != null) onFailed();
    }
}



