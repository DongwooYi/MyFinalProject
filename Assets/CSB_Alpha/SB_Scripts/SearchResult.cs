using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� �˻� ����� UI�� �־���
// 

public class SearchResult : MonoBehaviour
{
    GameObject worldManager;
    List<_MyBookInfo> myBookInfoList = new List<_MyBookInfo>();

    public Text bookTitle;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;

    public GameObject bookFactory;
    public GameObject reviewPanelFactory;

    public Transform myDesk;

    private void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        myBookInfoList = worldManager.GetComponent<WorldManager2D>().myBookList;

        myDesk = GameObject.Find("MyDesk").transform;
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

    public void SetIsbn(string s)
    {
        isbn.text = s;
    }

    public void SetImage(Texture texture)
    {
        thumbnail.texture = texture;
    }

    // ���� �а� �ִ� å ���(�߰�)�ϱ� ��ư
    // ��ư�� Ŭ���ϸ� Ŭ������ ����, �۰�, ��������, ����� �־���
    // �� Ŭ������ MyBookList �� �߰�
    public void OnClickAddCurrBook()
    {
        _MyBookInfo myBookInfo = new _MyBookInfo();

        myBookInfo.title = bookTitle.text;
        myBookInfo.author = author.text;
        myBookInfo.publishInfo = publishInfo.text;
        myBookInfo.isbn = isbn.text;
        myBookInfo.thumbnail = thumbnail;
        myBookInfo.isDone = false;

        // MyBookList �� �߰�
        myBookInfoList.Add(myBookInfo);

/*        // å�� ���� �����ϰ� �� ������ material�� texture�� thumbnail ��
        GameObject go = Instantiate(bookFactory, myDesk);

        CurrBook currBook = go.GetComponent<CurrBook>();

        currBook.ChangeTexture(myBookInfo.thumbnail.texture);*/
    }

    // �� ���� å ��� ��ư 
    // ��ư�� Ŭ���ϸ� Canvas ���� å���� �ۼ��ϴ� panel ã�Ƽ� setactive true ��
    public void OnClickAddPastBook()
    {
        // �θ� �� ģ��
        Transform canvas = GameObject.Find("Canvas").transform;

        // ���� ���� �� �ִ� panel
        GameObject panel = Instantiate(reviewPanelFactory, canvas);

        MyReviewPanel myReviewPanel = panel.GetComponent<MyReviewPanel>();

        // panel�� ����, ���ǻ�, ISBN, �۰� �� å ���� ����
        myReviewPanel.SetTitle(bookTitle.text);
        myReviewPanel.SetAuthor(author.text);
        myReviewPanel.SetPublishInfo(publishInfo.text);
        myReviewPanel.SetIsbn(isbn.text);
        myReviewPanel.SetImage(thumbnail.texture);

        // ������ å ��Ͽ� �߰�
    }

}
