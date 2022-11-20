using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    //내용 
    public Text roomInfo;

    //설명
    public Text roomDesc;

    //맵 id
    int map_id;

//    public Text roomMeetingDate;

    //클릭이 되었을 때 호출되는 함수를 가지고있는 변수
    public System.Action<string, int> onClickAction;

    void Start()
    {

    }

    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        //게임오브젝트의 이름을 roomName으로!
        name = roomName;
        //방이름 (0/0)
        roomInfo.text = roomName + " (" + currPlayer + " / " + maxPlayer + ")"; 
    }

    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        //desc 설정
        roomDesc.text = (string)info.CustomProperties["desc"];

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
}
