using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class CurrBookInfoPanel : MonoBehaviour
{
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();
    //List<_MyPastBookInfo> myPastBookListNet = new List<_MyPastBookInfo>();

    List<_MyBookInfo> myBookInfoList = new List<_MyBookInfo>();
    //List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;

    public Dropdown dropdown;

    public InputField inputFieldReview; // 리뷰 입력 칸
    public Button btnEnter; // 등록하기 버튼

    public GameObject player;   // 플레이어
    public GameObject bookQuad0;   // 머리 위 책 관련
    public GameObject bookQuad1;   // 머리 위 책 관련

    GameObject book;

    // 등록됨 안내 메시지 띄우기
    public GameObject alarmFactory;

    public Toggle headBook;

    MyBookManager bookManager;
    WorldManager2D wm;

    GameObject myCurrBookPanel;

    public void ToggleHead(Toggle headBook)
    {
        print("토글");
        if (headBook.isOn)
        {
            print("토글" + headBook.isOn);
            bookQuad0.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            bookQuad1.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            player.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
        }
    }

    void Start()
    {
        player = GameObject.Find("Character");

        bookQuad0 = GameObject.Find("BookQuad");
        bookQuad1 = GameObject.Find("BookQuad(1)");


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
