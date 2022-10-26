using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    //Text
    XRText chatText;
    //RectTransform
    RectTransform rt;
    void Awake()
    {
        chatText = GetComponent<XRText>();
        chatText.onChangedSize = AAA;
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        ////preferredHeight 가 변경이 되면 
        //if(rt.sizeDelta.y != chatText.preferredHeight)
        //{
        //    //height 맞춰주자
        //    rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
        //}
        
    }

    public void SetText(string chat)
    {
        //텍스트 셋팅
        chatText.text = chat;        
    }

    void AAA()
    {
        print("크기가 변경되었다!!!");
        //height 맞춰주자
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
    }
}
