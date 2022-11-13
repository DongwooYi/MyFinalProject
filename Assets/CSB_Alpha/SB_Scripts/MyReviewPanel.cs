using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


public class MyReviewPanel : MonoBehaviour
{
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();
    public List<_MyPastBookInfo> myPastBookListNet = new List<_MyPastBookInfo>();

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;

    public Dropdown dropdown;

    public InputField inputFieldReview; // 리뷰 입력 칸
    public Button btnEnter; // 등록하기 버튼

    // 등록됨 안내 메시지 띄우기
    public GameObject alarmFactory;

    public GameObject[] bookRewardFactory = new GameObject[3];    // 책장에 넣을 책 관련

    GameObject book;

    void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;
        myPastBookListNet = worldManager.GetComponent<WorldManager2D>().myPastBookListNet;

        book = GameObject.Find("Book");

        inputFieldReview.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string s)
    {
        btnEnter.interactable = s.Length > 0;  // 등록 버튼 활성화
    }

    // 등록 버튼 (누르면 <다읽은 책목록>에 추가)
    public void OnClickAddPastBook()
    {
        _MyPastBookInfo myPastBookInfo = new _MyPastBookInfo();

        myPastBookInfo.bookName = title.text;
        myPastBookInfo.bookAuthor = author.text;
        myPastBookInfo.bookPublishInfo = publishInfo.text;
        myPastBookInfo.bookISBN = isbn.text;
        myPastBookInfo.thumbnail = thumbnail;
        myPastBookInfo.isDone = true;
        myPastBookInfo.rating = dropdown.captionText.text;
        myPastBookInfo.review = inputFieldReview.text;

        // <다읽은책목록> 에 추가
        myPastBookListNet.Add(myPastBookInfo);

        HttpPostPastBookInfo();

        // <다읽은책목록>의 마지막 인덱스 
        int idx = myPastBookListNet.Count - 1;

        GameObject setBook = book.transform.GetChild(idx).gameObject;
        setBook.SetActive(true);
        setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);

/*        // 책장에 넣기
        int idx = Random.Range(0, 3);
        GameObject book = Instantiate(bookRewardFactory[idx]);  // 세 가지 모양 중 하나 생성

        book.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);*/

        // <등록 되었습니다>
        GameObject go = Instantiate(alarmFactory, gameObject.transform);    // 나의 자식으로 생성

    }

    // 나가기 버튼 (누르면 저장되지 않음)
    public void OnClickExit()
    {
        // 작성한 리뷰와 평점 초기화... 할 필요가 없네
        // 나 자신 초기화
        Destroy(gameObject);
    }

    #region 텍스트들 세팅 관련

    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetAuthor(string s)
    {
        author.text = s;
    }
    
    public void SetPublishInfo(string s)
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
    
    public void HttpPostPastBookInfo()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:8080/v1/records/write";
        requester.requestType = RequestType.POST;

        PastBookdata pastBookdata = new PastBookdata();

        pastBookdata.bookName = title.text;
        pastBookdata.bookAuthor = author.text;
        pastBookdata.bookPublishInfo = publishInfo.text;
        pastBookdata.bookISBN = isbn.text;
        pastBookdata.thumbnail = thumbnail;
        pastBookdata.rating = dropdown.captionText.text;
        pastBookdata.bookReview = inputFieldReview.text;

        requester.body = JsonUtility.ToJson(pastBookdata, true);
        requester.onComplete = OnCompletePostMyPastBook;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    public void OnCompletePostMyPastBook(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        //print(jObject + "jobj");
        int type = (int)jObject["status"];
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        // string token = (string)jObject["results"]["data"]["token"];
    }
}
