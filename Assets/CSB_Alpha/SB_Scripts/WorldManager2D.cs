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
    public string thumbnailLink;
    public string bookISBN;
    public RawImage thumbnail;

    // ��� ����
    public string rating;  // ����
    public string review;   // ����
    //public string bookRating;  // ����
    //public string bookReview;   // ����

    // �ϵ� ����
    public bool isDone;
    public string isDoneString;

    // �λ�å ����
    public bool isBest;
    public string isBestString;
}

[Serializable]
public class _MyPastBookInfo : _MyBookInfo
{
    //public string rating;  // ����
    //public string review;   // ����
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

    public GameObject showBook;
    GameObject book;
    GameObject bookBest;


    // -------------------------------------------------------------------------------
    public List<_MyBookInfo> myAllBookList = new List<_MyBookInfo>();   // ��������
    public List<_MyBookInfo> myAllBookListNet = new List<_MyBookInfo>();   // ��������

    public List<_MyBookInfo> myDoneBookList = new List<_MyBookInfo>();  // isDone == true ����

    // ���� ����
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
        book = GameObject.Find("Book");
        bookBest = GameObject.Find("MyBestBookshelf");

        HttpGetMyBookData();

        // å ���� �Է�
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);

    }

    private void Update()
    {
/*        if (bookPastCount > 2)
        {
            showBook.GetComponent<Outline>().OutlineColor = Color.yellow;
        }*/
    }

    // ���� ���� �� ���� ����
    // å��, ���� å��, ������
    int bookIdx = 0;
    int bestBookIdx = 0;
    int bookCount;
    void SettingMyRoom()
    {
        for(int i = 0; i < myAllBookListNet.Count; i++)
        {
            // ���� isDoneString == "Y" ��
            // å�� ���� & ���� ����
            if (myAllBookListNet[i].isDoneString == "Y")
            {
                bookCount++;
                // å�忡 å ����
                GameObject setBook = book.transform.GetChild(bookIdx).gameObject;
                setBook.SetActive(true);
                setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", myAllBookListNet[i].thumbnail.texture);
                bookIdx++;
            }

            // ���� isBestString == "Y" ��
            if (myAllBookListNet[i].isBestString == "Y")
            {
                // ���� å�忡 å ����
                GameObject setBestBook = bookBest.transform.GetChild(bestBookIdx).gameObject;
                setBestBook.SetActive(true);
                setBestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", myAllBookListNet[i].thumbnail.texture);
                bestBookIdx++;
            }
        }

        // ������ ����
        if(bookCount > 2)
        {
            showBook.GetComponent<Outline>().OutlineColor = Color.yellow;
        }
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

    // ��Ʈ��ũ���� �޾ƿ� ���� ������ ����Ʈ
    public List<string> titleListNet = new List<string>();
    public List<string> authorListNet = new List<string>();
    public List<string> publishInfoListNet = new List<string>();
    public List<string> thumbnailLinkListNet = new List<string>();
    public List<string> isbnListNet = new List<string>();
    public List<string> ratingListNet = new List<string>();
    public List<string> reviewListNet = new List<string>();
    public List<string> isDoneListNet = new List<string>();
    public List<string> isBestsListNet = new List<string>();

    // (�ٲ� ����) Http ��� ���� -------------------------------------------------------
    // 1. ���� ����� ��û�� API : ���� å(å��), �λ�å (���� å��) ���� �����ֱ�
    void HttpGetMyBookData()
    {
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:8080/v1/records/myroom";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetMyBookData;

        // HttpManager ���� ��û
        HttpManager.instance.SendRequest(requester, "");
    }

    void OnCompleteGetMyBookData(DownloadHandler handler)
    {
        // ������ ó��
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if(type == 200)
        {
            print("��ż���. ��絵��");
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

            myAllBookListNet.Clear();

            // �������� �����ϴ� List�� �־��ֱ�
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

                myAllBookListNet.Add(myBookInfo);
            }

            // �� data ����Ʈ�� �ʱ�ȭ
            titleListNet.Clear();
            authorListNet.Clear();
            publishInfoListNet.Clear();
            thumbnailLinkListNet.Clear();
            isbnListNet.Clear();
            ratingListNet.Clear();
            reviewListNet.Clear();
            isDoneListNet.Clear();
            isBestsListNet.Clear();

            print(jObject);
        }
    }

    // data parsing
    string ParseGETJson(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        string result = "";

        foreach (JObject obj in parseData.Children())
        {
            result = obj.GetValue(key).ToString();
        }

        return result;
    }

    // data ���� key ���� parsing
    List<string> ParseMyBookData(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        List<string> result = new List<string>();

        foreach (JObject obj in parseData.Children())
        {
            result.Add(obj.GetValue(key).ToString());
        }

        return result;
    }


    #region ���� API �޾ƿ��� ����
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
    #endregion

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
