using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

// å �˻�, å ��� ����

[Serializable]
public class _MyBookInfo
{
    // ���� ����
    public string bookName;
    public string bookAuthor;
    public string bookPublishInfo;
    public string bookISBN;
    public RawImage thumbnail;

    // �Ϸ� ���� -> �а� �ִ� å ��ư: false / �� ���� å ��ư: true
    public bool isDone;
}

[Serializable]
public class _MyPastBookInfo : _MyBookInfo
{
    public string rating;  // ����
    public string review;   // ����
}

public class WorldManager2D : MonoBehaviour
{
    public GameObject searchBookPanel;  // å�˻�

    public GameObject myPastBookPanel;    // ���������� ���

    public int bookCurrCount;
    public int bookPastCount;   // ����������

    public InputField inputBookTitleName;   // å ���� �Է� ĭ
    public Button btnSearch;    // �˻�(������) ��ư

    public APIManager manager;

    public List<string> titleList = new List<string>();
    public List<string> authorList = new List<string>();
    public List<string> publisherList = new List<string>();
    public List<string> pubdateList = new List<string>();
    public List<string> isbnList = new List<string>();
    public List<string> imageList = new List<string>();

    public Transform content;   // å ��� content
    public GameObject resultFactory;    // ���� �˻� ���

    // -------------------------------------------------------------------------------
    // ���� ���� å ���
    public List<_MyBookInfo> myBookList = new List<_MyBookInfo>();
    public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    // ���� ���� å ���
    public List<_MyPastBookInfo> myPastBookList = new List<_MyPastBookInfo>();
    public List<_MyPastBookInfo> myPastBookListNet = new List<_MyPastBookInfo>();

    //  ------------------------------------------------------------------------------

    public Material matBook;    // å�� Material


    void Start()
    {
        // å ���� �Է�
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);

    }

    void OnValueChanged(string s)
    {
        btnSearch.interactable = s.Length > 0;  // �˻� ��ư Ȱ��ȭ
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
    }

    // å ã�� ��ư ����
    public void OnClickSearchBookButton()
    {
        searchBookPanel.SetActive(true);
    }

    // <������ ���� ���> ��ư ����
    public void OnClickMyPastBookPanelButton()
    {
        myPastBookPanel.SetActive(true);
    }

    // �ڷ� ��ư ����
    public void OnClickGoBack()
    {
        // �ʱ�ȭ
        // content �� �ڽĵ� ��� ����
        // �����Ǿ� �ִ� �˻� ��� ����
        Transform[] childList = content.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        // inputfield text ���ֱ�
        inputBookTitleName.text = "";

        searchBookPanel.SetActive(false);
    }

    // �˻� ��ư ���� (������ ��ư)
    public void OnClickSearchBook()
    {
        // �˻� ��ư�� Ŭ���ϸ� 

        // �����Ǿ� �ִ� �˻� ��� ����
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


    // ���� �˻� ��� ���
    public void OnCompleteSearchBook(DownloadHandler handler)
    {
        // items �� ������ �޾ƿ�
        string result_items = ParseJson("[" +handler.text + "]", "items");

        // ���� items �� title
        //string result = ParseJson(result_items, "title", 5);
        titleList = ParseJsonList(result_items, "title");
        authorList = ParseJsonList(result_items, "author");
        publisherList = ParseJsonList(result_items, "publisher");
        pubdateList = ParseJsonList(result_items, "pubdate");
        isbnList = ParseJsonList(result_items, "isbn");
        imageList = ParseJsonList(result_items, "image");

        // ���� �˻� ��� ����
        for (int i = 0; i < titleList.Count; i++)
        {
            GameObject go = Instantiate(resultFactory, content);    // ���� �˻� ��� ����

            SearchResult searchResult = go.GetComponent<SearchResult>();
            searchResult.SetBookTitle(titleList[i]);
            searchResult.SetBookAuthor(authorList[i]);
            searchResult.SetBookPublishInfo(publisherList[i] + " / " + pubdateList[i]);
            searchResult.SetIsbn(isbnList[i]);

            StartCoroutine(GetThumbnail(imageList[i],searchResult.thumbnail));
        }
    }

    // �˻� ��ư ���� (������ ��ư)
    public void OnClickSearchBookGroup()
    {
        // �˻� ��ư�� Ŭ���ϸ� 

        // �����Ǿ� �ִ� �˻� ��� ����
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
            print("����");
        }
        else
        {
            //Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        yield return null;

    }


    /* ������ �Ľ� ���� (���� �� �������̵�) */
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

    // �ε����� Ư�� data parsing
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
