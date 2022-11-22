using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class CurrBookInfoPanel : MonoBehaviour
{
    public GameObject bookFactory;  // 담은도서 목록 공장

    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();
    //List<_MyPastBookInfo> myPastBookListNet = new List<_MyPastBookInfo>();

    List<_MyBookInfo> myBookInfoList = new List<_MyBookInfo>();
    //List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    //public Text rating;
    public InputField review;
    public RawImage thumbnail;

    bool isDone;
    int idx;


    public Dropdown dropdown;

    public InputField inputFieldReview; // 리뷰 입력 칸
    public Button btnEnter; // 등록하기 버튼

    public GameObject player;   // 플레이어
    public GameObject showBook;

    GameObject book;

    // 등록됨 안내 메시지 띄우기
    public GameObject alarmFactory;

    public Toggle headBook;
    public Toggle checkIsDone;

    MyBookManager bookManager;
    WorldManager2D wm;

    GameObject myCurrBookPanel;

    public void ToggleHead(Toggle headBook)
    {
        print("토글");
        if (headBook.isOn)
        {
            print("토글" + headBook.isOn);
            showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            //player.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
        }
    }

    void Start()
    {
        player = GameObject.Find("Character");

        showBook = GameObject.Find("ShowBook");

        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();
        myPastBookInfoList = wm.myPastBookList;
        //myPastBookListNet = worldManager.GetComponent<WorldManager2D>().myPastBookList;

        myBookInfoList = worldManager.GetComponent<WorldManager2D>().myBookList;
        //myBookListNet = worldManager.GetComponent<WorldManager2D>().myBookList;

        book = GameObject.Find("Book");

        myCurrBookPanel = GameObject.Find("MyCurrBookPanel");
        bookManager = GameObject.Find("MyBookManager").GetComponent<MyBookManager>();

        inputFieldReview.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string s)
    {
        btnEnter.interactable = s.Length > 0;  // 등록 버튼 활성화
    }

    // isDone 확인 토글 관련


    // 등록 버튼
    // isDone == true 면 WorldManager 의 myDoneBookList 에
    // isDone == false 면 review 저장
    public void OnClickEnter()
    {
        if (checkIsDone.isOn) isDone = true;
        else if (!checkIsDone.isOn) isDone = false;

        print(isDone);

        if (isDone)
        {
            // 0. WorldManager 의 myAllBookList 업데이트
            // isDone = true 포함
            wm.myAllBookList[idx].bookName = title.text;
            wm.myAllBookList[idx].bookAuthor = author.text;
            wm.myAllBookList[idx].bookPublishInfo = publishInfo.text;
            wm.myAllBookList[idx].bookISBN = isbn.text;
            wm.myAllBookList[idx].thumbnail = thumbnail;
            wm.myAllBookList[idx].isDone = true;
            wm.myAllBookList[idx].rating = dropdown.captionText.text;
            wm.myAllBookList[idx].review = inputFieldReview.text;

            // 1. WorldManager 의 myDoneBookList 에 추가
            _MyBookInfo myBookInfo = new _MyBookInfo();

            myBookInfo.bookName = title.text;
            myBookInfo.bookAuthor = author.text;
            myBookInfo.bookPublishInfo = publishInfo.text;
            myBookInfo.bookISBN = isbn.text;
            myBookInfo.thumbnail = thumbnail;
            myBookInfo.isDone = true;
            myBookInfo.rating = dropdown.captionText.text;
            myBookInfo.review = inputFieldReview.text;

            wm.myDoneBookList.Add(myBookInfo);

            // POSt 로 보내기
        }
        else
        {
            // 0. WorldManager 의 myAllBookList review 업데이트
            // 1. WorldManager 전체 업데이트
            wm.myAllBookList[idx].bookName = title.text;
            wm.myAllBookList[idx].bookAuthor = author.text;
            wm.myAllBookList[idx].bookPublishInfo = publishInfo.text;
            wm.myAllBookList[idx].bookISBN = isbn.text;
            wm.myAllBookList[idx].thumbnail = thumbnail;
            wm.myAllBookList[idx].isDone = false;
            wm.myAllBookList[idx].rating = dropdown.captionText.text;
            wm.myAllBookList[idx].review = inputFieldReview.text;
        }

        bookManager.ShowAllBookList();
        print("111111111");
    }

   
    
    // ===============================================================
    // 등록 버튼 (누르면 <다읽은 책목록>에 추가)
    public void OnClickAddPastBook()
    {
        wm.bookPastCount++;

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
        myPastBookInfoList.Add(myPastBookInfo);
        //myPastBookListNet.Add(myPastBookInfo);

        // texture 다 뺴기
        for (int i = 0; i < 6; i++)
        {
            myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = null;
        }

        // 업데이트 된 <현재 도서 목록> 에서 받아와서 뿌리기
        int destroyBookIdx = bookManager.idx;
        myBookInfoList.RemoveAt(destroyBookIdx);
        //myBookListNet.RemoveAt(destroyBookIdx);

        // MyCurrBookPanel 의 자식의 인덱스와 myCurrBookList 의 인덱스 맞춰서 넣어줌
        for (int i = 0; i < myBookInfoList.Count; i++)
        {
            myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myBookInfoList[i].thumbnail.texture;
        }


        //HttpPostPastBookInfo();

        // 책장에 넣기
        // <다읽은책목록>의 마지막 인덱스 
        int idx = myPastBookInfoList.Count - 1;
        //int idx = myPastBookListNet.Count - 1;

        GameObject setBook = book.transform.GetChild(idx).gameObject;
        setBook.SetActive(true);
        setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);

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

    #region 텍스트 세팅 관련
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

    public void SetThumbnail(Texture texture)
    {
        thumbnail.texture = texture;
    }

/*    public void SetRating(string s)
    {
        rating.text = s;
    }*/

    public void SetReview(string s)
    {
        review.text = s;
    }

    public void SetIndex(int num)
    {
        idx = num; ;
    }

    public void SetIsDone(bool done)
    {
        isDone = done;
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
