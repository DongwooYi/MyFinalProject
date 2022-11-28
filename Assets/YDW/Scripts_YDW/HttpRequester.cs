using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

#region ä��
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
    // ���� ����
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
        name:"����"
    }
}*/

#region ��� ���� å
[System.Serializable]
public class AllBookReview
{
    public string bookName;
    public string bookReview;
    public string memebersName;
    public RawImage thumbnail;
}
#endregion
#region ȸ������
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
#region �α���(���̵�/ ���)
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
#region �̹���
[System.Serializable]
public class ImageData
{
    public byte[] imageDatas;
}
#endregion
#region ������
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
{//���̸�
    public string clubName;
    //å�̸�
    public string bookName;
    //���� �Ұ�
    public string clubIntro;
    // ���� �Ⱓ
   public string recruitStartDate;
   public string recruitEndDate;
    //ç���� �Ⱓ
    public string startDate;
    public string endDate;
    //�ִ��ο���
    public string numberOfMember;
    //���� 
    public string  dayOfWeeks;

    public byte[] imgFile;


    /*     "clubName":"���¸�Ŭ��30",
        "bookName":"�̽��ͼ�����",
        "clubIntro":" ȭ�����ϴ� ���������Դϴ�. ȭ����!",
        "recruitStartDate":"2022-11-01T12:00",
        "recruitEndDate":"2022-11-22T22:00",
        "startDate":"2022-11-23T00:00",
        "endDate":"2022-11-27T14:00",
        "numberOfMember":8,
        "scheduleDTOList":[
            {"dayOfWeek" : "", "startTime" : "14:05", "endTime" : "22:05"},
            { "dayOfWeek" : "��", "startTime" : "15:00", "endTime" : "16:00"},
            { "dayOfWeek" : "��", "startTime" : "17:00", "endTime" : "18:00"}*/

}
#endregion
#region ��������Ʈ
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
    //��û Ÿ��: Get, post)
    public RequestType requestType;
    public string body;

    //������ ���� �� ȣ������ �Լ� (Action)
    //Action �Լ��� ���� �� �֤��� �ڷ��� 
    public Action<DownloadHandler> onComplete;
    public Action onFailed;

    //����
    public void OnComplete(DownloadHandler result)
    {
        if (onComplete != null) onComplete(result);
    }

    public void OnFailed()
    {
        if (onFailed != null) onFailed();
    }
}



