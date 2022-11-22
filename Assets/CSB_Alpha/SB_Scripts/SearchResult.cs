using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


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

    public GameObject reviewPanelFactory;
    public GameObject alarmFactory;

    public Transform myDesk;

    public WorldManager2D wm;

    private void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();
        
        myBookInfoList = wm.myBookList;
    }

    /* å ��� ���� */
    public void OnClickAddBook()
    {
        _MyBookInfo myBookInfo = new _MyBookInfo();

        myBookInfo.bookName = bookTitle.text;
        myBookInfo.bookAuthor = author.text;
        myBookInfo.bookPublishInfo = publishInfo.text;
        myBookInfo.bookISBN = isbn.text;
        myBookInfo.thumbnail = thumbnail;
        myBookInfo.isDone = false;

        wm.myAllBookList.Add(myBookInfo);   // ������ myAllBookList �� �߰�

        // <��� �Ǿ����ϴ�>
        Transform canvas = GameObject.Find("Canvas").transform;
        GameObject go = Instantiate(alarmFactory, canvas);
    }

    #region Text Setting
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
    #endregion

    #region ���� ��� ����
    /* ���� �а� �ִ� å ���(�߰�)�ϱ� ��ư */
    // ��ư�� Ŭ���ϸ� Ŭ������ ����, �۰�, ��������, ����� �־���
    // �� Ŭ������ MyBookList �� �߰�
    public void OnClickAddCurrBook()
    {
        // ���� ���� count 1 ����
        wm.bookCurrCount++;
        _MyBookInfo myBookInfo = new _MyBookInfo();

        myBookInfo.bookName = bookTitle.text;
        myBookInfo.bookAuthor = author.text;
        myBookInfo.bookPublishInfo = publishInfo.text;
        myBookInfo.bookISBN = isbn.text;
        myBookInfo.thumbnail = thumbnail;
        myBookInfo.isDone = false;

        // MyBookList �� �߰�
        myBookInfoList.Add(myBookInfo);

        // Http ��� �Լ� �߰� (POST)
        //HttpPostCurrBookInfo();

        // <��� �Ǿ����ϴ�>
        Transform canvas = GameObject.Find("Canvas").transform;
        GameObject go = Instantiate(alarmFactory, canvas);
        
    }



    /* �� ���� å ��� ��ư */
    // ��ư�� Ŭ���ϸ� Canvas ���� å���� �ۼ��ϴ� panel ã�Ƽ� SetActive true ��
    public void OnClickAddPastBook()
    {
        wm.bookPastCount++;

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
    }
    #endregion

    // Http ��� �Լ� (POST)
    void HttpPostCurrBookInfo()
    {
        print("�� ��� ������");
        //HttpRequester requester = gameObject.AddComponent<HttpRequester>();
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:8080/v1/records/add";
        requester.requestType = RequestType.POST;

        CurrBookdata currBookdata = new CurrBookdata();

        currBookdata.bookName = bookTitle.text;
        currBookdata.bookAuthor = author.text;
        currBookdata.bookISBN = isbn.text;
        currBookdata.bookPublishInfo = publishInfo.text;
        currBookdata.thumbnail = thumbnail;

        requester.body = JsonUtility.ToJson(currBookdata, true);

        requester.onComplete = OnCompletePostMyCurrBook;

        HttpManager.instance.SendRequest(requester, "application/json");

    }

    public void OnCompletePostMyCurrBook(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];
    }
}
