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

[System.Serializable]
public class BookData
{
    // 도서 정보
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
    public string rating;
    public string bookReview;
    public string isDone;
    public string isOverHead;
}

[System.Serializable]
public class BestBookData
{
    public List<BestBook> recordList;
}

[System.Serializable]
public class BestBook
{
    public string bookISBN;
    public string isBest;
}

[System.Serializable]
public class BookRecord
{
    public BookInfo record;
    public byte[] bookImg;
}


[System.Serializable]
public class BookInfo
{
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
}

/*public class AAA
{
    public BBB mine;
}


public class BBB
{
    public int number;
    public string name;
}


{
    mine:
    {
        number: 2,
        name:"수빈"
    }
}*/

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
public class RoomDataImage
{
    public byte[] imgFile;
}
[System.Serializable]
public class Request
{
    //public RoomData roomDataforSedning;
   // public byte[] imgFile;
}
[System.Serializable]
public class RoomData
{//방이름
    public string clubName;
    //책이름
    public string bookName;
    //모임 소개
    public string clubIntro;
    // 모집 기간
   public string recruitStartDate;
   public string recruitEndDate;
    //챌린지 기간
    public string startDate;
    public string endDate;
    //최대인원수
    public string numberOfMember;
    //요일 
    public string  dayOfWeeks;

    public byte[] imgFile;


    /*     "clubName":"김태리클럽30",
        "bookName":"미스터선샤인",
        "clubIntro":" 화이팅하는 독서모임입니다. 화이팅!",
        "recruitStartDate":"2022-11-01T12:00",
        "recruitEndDate":"2022-11-22T22:00",
        "startDate":"2022-11-23T00:00",
        "endDate":"2022-11-27T14:00",
        "numberOfMember":8,
        "scheduleDTOList":[
            {"dayOfWeek" : "", "startTime" : "14:05", "endTime" : "22:05"},
            { "dayOfWeek" : "수", "startTime" : "15:00", "endTime" : "16:00"},
            { "dayOfWeek" : "금", "startTime" : "17:00", "endTime" : "18:00"}*/

}
#endregion
#region 독서메이트
[System.Serializable]

public class MemeberDataDetail
{
    public int status;
    public string message;
    public List<MemeberDataDetail2nd> data;
}

[System.Serializable]
public class MemeberDataDetail3rd
{
    public int recordCode;
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string thumnailLink;
    public string bookISBM;
}
[System.Serializable]

public class MemeberDataDetail2nd
{
    public int memberCode;
    public string memberId;
    public string name;
    public List<MemeberDataDetail3rd> records;
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



