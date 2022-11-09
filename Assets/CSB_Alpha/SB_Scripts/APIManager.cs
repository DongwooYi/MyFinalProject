using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{

    // �˻��� å ����
    public WorldManager2D worldManager;

    // ������ ��û
    public void SendRequest(APIRequester requester)
    {
        StartCoroutine(SearchBooks(requester));
    }

    IEnumerator SearchBooks(APIRequester requester)
    {
        // �˻��� (å ����)
        string bookTitle = worldManager.inputBookTitleName.text;

        // Request url + �˻��ϴ� ���� ����
        string url = "https://openapi.naver.com/v1/search/book.json" + "?query=" + bookTitle;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        webRequest.SetRequestHeader("X-Naver-Client-Id", "8JF68Vc47xEH6TF4aQbV");
        webRequest.SetRequestHeader("X-Naver-Client-Secret", "1eCt0Vbvqo");

        yield return webRequest.SendWebRequest();   // ������ ��ٸ�

        // ����
        if(webRequest.result == UnityWebRequest.Result.Success)
        {
            requester.onComplete(webRequest.downloadHandler);
        }
        else
        {
            print("����");
        }
        yield return null;
    }

}
