using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 책 제목 입력 받으면 Request url에 title 넣어서 requests
[Serializable]
public class BookInfo   // 받아오는 책정보
{
    //public 
/*    public string title;    // 제목
    //public string contents; // 설명
    public string image;  // 썸네일 이미지 url
    public string isbn;
    public string author;
    public string publisher;
    public string[] translators;*/
}

public class APIRequester : MonoBehaviour
{

    public Action<DownloadHandler> onComplete;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
