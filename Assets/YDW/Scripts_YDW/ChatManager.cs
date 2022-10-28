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

//Json�� ��� ���� "Ű" : "��"
[System.Serializable]
public struct ChatInfo
{
    public string nickName;
    public string chatText;
}

public class ChatManager : MonoBehaviourPun
{
    //ChatItme ����
    public GameObject chatItemFactory;
    //InputChat 
    public InputField inputChat;
    //ScrollView�� Content transform
    public RectTransform trContent;

    //���� �г��� ����
    Color nickColor;

    //��ü ���� ������ ����
    public List<ChatInfo> chatList = new List<ChatInfo>();

    // Joson ���� Test
    public Text testChatList;

    void Start()
    {
        //inputChat���� ���͸� ������ �� ȣ��Ǵ� �Լ� ���
        inputChat.onSubmit.AddListener(OnSubmit);
        //Ŀ���� �Ⱥ��̰�!
        Cursor.visible = false;

        nickColor = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f)
       );
    }

    void Update()
    {
        //escŰ�� ������ Ŀ���� Ȱ��ȭ
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        //��򰡸� Ŭ���ϸ� Ŀ���� Ȱ��ȭ
        if(Input.GetMouseButtonDown(0))
        {
            //���࿡ Ŀ���� UI�� ���ٸ�
            //����϶�
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

    //inputChat���� ���͸� ������ �� ȣ��Ǵ� �Լ�
    void OnSubmit(string s)
    {
        //<color=#FF0000>�г���</color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(nickColor) + ">" +
            PhotonNetwork.NickName + "</color>" + " : " + s;

        //1.���� ���ٰ� ���͸� ġ��
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);

        //4. inputChat�� ������ �ʱ�ȭ
        inputChat.text = "";

        //5. inputChat�� ���õǵ��� �Ѵ�.
        inputChat.ActivateInputField();
    }


    public RectTransform rtScrollView;
    float prevContentH;

    [PunRPC]
    void RpcAddChat(string nick, string chatText, float r, float g, float b)
    {
        print("���� �� : " + nick);
        print("���� ���� : " + chatText);

        //���� ������ ����
        ChatInfo info = new ChatInfo();
        info.nickName = nick;
        info.chatText = chatText;

        //<color=#FFFFFF>�г���</color>
        string s = "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(r, g, b)) + ">" + nick + "</color>" + " : " + chatText;

        //0. �ٲ�� ���� Content H���� ����
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem�� �����(�θ� Scorllview�� Content)
        GameObject item = Instantiate(chatItemFactory, trContent);

        //2.���� ChatItem���� ChatItem ������Ʈ �����´�
        ChatItem chat = item.GetComponent<ChatItem>();
              

        //3.������ ������Ʈ�� s�� ����
        chat.SetText(s);

        //Json ������ -> List�� ���
        chatList.Add(info);
        // 5�� �̻��� �ȴٸ� Json������
        if (chatList.Count >= 5)
        {
            ChatInfoList chatInfoList = new ChatInfoList();
            chatInfoList.data = chatList;

            //Json
            string jsonData = JsonUtility.ToJson(chatInfoList, true);
            print(jsonData);

            //Ȯ�ο�!
            testChatList.text = jsonData;

            //[�����ؾ���] API ����Ʈ ��� -> �ٵ� Http������� ������

            chatList.Clear();
        }

    }

    IEnumerator AutoScrollBottom()
    {
        yield return null;
        //��ũ�Ѻ� H���� Content H���� Ŭ ����(��ũ���� ������ ���¶��)
        if(trContent.sizeDelta.y > rtScrollView.sizeDelta.y)
        {            
            //(content y  >= ����Ǳ��� content H - ��ũ�Ѻ� H)
            if (trContent.anchoredPosition.y >= prevContentH - rtScrollView.sizeDelta.y)
            {
                //5. �߰��� ���̸�ŭ content y���� �����ϰڴ�.
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