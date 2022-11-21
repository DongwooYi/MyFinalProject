using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

//채팅 로그 저장
//Json - 전체 보낼 데이터 
[System.Serializable]
public struct ChatInfoList
{
    public string opponentName;
    public List<ChatInfo> data;
}

//Json에 담길 내용 "키" : "값"
[System.Serializable]
public struct ChatInfo
{
    public string nickName;
    public string chatText;
}

//챗봇 대화
//Json에 담길 내용 "키" : "값"
[System.Serializable]
public struct AiChatInfo
{
    public string chatRequest;
    public string memberId;
    public string weId;
}

public class ChatManager : MonoBehaviourPun
{

    [Header("일반 채팅")]
    //InputChat -> 사용자가 채팅한 내용
    public InputField inputChat;
    //ScorllView의 Content
    public RectTransform trContent;
    //ChatItem 공장
    public GameObject chatItemFactory;

    [Header("Json")]
    //전체 보낼 데이터 생성
    public List<ChatInfo> chatList = new List<ChatInfo>();
    [Header("채팅창")]
    public GameObject ChattingPannel;

    public Toggle toggleChatting;
    //내 아이디 색
    Color idColor;
    Color otherColor;

    YDW_CharacterControllerPhoton ydw_CharacterControllerPhoton;
    void Start()
    {
        ydw_CharacterControllerPhoton = GameObject.FindObjectOfType<YDW_CharacterControllerPhoton>();
        //InputField에서 엔터를 쳤을 때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);
   
        //idColor를 랜덤하게
        //idColor = new Color32((byte)Random.Range(0, 256),(byte)Random.Range(0, 256),(byte)Random.Range(0, 256),255);
        idColor = Color.yellow;
        otherColor = Color.green;
    }

    void Update()
    {
        if (toggleChatting.isOn)
        {
            ChattingPannel.SetActive(true);
        }
        else
        {
            ChattingPannel.SetActive(false);
        }
    }

    string findPlayer()
    {
        string s = "";

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].NickName != PhotonNetwork.NickName)
            {
                s = PhotonNetwork.PlayerList[i].NickName;
            }
        }
        return s;
    }

    public bool enterchat;

    //InputField에서 엔터를 쳤을때 호출되는 함수
    void OnSubmit(string s)
    {
        enterchat = true;
        //나라면
        if(photonView.IsMine)
        {
         photonView.RPC("RpcAddChat", RpcTarget.All,
         PhotonNetwork.NickName,
         s,
         idColor.r,
         idColor.g,
         idColor.b);
       
        }
        else
        {
            photonView.RPC("RpcAddChat", RpcTarget.All,
         PhotonNetwork.NickName,
         s,
         otherColor.r,
         otherColor.g,
         otherColor.b);
        }
        //4. InputChat의 내용을 초기화
        inputChat.text = "";
        //5. InputChat에 Focusing 을 해주자.
        //Enter 를 눌러도 InputField 가 계속 활성화 되게 해주는 코드
        inputChat.ActivateInputField();
    }

    
    //이전 Content의 H
    float prevContentH;
    //ScorllView의 RectTransform
    public RectTransform AIScrollView;

    string jsonData;

    float currentTime = 0;
    [PunRPC]
    void RpcAddChat(string nick, string chatText, float r, float g, float b)
    {
        currentTime += Time.deltaTime;
        enterchat = false;
        print("보낸 놈 : " + nick);
        print("보낸 내용 : " + chatText);
        //보낼 데이터 생성
        ChatInfo info = new ChatInfo();
        info.nickName = nick;
        info.chatText = chatText;

        print(info.chatText);

        //<color=#FFFFFF>닉네임</color>
        string s = "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(r, g, b)) + ">" + nick + "</color>" + " : " + chatText;
        string j = chatText;
        
        //0. 바뀌기 전의 Content H값을 넣자
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem을 만든다(부모를 Scorllview의 Content)
        GameObject item = Instantiate(chatItemFactory, trContent);

        //2.만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        ChatItem chat = item.GetComponent<ChatItem>();


        
        //3.가져온 컴포넌트에 s를 셋팅
        chat.SetText(s);
        
        ydw_CharacterControllerPhoton.speecgBubbleGameObj.SetActive(true);
        ydw_CharacterControllerPhoton.speechBubble.text = chatText;
       
        //Json 보내기 -> List에 담기
        chatList.Add(info);

        // 5개 이상이 된다면 Json보내기
        if (chatList.Count >= 6)
        {

            ChatInfoList chatInfoList = new ChatInfoList();
            //상대방 닉네임 넣어야함
            chatInfoList.opponentName = findPlayer();
            chatInfoList.data = chatList;

            //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
            jsonData = JsonUtility.ToJson(chatInfoList, true);
            print(jsonData);


            //[설정해야함] API 포스트 방식 -> 바디 Http통신으로 보내기

            chatList.Clear();

            OnPost(jsonData);
        }
        //위에서 설정해야함 ->Rpc이기 때문에
        //inputChat.text = "";

        //스크롤 바 계속 내리기 코드로 구현
        StartCoroutine(AIAutoScrollBottom());
        
    }

    IEnumerator AIAutoScrollBottom()
    {
        yield return null;

        //trScrollView H 보다 Content H 값이 커지면(스크롤 가능상태)
        if (trContent.sizeDelta.y > AIScrollView.sizeDelta.y)
        {
            //4. Content가 바닥에 닿아있었다면
            if (trContent.anchoredPosition.y >= prevContentH - AIScrollView.sizeDelta.y)
            {
                //5. Content의 y값을 다시 설정해주자
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - AIScrollView.sizeDelta.y);
            }
        }
    }


    //네트워크 수정
    public void OnPost(string s)
    {

        HttpRequester requester = new HttpRequester();

        ///post/1, GET, 완료되었을 때 호출되는 함수
        requester.url = "";

        chatData data = new chatData();
        data.chattingData = s;

        requester.body = JsonUtility.ToJson(data, true);
        requester.requestType = RequestType.POST;
        //requester.onComplete = OnCompleteGetPost;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");

        /*  string url = "/chat/";
          url += HttpManager.instance.username;

          //생성 -> 데이터 조회 -> 값을 넣어줌 
          HttpRequester requester = new HttpRequester();

          requester.SetUrl(RequestType.POST, url, true);
          requester.body = s;
          requester.isChat = true;

          requester.onComplete = OnPostComplete;
          requester.onFailed = OnGetFailed;

          HttpManager.instance.SendRequest(requester);*/
    }

    //방에 플레이어가 참여 했을 때 호출해주는 함수 -> GameManager에서 실행
    public void AddPlayer(string add)
    {
        //0. 바뀌기 전의 Content H값을 넣자
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem을 만든다(부모를 Scorllview의 Content)
        GameObject item = Instantiate(chatItemFactory, trContent);

        //2.만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        ChatItem chat = item.GetComponent<ChatItem>();

        //3.가져온 컴포넌트에 s를 셋팅
        chat.SetText(add);
                     
    }
}
