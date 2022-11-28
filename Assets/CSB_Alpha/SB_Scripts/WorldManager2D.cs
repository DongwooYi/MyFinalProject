using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.IO;

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
    public Texture texture;

    // ��� ����
    public string rating;  // ����
    public string review;   // ����

    // �ϵ� ����
    public bool isDone;
    public string isDoneString;
    
    // �λ�å ����
    public bool isBest;
    public string isBestString;

    // ����
    public string nickname;

    // ��ǥå
    public bool isOverHead;
    public string isOverHeadString;
}


public class WorldManager2D : MonoBehaviour
{
    public GameObject searchBookPanel;  // å�˻�

    public GameObject myPastBookPanel;    // ���������� ���

    public int bookPastCount;   // ����������

    public InputField inputBookTitleName;   // å ���� �Է� ĭ
    public Button btnSearch;    // �˻�(������) ��ư

    // ���� API ����
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
    public GameObject book;
    public GameObject bookBest;


    // -------------------------------------------------------------------------------
    [Header("�������� ����Ʈ")]
    public List<_MyBookInfo> myAllBookListNet = new List<_MyBookInfo>();   // ��������
    //  ------------------------------------------------------------------------------

    public Material matBook;    // å�� Material
    void Start()
    {
        
        showBook = GameObject.Find("ShowBook");

        book = GameObject.Find("Book");
        bookBest = GameObject.Find("myroom/MyBestBookShelf");
        print(bookBest.name);

        HttpGetMyBookData();
       
        // å ���� �Է�
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);
    }

    #region ���� ����
    // ���� ���� �� ���� ����
    // å��, ���� å��, ������, �Ӹ��� ��ǥå
    public void SettingMyRoom()
    {
        int bookIdx = 0;
        int bestBookIdx = 0;
        int bookCount = 0;
        for (int i = 0; i < myAllBookListNet.Count; i++)
        {
            // (������å) ���� isDoneString == "Y" ��
            // å�� ���� & ���� ����
            if (myAllBookListNet[i].isDoneString == "Y")
            {
                bookCount++;
                // å�忡 å ����
                GameObject setBook = book.transform.GetChild(bookIdx).gameObject;
                setBook.SetActive(true);
                
                setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i] );
                bookIdx++;
            }

            // (�λ�å) ���� isBestString == "Y" ��
            if (myAllBookListNet[i].isBestString == "Y")
            {
                // ���� å�忡 å ����
                GameObject setBestBook = bookBest.transform.GetChild(bestBookIdx).gameObject;
                setBestBook.SetActive(true);
                setBestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i]);
                bestBookIdx++;
            }

            // (��ǥå) ���� isOverHead == "Y" ��
            if(myAllBookListNet[i].isOverHeadString == "Y")
            {
                // �Ӹ��� ����
                showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i]);
                HttpManager.instance.TextureShowBook.texture = showBook.GetComponent<MeshRenderer>().material.mainTexture;

              
            }

        }
        #region ������(����) ����
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
        btnSearch.interactable = s.Length > 0;  // �˻� ��ư Ȱ��ȭ
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
        OnClickSearchBook();
    }

    // å ã�� ��ư ����
    public void OnClickSearchBookButton()
    {
        searchBookPanel.SetActive(true);
    }

    // <å�˻�> �ڷ� ��ư ����
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
    public List<Texture> thumbnailImgListNet = new List<Texture>();
    public List<string> isOverHeadListNet = new List<string>();

    #region HTTP ��� ����
    // (�ٲ� ����) Http ��� ���� -------------------------------------------------------
    // 1. ���� ����� ��û�� API : ���� å(å��), �λ�å (���� å��) ���� �����ֱ�
    public void HttpGetMyBookData()
    {            
        // �� data ����Ʈ�� �ʱ�ȭ
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
        print("��û");
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:80/v1/records/myroom";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetMyBookData;

        // HttpManager ���� ��û
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnCompleteGetMyBookData(DownloadHandler handler)
    {
        // ������ ó��
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if(type == 200)
        {
            print("��ż���. ��絵��.��������");
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
                print("����");
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

    // data ���� key ���� parsing
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

    #region ���� API �޾ƿ��� ����
    // �˻� ��ư ���� (������ ��ư)
    public GameObject mask; // �� �̹��� ������ �뵵
    public void OnClickSearchBook()
    {
        // �˻� ��ư�� Ŭ���ϸ� 
        mask.SetActive(true);

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


    IEnumerator GetThumbnail(string url, RawImage rawImage)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        print(www.result);

        if(www.result != UnityWebRequest.Result.Success)
        {
            print("����");
        }
        else
        {
            //Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
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
    #endregion
}
