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

    void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;

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
        myPastBookInfoList.Add(myPastBookInfo);

        // 책장에 넣기

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
   /* public void OnClickUserRegister()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        ///post/1, GET, 완료되었을 때 호출되는 함수
        requester.url = "http://172.16.20.50:8080/v1/members";

        Bookdata data = new Bookdata();
        data.bookData = myPastBookInfoList;
        
        requester.body = JsonUtility.ToJson(data, true);
        requester.requestType = RequestType.POST;
        requester.onComplete = OnCompleteGetPost;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester);
    }
    public void OnCompleteGetPost(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        //print(jObject + "jobj");
        *//*int type = (int)jObject["status"];
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        // string token = (string)jObject["results"]["data"]["token"];
        print(type);
        // 통신 성공
        if (type)
        {
            // 1. 회원 가입 성공했습니다. ui
          
            print("통신 성공");
            // 2. PlayerPref에 key는 jwt, value는 token
            //PlayerPrefs.SetString("jwt", );
        }*//*
     
    }*/
}
