using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.IO;

// 책 검색, 책 등록 관리
[Serializable]
public class _MyBookInfo
{
    // 도서 정보
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string thumbnailLink;
    public string bookISBN;
    public RawImage thumbnail;
    public Texture texture;

    // 기록 관련
    public string rating;  // 평점
    public string review;   // 리뷰

    // 완독 여부
    public bool isDone;
    public string isDoneString;
    
    // 인생책 여부
    public bool isBest;
    public string isBestString;

    // 한줄
    public string nickname;

    // 대표책
    public bool isOverHead;
    public string isOverHeadString;
}


public class WorldManager2D : MonoBehaviour
{
    public GameObject searchBookPanel;  // 책검색

    public GameObject myPastBookPanel;    // 다읽은도서 목록

    public int bookPastCount;   // 다읽은도서

    public InputField inputBookTitleName;   // 책 제목 입력 칸
    public Button btnSearch;    // 검색(돋보기) 버튼

    // 도서 API 관련
    public APIManager manager;

    public List<string> titleList = new List<string>();
    public List<string> authorList = new List<string>();
    public List<string> publisherList = new List<string>();
    public List<string> pubdateList = new List<string>();
    public List<string> isbnList = new List<string>();
    public List<string> imageList = new List<string>();

    public Transform content;   // 책 목록 content
    public GameObject resultFactory;    // 도서 검색 결과

    public GameObject showBook;
    public GameObject book;
    public GameObject bookBest;


    // -------------------------------------------------------------------------------
    [Header("담은도서 리스트")]
    public List<_MyBookInfo> myAllBookListNet = new List<_MyBookInfo>();   // 담은도서
    //  ------------------------------------------------------------------------------

