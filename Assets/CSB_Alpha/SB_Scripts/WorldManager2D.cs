using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

// 책 검색, 책 등록 관리

public class WorldManager2D : MonoBehaviour
{
    public InputField inputBookTitleName;
    public Button btnSearch;

    public APIManager manager;

    public List<string> titleList = new List<string>();
    public List<string> authorList = new List<string>();
    public List<string> publisherList = new List<string>();
    public List<string> pubdateList = new List<string>();
    public List<string> imageList = new List<string>();

    public Transform content;
    public GameObject resultFactory;

    void Start()
    {
        // 책 제목 입력
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);

    }

    void Update()
    {
        
    }


    void OnValueChanged(string s)
    {
        btnSearch.interactable = s.Length > 0;  // 검색 버튼 활성화
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
    }

    // 검색 버튼 관련
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
        imageList = ParseJsonList(result_items, "image");

        // 도서 검색 결과 생성
        for(int i=0; i < titleList.Count; i++)
        {
            GameObject go = Instantiate(resultFactory, content);    // 도서 검색 결과 생성

            SearchResult searchResult = go.GetComponent<SearchResult>();
            searchResult.SetBookTitle(titleList[i]);
            searchResult.SetBookAuthor(authorList[i]);
            searchResult.SetBookPublishInfo(publisherList[i] + " / " + pubdateList[i]);


        }

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
