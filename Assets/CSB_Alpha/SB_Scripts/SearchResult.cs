using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� �˻� ����� UI�� �־���
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

    // ���(�߰�)�ϱ� ��ư
    // ��ư�� Ŭ���ϸ� Ŭ������ ����, �۰�, ��������, ����� �־���
    // �� Ŭ������ MyBookList �� �߰�
    public void OnClickAddBook()
    {
        _MyBookInfo myBookInfo = new _MyBookInfo();

        myBookInfo.title = bookTitle.text;
        myBookInfo.author = author.text;
        myBookInfo.publishInfo = publishInfo.text;
        myBookInfo.thumbnail = thumbnail;
        // MyBookList �� �߰�
        myBookInfoList.Add(myBookInfo);
    }

}
