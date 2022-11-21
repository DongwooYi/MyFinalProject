using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

// 책 검색, 책 등록 관리

[Serializable]
public class _MyBookInfo
{
    // 도서 정보
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
    public RawImage thumbnail;

    // 완료 여부 -> 읽고 있는 책 버튼: false / 다 읽은 책 버튼: true
    public bool isDone;
}

[Serializable]
public class _MyPastBookInfo : _MyBookInfo
{
    public string rating;  // 평점
    public string review;   // 리뷰
}

public class WorldManager2D : MonoBehaviour
{
    public GameObject searchBookPanel;  // 책검색

    public GameObject myPastBookPanel;    // 다읽은도서 목록

    public int bookCurrCount;
    public int bookPastCount;   // 다읽은도서

    public InputField inputBookTitleName;   // 책 제목 입력 칸
    public Button btnSearch;    // 검색(돋보기) 버튼

    public APIManager manager;

    public List<string> titleList = new List<string>();
    public List<string> authorList = new List<string>();
    public List<string> publisherList = new List<string>();
    public List<string> pubdateList = new List<string>();
    public List<string> isbnList = new List<string>();
    public List<string> imageList = new List<string>();

    public Transform content;   // 책 목록 content
    public GameObject resultFactory;    // 도서 검색 결과

    // -------------------------------------------------------------------------------
    // 나의 현재 책 목록
    public List<_MyBookInfo> myBookList = new List<_MyBookInfo>();
    public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    // 나의 지난 책 목록
    public List<_MyPastBookInfo> myPastBookList = new List<_MyPastBookInfo>();
    public List<_MyPastBookInfo> myPastBookListNet = new List<_MyPastBookInfo>();

    //  ------------------------------------------------------------------------------

    public Material matBook;    // 책의 Material


    void Start()
    {
        // 책 제목 입력
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);

    }

    void OnValueChanged(string s)
    {
        btnSearch.interactable = s.Length > 0;  // 검색 버튼 활성화
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
    }

    // 책 찾기 버튼 관련
    public void OnClickSearchBookButton()
    {
        searchBookPanel.SetActive(true);
    }

    // <다읽은 도서 목록> 버튼 관련
    public void OnClickMyPastBookPanelButton()
    {
        myPastBookPanel.SetActive(true);
    }

    // 뒤로 버튼 관련
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

    // 검색 버튼 관련 (돋보기 버튼)
    public void OnClickSearchBook()
    {
        // 검색 버튼을 클릭하면 

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

    // 검색 버튼 관련 (돋보기 버튼)
    public void OnClickSearchBookGroup()
    {
        // 검색 버튼을 클릭하면 

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

        requester.onComplete = OnCompleteBook;

        manager.SendRequest(requester);
    }

    public Text textBookName;
    public void OnCompleteBook(DownloadHandler handler)
    {
        string result_items = ParseJson("[" + handler.text + "]", "items");

        titleList = ParseJsonList(result_items, "title");
        textBookName.text= result_items;
    }

    IEnumerator GetThumbnail(string url, RawImage rawImage)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            print("실패");
        }
        else
        {
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
   
}
