using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 도서 검색 결과를 UI에 넣어줌
// 

public class SearchResult : MonoBehaviour
{
    public Text bookTitle;
    public Text author;
    public Text publishInfo;
    public RawImage thumbnail;

    public void SetBookTitle(string s)
    {
        bookTitle.text = s;
    }

    public void SetBookAuthor(string s)
    {
        author.text = s;
    }

    public void SetBookPublishInfo(string s)
    {
        publishInfo.text = s;
    }

    public void SetImage(string url)
    {
        //thumbnail.texture = url;
    }
}
