using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        BookInfo info = JsonUtility.FromJson<BookInfo>(handler.text);
        print("도서 정보 출력");
    }


    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
    }

}
