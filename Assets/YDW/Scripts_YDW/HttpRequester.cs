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
#region ���� å/���� å
[System.Serializable]
public class CurrBookdata
{
    // ���� ����
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
    public RawImage thumbnail;
}

[System.Serializable]
public class PastBookdata
{
    // ���� ����
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
    public RawImage thumbnail;

    public string rating;
    public string bookReview;

}
#endregion
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



