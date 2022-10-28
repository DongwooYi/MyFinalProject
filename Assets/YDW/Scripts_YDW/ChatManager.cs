using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

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
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                Cursor.visible = false;
            }

            //모바일땐
            //if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            //{
            //}

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
    void RpcAddChat(string chat)
    {
        //이전 content의 H값을 저장하자
        prevContentH = trContent.sizeDelta.y;

        //2.ChatItem을 하나 만든다. 
        //(부모를 ScrollView - Content)
        GameObject item = Instantiate(chatItemFactory, trContent);

        //3.text 컴포넌트 가져와서 inputField의
        //내용을 셋팅
        ChatItem chatItem = item.GetComponent<ChatItem>();
        chatItem.SetText(chat);

        //4. 이전에 바닥에 닿아있었다면
        StartCoroutine(AutoScrollBottom());
        
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
}
