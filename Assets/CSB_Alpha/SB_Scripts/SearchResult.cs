using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


// 도서 검색 결과를 UI에 넣어줌
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

    /* 현재 읽고 있는 책 등록(추가)하기 버튼 */
    // 버튼을 클릭하면 클래스에 제목, 작가, 출판정보, 썸네일 넣어줌
    // 그 클래스를 MyBookList 에 추가
    public void OnClickAddCurrBook()
    {
        _MyBookInfo myBookInfo = new _MyBookInfo();

        myBookInfo.bookName = bookTitle.text;
        myBookInfo.bookAuthor = author.text;
        myBookInfo.bookPublishInfo = publishInfo.text;
        myBookInfo.bookISBN = isbn.text;
        myBookInfo.thumbnail = thumbnail;
        myBookInfo.isDone = false;

        // MyBookList 에 추가
        myBookInfoList.Add(myBookInfo);

        // Http 통신 함수 추가 (POST)
        HttpPost();
    }

    /* 다 읽은 책 등록 버튼 */
    // 버튼을 클릭하면 Canvas 에서 책리뷰 작성하는 panel 찾아서 setactive true 로
    public void OnClickAddPastBook()
    {
        // 부모가 될 친구
        Transform canvas = GameObject.Find("Canvas").transform;

        // 리뷰 적을 수 있는 panel
        GameObject panel = Instantiate(reviewPanelFactory, canvas);

        MyReviewPanel myReviewPanel = panel.GetComponent<MyReviewPanel>();

        // panel에 제목, 출판사, ISBN, 작가 등 책 정보 셋팅
        myReviewPanel.SetTitle(bookTitle.text);
        myReviewPanel.SetAuthor(author.text);
        myReviewPanel.SetPublishInfo(publishInfo.text);
        myReviewPanel.SetIsbn(isbn.text);
        myReviewPanel.SetImage(thumbnail.texture);
    }

    // Http 통신 함수 (POST)
    void HttpPost()
    {
        print("넌 몇번 들어오니");
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
        //currBookdata.isDone = false;

        requester.body = JsonUtility.ToJson(currBookdata, true);

        requester.onComplete = OnCompletePost;

        HttpManager.instance.SendRequest(requester, "application/json");

    }

    public void OnCompletePost(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        
    }
}
