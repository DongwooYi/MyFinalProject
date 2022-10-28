/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

[System.Serializable]
public struct ChatInfoList
{
    public List<ChatInfo> data;
}

//Json에 담길 내용 "키" : "값"
[System.Serializable]
public struct ChatInfo
{
    public string nickName;
    public string chatText;
}

public class ChatManager : MonoBehaviourPun
{
    //ChatItme 공장
    public GameObject chatItemFactory;
    //InputChat 
    public InputField inputChat;
    //ScrollView의 Content transform
    public RectTransform trContent;

    //나의 닉네임 색깔
    Color nickColor;

    //전체 보낼 데이터 생성
    public List<ChatInfo> chatList = new List<ChatInfo>();

    // Joson 정보 Test
    public Text testChatList;

    void Start()
    {
        //inputChat에서 엔터를 눌렀을 때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);
        //커서를 안보이게!
        Cursor.visible = false;

        nickColor = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f)
       );
    }

    void Update()
    {
        //esc키를 누르면 커서를 활성화
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        //어딘가를 클릭하면 커서를 활성화
        if(Input.GetMouseButtonDown(0))
        {
            //만약에 커서가 UI에 없다면
            //모바일땐
            //if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;

            }




        }
    }

    //inputChat에서 엔터를 눌렀을 때 호출되는 함수
    void OnSubmit(string s)
    {
        //<color=#FF0000>닉네임</color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(nickColor) + ">" +
            PhotonNetwork.NickName + "</color>" + " : " + s;

        //1.글을 쓰다가 엔터를 치면
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);

        //4. inputChat의 내용을 초기화
        inputChat.text = "";

        //5. inputChat이 선택되도록 한다.
        inputChat.ActivateInputField();
    }


    public RectTransform rtScrollView;
    float prevContentH;

    [PunRPC]
    void RpcAddChat(string nick, string chatText, float r, float g, float b)
    {
        print("보낸 놈 : " + nick);
        print("보낸 내용 : " + chatText);

        //보낼 데이터 생성
        ChatInfo info = new ChatInfo();
        info.nickName = nick;
        info.chatText = chatText;

        //<color=#FFFFFF>닉네임</color>
        string s = "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(r, g, b)) + ">" + nick + "</color>" + " : " + chatText;

        //0. 바뀌기 전의 Content H값을 넣자
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem을 만든다(부모를 Scorllview의 Content)
        GameObject item = Instantiate(chatItemFactory, trContent);

        //2.만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        ChatItem chat = item.GetComponent<ChatItem>();
              

        //3.가져온 컴포넌트에 s를 셋팅
        chat.SetText(s);

        //Json 보내기 -> List에 담기
        chatList.Add(info);
        // 5개 이상이 된다면 Json보내기
        if (chatList.Count >= 5)
        {
            ChatInfoList chatInfoList = new ChatInfoList();
            chatInfoList.data = chatList;

            //Json
            string jsonData = JsonUtility.ToJson(chatInfoList, true);
            print(jsonData);

            //확인용!
            testChatList.text = jsonData;

            //[설정해야함] API 포스트 방식 -> 바디 Http통신으로 보내기

            chatList.Clear();
        }

    }

    IEnumerator AutoScrollBottom()
    {
        yield return null;
        //스크롤뷰 H보다 Content H값이 클 때만(스크롤이 가능한 상태라면)
        if(trContent.sizeDelta.y > rtScrollView.sizeDelta.y)
        {            
            //(content y  >= 변경되기전 content H - 스크롤뷰 H)
            if (trContent.anchoredPosition.y >= prevContentH - rtScrollView.sizeDelta.y)
            {
                //5. 추가된 높이만큼 content y값을 변경하겠다.
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - rtScrollView.sizeDelta.y);
            }
        }
    }
   *//* public void OnGetPost(string s)
    {
        string url = "https://8c49-119-194-163-123.jp.ngrok.io/chat_bot?chat_request=";
        url += "&user_id=" + 1;
        url += "&we_id=" + 1;

        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, false);

        HttpManager.instance.SendRequest(requester);
    }*//*
}
*/