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

    // 검색 버튼 관련
    public void OnClickSearchBook()
    {
        // 검색 버튼을 클릭하면 
        APIRequester requester = new APIRequester();

        requester.onComplete = OnCompleteSearchBook;

        manager.SendRequest(requester);
    }

    // 도서 검색 결과 출력
    public void OnCompleteSearchBook(DownloadHandler handler)
    {
        // 여러 개 출력되니까 List 로 받음

        //BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(handler.text);
        // 제목 출력
        //bookInfo.title 
        print("도서 정보 출력\t" + handler.text);
        string result_items = ParseJson("[" +handler.text + "]", "items");
        string result = ParseJson(result_items, "title", 5
            );
        print(result);
    }


    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
    }

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
}
