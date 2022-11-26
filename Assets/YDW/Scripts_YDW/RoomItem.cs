using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    //���� 
    public Text roomInfo;
    public Text roomPlayerInfo;

    //����
    public Text roomShortDesc;
    public Text bookNameDesc;
    public Text ChallengePeriod;
    public Text textTimer;
    public RawImage rawImage;
    //�� id
    byte[] map_id;

   [Header("Ÿ�̸�")]
    public string time;
    public bool isTimerOn;

    //Ŭ���� �Ǿ��� �� ȣ��Ǵ� �Լ��� �������ִ� ����
    public System.Action<string> onClickAction;
    private void Update()
    {
        if (isTimerOn)
        { CountDownTimer(); }
    }
    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        //���ӿ�����Ʈ�� �̸��� roomName����!
        name = roomName;
        //���̸� (0/0)
        roomInfo.text = roomName; // + " (" + currPlayer + " / " + maxPlayer + ")"; 
        roomPlayerInfo.text = currPlayer + " / " + maxPlayer + "��";
    }
    string MeetingDayofweeks, meetingTime, hostName, roomDesc;
    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        //desc ����
        roomShortDesc.text = (string)info.CustomProperties["descShortForm"];
        bookNameDesc.text = "��� ����  "+(string)info.CustomProperties["book_Name"];
        ChallengePeriod.text = ((string)info.CustomProperties["date"]);
        time = ((string)info.CustomProperties["DDay"]);
        MeetingDayofweeks =(string)info.CustomProperties["dayOfWeeks"];
        meetingTime = (string)info.CustomProperties["meetingTime"];
        hostName = (string)info.CustomProperties["roomHost_Name"];
        roomDesc = (string)info.CustomProperties["desc"];
        isTimerOn = true;   

        //map id ����
        map_id = (byte[])info.CustomProperties["map_id"];
        print("mapID �迭"+ map_id.Length);
        Texture2D tex = new Texture2D((int)rawImage.rectTransform.rect.width, (int)rawImage.rectTransform.rect.height, TextureFormat.RGBA32, false);
        bool canLoad = tex.LoadImage(map_id);
        if(canLoad)
        {
            rawImage.color = Color.white;
        }
        rawImage.texture = (Texture)tex;
    }

    public GameObject gameObjectForRoomDetailDesc;

    public void OnClick()
    {
        //���࿡ onClickAction �� null�� �ƴ϶��
        if (onClickAction != null)
        {
            //onClickAction ����
            onClickAction(name);
        GameObject go = Instantiate(gameObjectForRoomDetailDesc);
     
        }
    }
   /* IEnumerator GetImageData()
    {
        yield return ;
    }
*/
   void CountDownTimer()
    {
        DateTime expiringTime = DateTime.Parse(time);
        TimeSpan remainingTime = expiringTime - DateTime.Now;
        textTimer.text = $"D-{remainingTime.Days+1}";    
    }
}
