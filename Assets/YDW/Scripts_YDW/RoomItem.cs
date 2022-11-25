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
    public Text roomDesc;
    public Text ChallengePeriod;
    public Text textTimer;
    //맵 id
    int map_id;

   [Header("타이머")]
    public string time;
    public bool isTimerOn;

    //클릭이 되었을 때 호출되는 함수를 가지고있는 변수
    public System.Action<string, int> onClickAction;
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
    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        //desc 설정
        roomDesc.text = (string)info.CustomProperties["descShortForm"];
        ChallengePeriod.text = ((string)info.CustomProperties["date"]);
        textTimer.text = ((string)info.CustomProperties["DDay"]);
        isTimerOn = true;   
        //map id 설정
        map_id = (int)info.CustomProperties["map_id"];
    }



    public void OnClick()
    {
        //만약에 onClickAction 가 null이 아니라면
        if (onClickAction != null)
        {
            //onClickAction 실행
            onClickAction(name, map_id);
        }

        ////1. InputRoomName 게임오브젝 찾자
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. InputField 컴포넌트 가져오자
        //InputField inputField = go.GetComponent<InputField>();
        ////3. text에 roomName 넣자.
        //inputField.text = name;
    }
   /* IEnumerator GetImageData()
    {
        yield return ;
    }
*/
   void CountDownTimer()
    {
        DateTime expiringTime = DateTime.Parse(time);
        print(time + "카운트다운");
        TimeSpan remainingTime = expiringTime - DateTime.Now;
        //textTimer.text = $"챌린지 시작까지: {remainingTime.ToString(@"dd'일 'hh'시간 'mm'분 'ss'초'")} 남음";       
        textTimer.text = $"D-{remainingTime.ToString(@"DD")}";       
    }
}
