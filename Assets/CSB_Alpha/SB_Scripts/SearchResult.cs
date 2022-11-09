using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 도서 검색 결과를 UI에 넣어줌
// 

public class SearchResult : MonoBehaviour
{
    public GameObject worldManager;
    List<_MyBookInfo> myBookInfoList = new List<_MyBookInfo>();

    public Text bookTitle;
    public Text author;
    public Text publishInfo;
    public RawImage thumbnail;

    private void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        myBookInfoList = worldManager.GetComponent<WorldManager2D>().myBookList;
    }

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

    public void SetImage(Texture texture)
    {
        thumbnail.texture = texture;
    }

    // 등록(추가)하기 버튼
    // 버튼을 클릭하면 클래스에 제목, 작가, 출판정보, 썸네일 넣어줌
    // 그 클래스를 MyBookList 에 추가
    public void OnClickAddBook()
    {
        _MyBookInfo myBookInfo = new _MyBookInfo();

        myBookInfo.title = bookTitle.text;
        myBookInfo.author = author.text;
        myBookInfo.publishInfo = publishInfo.text;
        myBookInfo.thumbnail = thumbnail;
        // MyBookList 에 추가
        myBookInfoList.Add(myBookInfo);
    }

}
