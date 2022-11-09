using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{

    // 검색할 책 제목
    public WorldManager2D worldManager;

    // 서버에 요청
    public void SendRequest(APIRequester requester)
    {
        StartCoroutine(SearchBooks(requester));
    }

    IEnumerator SearchBooks(APIRequester requester)
    {
        // 검색어 (책 제목)
        string bookTitle = worldManager.inputBookTitleName.text;

        // Request url + 검색하는 도서 제목
        string url = "https://openapi.naver.com/v1/search/book.json" + "?query=" + bookTitle;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        webRequest.SetRequestHeader("X-Naver-Client-Id", "8JF68Vc47xEH6TF4aQbV");
        webRequest.SetRequestHeader("X-Naver-Client-Secret", "1eCt0Vbvqo");

        yield return webRequest.SendWebRequest();   // 응답을 기다림

        // 성공
        if(webRequest.result == UnityWebRequest.Result.Success)
        {
            requester.onComplete(webRequest.downloadHandler);
        }
        else
        {
            print("실패");
        }
        yield return null;
    }

}
