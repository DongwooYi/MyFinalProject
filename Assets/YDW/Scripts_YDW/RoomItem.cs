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
    public Text roomDesc;
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
    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        //desc ����
        roomDesc.text = (string)info.CustomProperties["descShortForm"];
        ChallengePeriod.text = ((string)info.CustomProperties["date"]);
        time = ((string)info.CustomProperties["DDay"]);
        isTimerOn = true;   
        //map id ����
        map_id = (byte[])info.CustomProperties["map_id"];
        print("mapID �迭"+ map_id.Length);
        Texture2D tex = new Texture2D(16, 16, TextureFormat.RGBA32, false);
        bool canLoad = tex.LoadImage(map_id);
        if(canLoad)
        {
            rawImage.color = Color.white;
        }
        rawImage.texture = (Texture)tex;
    }



    public void OnClick()
    {
        //���࿡ onClickAction �� null�� �ƴ϶��
        if (onClickAction != null)
        {
            //onClickAction ����
            onClickAction(name);
        }

        ////1. InputRoomName ���ӿ����� ã��
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. InputField ������Ʈ ��������
        //InputField inputField = go.GetComponent<InputField>();
        ////3. text�� roomName ����.
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
        TimeSpan remainingTime = expiringTime - DateTime.Now;
        textTimer.text = $"D-{remainingTime.Days}";    
    }
}