    public Material matBook;    // 책의 Material
    void Start()
    {
        
        showBook = GameObject.Find("ShowBook");

        book = GameObject.Find("Book");
        bookBest = GameObject.Find("myroom/MyBestBookShelf");
        print(bookBest.name);

        HttpGetMyBookData();
       
        // 책 제목 입력
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);
    }

    #region 월드 세팅
    // 월드 입장 시 월드 세팅
    // 책장, 낮은 책장, 리워드, 머리에 대표책
    public void SettingMyRoom()
    {
        int bookIdx = 0;
        int bestBookIdx = 0;
        int bookCount = 0;
        for (int i = 0; i < myAllBookListNet.Count; i++)
        {
            // (다읽은책) 만약 isDoneString == "Y" 면
            // 책장 세팅 & 개수 세기
            if (myAllBookListNet[i].isDoneString == "Y")
            {
                bookCount++;
                // 책장에 책 생성
                GameObject setBook = book.transform.GetChild(bookIdx).gameObject;
                setBook.SetActive(true);
                
                setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i] );
                bookIdx++;
            }

            // (인생책) 만약 isBestString == "Y" 면
            if (myAllBookListNet[i].isBestString == "Y")
            {
                // 낮은 책장에 책 생성
                GameObject setBestBook = bookBest.transform.GetChild(bestBookIdx).gameObject;
                setBestBook.SetActive(true);
                setBestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i]);
                bestBookIdx++;
            }

            // (대표책) 만약 isOverHead == "Y" 면
            if(myAllBookListNet[i].isOverHeadString == "Y")
            {
                // 머리에 띄우기
                showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i]);
                HttpManager.instance.TextureShowBook.texture = showBook.GetComponent<MeshRenderer>().material.mainTexture;

              
            }

        }
        #region 리워드(색상) 관련
        if (bookCount < 10)
        {
            showBook.GetComponent<Outline>().OutlineColor = Color.red;
        }
        else if (bookCount < 20)
        {
            showBook.GetComponent<Outline>().OutlineColor = Color.yellow;
        }
        else if(bookCount < 30)
        {
            showBook.GetComponent<Outline>().OutlineColor = Color.green;
        }
        else if(bookCount < 40)
        {
            showBook.GetComponent<Outline>().OutlineColor = Color.blue;
        }
        else if(bookCount < 50)
        {
            showBook.GetComponent<Outline>().OutlineColor = Color.cyan;
        }

        HttpManager.instance.outlineShowBook = showBook.GetComponent<Outline>().OutlineColor;
        #endregion
    }
    #endregion
    void OnValueChanged(string s)
    {
        btnSearch.interactable = s.Length > 0;  // 검색 버튼 활성화
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
        OnClickSearchBook();
    }

    // 책 찾기 버튼 관련
    public void OnClickSearchBookButton()
    {
        searchBookPanel.SetActive(true);
    }

    // <책검색> 뒤로 버튼 관련
    public void OnClickGoBack()
    {
        // 초기화
        // content 의 자식들 모두 삭제
        // 생성되어 있는 검색 결과 삭제
        Transform[] childList = content.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        // inputfield text 없애기
        inputBookTitleName.text = "";

        searchBookPanel.SetActive(false);
    }

    // 네트워크에서 받아온 정보 저장할 리스트
    public List<string> titleListNet = new List<string>();
    public List<string> authorListNet = new List<string>();
    public List<string> publishInfoListNet = new List<string>();
    public List<string> thumbnailLinkListNet = new List<string>();
    public List<string> isbnListNet = new List<string>();
    public List<string> ratingListNet = new List<string>();
    public List<string> reviewListNet = new List<string>();
    public List<string> isDoneListNet = new List<string>();
    public List<string> isBestsListNet = new List<string>();
    public List<Texture> thumbnailImgListNet = new List<Texture>();
    public List<string> isOverHeadListNet = new List<string>();

    #region HTTP 통신 관련
    // (바뀐 버전) Http 통신 관련 -------------------------------------------------------
    // 1. 월드 입장시 요청할 API : 읽은 책(책장), 인생책 (낮은 책장) 정보 보내주기
    public void HttpGetMyBookData()
    {            
        // 각 data 리스트들 초기화
        titleListNet.Clear();
        authorListNet.Clear();
        publishInfoListNet.Clear();
        thumbnailLinkListNet.Clear();
        thumbnailImgListNet.Clear();
        isbnListNet.Clear();
        ratingListNet.Clear();
        reviewListNet.Clear();
        isDoneListNet.Clear();
        isBestsListNet.Clear();
        isOverHeadListNet.Clear();

        myAllBookListNet.Clear();
        print("요청");
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, 완료되었을 때 호출되는 함수
        requester.url = "http://15.165.28.206:80/v1/records/myroom";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetMyBookData;

        // HttpManager 에게 요청
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnCompleteGetMyBookData(DownloadHandler handler)
    {
        // 데이터 처리
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if(type == 200)
        {
            print("통신성공. 모든도서.서재입장");
            string result_data = ParseGETJson("[" + handler.text + "]", "data");

            titleListNet = ParseMyBookData(result_data, "bookName");
            authorListNet = ParseMyBookData(result_data, "bookAuthor");
            publishInfoListNet = ParseMyBookData(result_data, "bookPublishInfo");
            thumbnailLinkListNet = ParseMyBookData(result_data, "thumbnailLink");
            isbnListNet = ParseMyBookData(result_data, "bookISBN");
            ratingListNet = ParseMyBookData(result_data, "rating");
            reviewListNet = ParseMyBookData(result_data, "bookReview");
            isDoneListNet = ParseMyBookData(result_data, "isDone");
            isBestsListNet = ParseMyBookData(result_data, "isBest");
            isOverHeadListNet = ParseMyBookData(result_data, "isOverHead");

            GETThumbnailTexture();
        }
    }

    //public Text log;
    public void GETThumbnailTexture()
    {
        StartCoroutine(GetThumbnailImg(thumbnailLinkListNet.ToArray()));
    }

    public IEnumerator GetThumbnailImg(string[] url)
    {
        for (int j = 0; j < url.Length; j++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url[j]);
            yield return www.SendWebRequest();
   

            if (www.result != UnityWebRequest.Result.Success)
            {
                print("실패");
                break;
            }
            else
            {
                Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                thumbnailImgListNet.Add(myTexture);
            }
            yield return null;
        }
        for (int i = 0; i < titleListNet.Count; i++)
        {
            _MyBookInfo myBookInfo = new _MyBookInfo();

            myBookInfo.bookName = titleListNet[i];
            myBookInfo.bookAuthor = authorListNet[i];
            myBookInfo.bookPublishInfo = publishInfoListNet[i];
            myBookInfo.thumbnailLink = thumbnailLinkListNet[i];
            myBookInfo.bookISBN = isbnListNet[i];
            myBookInfo.rating = ratingListNet[i];
            myBookInfo.review = reviewListNet[i];
            myBookInfo.isDoneString = isDoneListNet[i];
            myBookInfo.isBestString = isBestsListNet[i];
            myBookInfo.isOverHeadString = isOverHeadListNet[i];
            //myBookInfo.thumbnail = rawImages[i];
            myBookInfo.texture = thumbnailImgListNet[i];
            myAllBookListNet.Add(myBookInfo);
        }
        SettingMyRoom();
    }

    // data parsing
    public string ParseGETJson(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        string result = "";

        foreach (JObject obj in parseData.Children())
        {
            result = obj.GetValue(key).ToString();
        }

        return result;
    }

    // data 에서 key 별로 parsing
    public List<string> ParseMyBookData(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        List<string> result = new List<string>();

        foreach (JObject obj in parseData.Children())
        {
            result.Add(obj.GetValue(key).ToString());
        }
        return result;
    }

    #region 도서 API 받아오기 관련
    // 검색 버튼 관련 (돋보기 버튼)
    public GameObject mask; // 뒤 이미지 가리는 용도
    public void OnClickSearchBook()
    {
        // 검색 버튼을 클릭하면 
        mask.SetActive(true);

        // 생성되어 있는 검색 결과 삭제
        Transform[] childList = content.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        APIRequester requester = new APIRequester();

        requester.onComplete = OnCompleteSearchBook;

        manager.SendRequest(requester);
    }


    // 도서 검색 결과 출력
    public void OnCompleteSearchBook(DownloadHandler handler)
    {
        // items 의 내용을 받아옴
        string result_items = ParseJson("[" +handler.text + "]", "items");

        // 받은 items 의 title
        //string result = ParseJson(result_items, "title", 5);
        titleList = ParseJsonList(result_items, "title");
        authorList = ParseJsonList(result_items, "author");
        publisherList = ParseJsonList(result_items, "publisher");
        pubdateList = ParseJsonList(result_items, "pubdate");
        isbnList = ParseJsonList(result_items, "isbn");
        imageList = ParseJsonList(result_items, "image");

        // 도서 검색 결과 생성
        for (int i = 0; i < titleList.Count; i++)
        {
            GameObject go = Instantiate(resultFactory, content);    // 도서 검색 결과 생성

            SearchResult searchResult = go.GetComponent<SearchResult>();
            searchResult.SetBookTitle(titleList[i]);
            searchResult.SetBookAuthor(authorList[i]);
            searchResult.SetBookPublishInfo(publisherList[i] + " / " + pubdateList[i]);
            searchResult.SetIsbn(isbnList[i]);

            StartCoroutine(GetThumbnail(imageList[i],searchResult.thumbnail));
        }
    }
    #endregion


    IEnumerator GetThumbnail(string url, RawImage rawImage)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        print(www.result);

        if(www.result != UnityWebRequest.Result.Success)
        {
            print("실패");
        }
        else
        {
            //Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        yield return null;

    }


    /* 데이터 파싱 관련 (여러 개 오버라이드) */
    // data parsing
    string ParseJson(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        string result = "";

        foreach(JObject obj in parseData.Children())
        {
            result = obj.GetValue(key).ToString(); 
        }

        return result;
    }

    // 인덱스로 특정 data parsing
    string ParseJson(string jsonText, string key, int childCount)
    {
        JArray parseData = JArray.Parse(jsonText);
        string result = "";

        int index = 0;
        foreach (JObject obj in parseData.Children())
        {
            if (index == childCount)
            {
                result = obj.GetValue(key).ToString();
                break;
            }
            else
            {
                index++;
            }
        }

        return result;
    }

    List<string> ParseJsonList(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        List<string> result = new List<string>();

        foreach (JObject obj in parseData.Children())
        {
            //result = obj.GetValue(key).ToString();
            result.Add(obj.GetValue(key).ToString());
        }

        return result;
    }
    #endregion
}
