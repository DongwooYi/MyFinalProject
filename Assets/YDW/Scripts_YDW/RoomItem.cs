using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    //내용 
    public Text roomInfo;
    public Text roomPlayerInfo;

    //설명
    public Text roomShortDesc;
    public Text bookNameDesc;
    public Text ChallengePeriod;
    public Text textTimer;
    public RawImage rawImage;
    //맵 id
    byte[] map_id;

   [Header("타이머")]
    public string time;
    public bool isTimerOn;

    //클릭이 되었을 때 호출되는 함수를 가지고있는 변수
    public System.Action<string> onClickAction;
    private void Update()
    {
        if (isTimerOn)
        { CountDownTimer(); }
    }
    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        //게임오브젝트의 이름을 roomName으로!
        name = roomName;
        //방이름 (0/0)
        roomInfo.text = roomName; // + " (" + currPlayer + " / " + maxPlayer + ")"; 
        roomPlayerInfo.text = currPlayer + " / " + maxPlayer + "명";
    }
    string MeetingDayofweeks, meetingTime, hostName, roomDesc;
    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        //desc 설정
        roomShortDesc.text = (string)info.CustomProperties["descShortForm"];
        bookNameDesc.text = "대상 도서  "+(string)info.CustomProperties["book_Name"];
        ChallengePeriod.text = ((string)info.CustomProperties["date"]);
        time = ((string)info.CustomProperties["DDay"]);
        MeetingDayofweeks =(string)info.CustomProperties["dayOfWeeks"];
        meetingTime = (string)info.CustomProperties["meetingTime"];
        hostName = (string)info.CustomProperties["roomHost_Name"];
        roomDesc = (string)info.CustomProperties["desc"];
        isTimerOn = true;   

        //map id 설정
        map_id = (byte[])info.CustomProperties["map_id"];
        print("mapID 배열"+ map_id.Length);
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
        //만약에 onClickAction 가 null이 아니라면
        if (onClickAction != null)
        {
            //onClickAction 실행
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
