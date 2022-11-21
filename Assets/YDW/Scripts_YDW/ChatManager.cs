using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

//ä�� �α� ����
//Json - ��ü ���� ������ 
[System.Serializable]
public struct ChatInfoList
{
    public string opponentName;
    public List<ChatInfo> data;
}

//Json�� ��� ���� "Ű" : "��"
[System.Serializable]
public struct ChatInfo
{
    public string nickName;
    public string chatText;
}

//ê�� ��ȭ
//Json�� ��� ���� "Ű" : "��"
[System.Serializable]
public struct AiChatInfo
{
    public string chatRequest;
    public string memberId;
    public string weId;
}

public class ChatManager : MonoBehaviourPun
{

    [Header("�Ϲ� ä��")]
    //InputChat -> ����ڰ� ä���� ����
    public InputField inputChat;
    //ScorllView�� Content
    public RectTransform trContent;
    //ChatItem ����
    public GameObject chatItemFactory;

    [Header("Json")]
    //��ü ���� ������ ����
    public List<ChatInfo> chatList = new List<ChatInfo>();
    [Header("ä��â")]
    public GameObject ChattingPannel;

    public Toggle toggleChatting;
    //�� ���̵� ��
    Color idColor;
    Color otherColor;

    YDW_CharacterControllerPhoton ydw_CharacterControllerPhoton;
    void Start()
    {
        ydw_CharacterControllerPhoton = GameObject.FindObjectOfType<YDW_CharacterControllerPhoton>();
        //InputField���� ���͸� ���� �� ȣ��Ǵ� �Լ� ���
        inputChat.onSubmit.AddListener(OnSubmit);
   
        //idColor�� �����ϰ�
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

    //InputField���� ���͸� ������ ȣ��Ǵ� �Լ�
    void OnSubmit(string s)
    {
        enterchat = true;
        //�����
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
        //4. InputChat�� ������ �ʱ�ȭ
        inputChat.text = "";
        //5. InputChat�� Focusing �� ������.
        //Enter �� ������ InputField �� ��� Ȱ��ȭ �ǰ� ���ִ� �ڵ�
        inputChat.ActivateInputField();
    }

    
    //���� Content�� H
    float prevContentH;
    //ScorllView�� RectTransform
    public RectTransform AIScrollView;

    string jsonData;

    float currentTime = 0;
    [PunRPC]
    void RpcAddChat(string nick, string chatText, float r, float g, float b)
    {
        currentTime += Time.deltaTime;
        enterchat = false;
        print("���� �� : " + nick);
        print("���� ���� : " + chatText);
        //���� ������ ����
        ChatInfo info = new ChatInfo();
        info.nickName = nick;
        info.chatText = chatText;

        print(info.chatText);

        //<color=#FFFFFF>�г���</color>
        string s = "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(r, g, b)) + ">" + nick + "</color>" + " : " + chatText;
        string j = chatText;
        
        //0. �ٲ�� ���� Content H���� ����
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem�� �����(�θ� Scorllview�� Content)
        GameObject item = Instantiate(chatItemFactory, trContent);

        //2.���� ChatItem���� ChatItem ������Ʈ �����´�
        ChatItem chat = item.GetComponent<ChatItem>();


        
        //3.������ ������Ʈ�� s�� ����
        chat.SetText(s);
        
        ydw_CharacterControllerPhoton.speecgBubbleGameObj.SetActive(true);
        ydw_CharacterControllerPhoton.speechBubble.text = chatText;
       
        //Json ������ -> List�� ���
        chatList.Add(info);

        // 5�� �̻��� �ȴٸ� Json������
        if (chatList.Count >= 6)
        {

            ChatInfoList chatInfoList = new ChatInfoList();
            //���� �г��� �־����
            chatInfoList.opponentName = findPlayer();
            chatInfoList.data = chatList;

            //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
            jsonData = JsonUtility.ToJson(chatInfoList, true);
            print(jsonData);


            //[�����ؾ���] API ����Ʈ ��� -> �ٵ� Http������� ������

            chatList.Clear();

            OnPost(jsonData);
        }
        //������ �����ؾ��� ->Rpc�̱� ������
        //inputChat.text = "";

        //��ũ�� �� ��� ������ �ڵ�� ����
        StartCoroutine(AIAutoScrollBottom());
        
    }

    IEnumerator AIAutoScrollBottom()
    {
        yield return null;

        //trScrollView H ���� Content H ���� Ŀ����(��ũ�� ���ɻ���)
        if (trContent.sizeDelta.y > AIScrollView.sizeDelta.y)
        {
            //4. Content�� �ٴڿ� ����־��ٸ�
            if (trContent.anchoredPosition.y >= prevContentH - AIScrollView.sizeDelta.y)
            {
                //5. Content�� y���� �ٽ� ����������
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - AIScrollView.sizeDelta.y);
            }
        }
    }


    //��Ʈ��ũ ����
    public void OnPost(string s)
    {

        HttpRequester requester = new HttpRequester();

        ///post/1, GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "";

        chatData data = new chatData();
        data.chattingData = s;

        requester.body = JsonUtility.ToJson(data, true);
        requester.requestType = RequestType.POST;
        //requester.onComplete = OnCompleteGetPost;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");

        /*  string url = "/chat/";
          url += HttpManager.instance.username;

          //���� -> ������ ��ȸ -> ���� �־��� 
          HttpRequester requester = new HttpRequester();

          requester.SetUrl(RequestType.POST, url, true);
          requester.body = s;
          requester.isChat = true;

          requester.onComplete = OnPostComplete;
          requester.onFailed = OnGetFailed;

          HttpManager.instance.SendRequest(requester);*/
    }

    //�濡 �÷��̾ ���� ���� �� ȣ�����ִ� �Լ� -> GameManager���� ����
    public void AddPlayer(string add)
    {
        //0. �ٲ�� ���� Content H���� ����
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem�� �����(�θ� Scorllview�� Content)
        GameObject item = Instantiate(chatItemFactory, trContent);

        //2.���� ChatItem���� ChatItem ������Ʈ �����´�
        ChatItem chat = item.GetComponent<ChatItem>();

        //3.������ ������Ʈ�� s�� ����
        chat.SetText(add);
                     
    }
}
