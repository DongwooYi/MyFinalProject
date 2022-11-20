using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
#region 현재 책/과거 책
[System.Serializable]
public class CurrBookdata
{
    // 도서 정보
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
    public RawImage thumbnail;
}

[System.Serializable]
public class PastBookdata
{
    // 도서 정보
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
    public RawImage thumbnail;

    public string rating;
    public string bookReview;

}
#endregion
#region 배너 관련 책
[System.Serializable]
public class AllBookReview
{
    public string bookName;
    public string bookReview;
    public string memebersName;
    public RawImage thumbnail;
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
#region 방정보
[System.Serializable]
public class RoomData
{
    public string RoomName;
    public string ThePeriodProject;
    public string MeetingDate;

}
#endregion
public enum RequestType
{
    LOGIN,
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



