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

/*    public _MyBookInfo(string name, string author, string info, string link, string ISBN, string isDoneStr, string isBestStr)
    {
        bookName = name;
        bookAuthor = author;
        bookPublishInfo = info;
        thumbnailLink = link;
        bookISBN = ISBN;
        isDoneString = isDoneStr;
        isBestString = isBestStr;
    }*/
}


public class WorldManager2D : MonoBehaviour
{
    public GameObject searchBookPanel;  // å�˻�

    public GameObject myPastBookPanel;    // ���������� ���

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
    [Header("�������� ����Ʈ")]
    public List<_MyBookInfo> myAllBookListNet = new List<_MyBookInfo>();   // ��������

    public List<_MyBookInfo> myDoneBookList = new List<_MyBookInfo>();  // isDone == true ����
    //  ------------------------------------------------------------------------------

    public Material matBook;    // å�� Material

    private void Awake()
    {
        
    }

    void Start()
    {
        book = GameObject.Find("Book");
        bookBest = GameObject.Find("myroom/MyBestBookshelf");


        HttpGetMyBookData();
       

        // å ���� �Է�
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);
    }

    private void Update()
    {
        // ������ ����
        //SettingMyRoom();
    }

        int bookIdx = 0;
        int bestBookIdx = 0;
        int bookCount = 0;
    public Texture tex;
    // ���� ���� �� ���� ����
    // å��, ���� å��, ������
    void SettingMyRoom()
    {
        
        for (int i = 0; i < myAllBookListNet.Count; i++)
        {
            // ���� isDoneString == "Y" ��
            // å�� ���� & ���� ����
            if (myAllBookListNet[i].isDoneString == "Y")
            {
                bookCount++;
                // å�忡 å ����
                GameObject setBook = book.transform.GetChild(bookIdx).gameObject;
                setBook.SetActive(true);
                
                setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i] );
                //log.text = $" �̸� : {book.transform.GetChild(i).gameObject.name}, �����: {thumbnailImgListNet[i].name}";
                bookIdx++;
            }

            // ���� isBestString == "Y" ��
            if (myAllBookListNet[i].isBestString == "Y")
            {
                // ���� å�忡 å ����
                GameObject setBestBook = bookBest.transform.GetChild(bestBookIdx).gameObject;
                setBestBook.SetActive(true);
                setBestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnailImgListNet[i]);
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
    public List<Texture> thumbnailImgListNet = new List<Texture>();

    // �׽�Ʈ�� 
    public List<RawImage> rawImagethumbnailImgList = new List<RawImage>();

    // (�ٲ� ����) Http ��� ���� -------------------------------------------------------
    // 1. ���� ����� ��û�� API : ���� å(å��), �λ�å (���� å��) ���� �����ֱ�
    void HttpGetMyBookData()
    {
        print("��û");
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:80/v1/records/myroom";
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

            myAllBookListNet.Clear();
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

            /*            for (int i = 0; i < titleListNet.Count; i++)
                        {
                            _MyBookInfo test = new _MyBookInfo(titleListNet[i], authorListNet[i],  publishInfoListNet[i], thumbnailLinkListNet[i], isbnListNet[i], isDoneListNet[i], isBestsListNet[i]);

                        }*/
            GETThumbnailTexture();



           
            
        }
    }

    public List<RawImage> rawImages = new List<RawImage>();
    //public RawImage[] raws;
    public Text log;
    void GETThumbnailTexture()
    {
            StartCoroutine(GetThumbnailImg(thumbnailLinkListNet.ToArray()));
      
    }

    IEnumerator GetThumbnailImg(string[] url)
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
            //myBookInfo.thumbnail = rawImages[i];
            myBookInfo.texture = thumbnailImgListNet[i];
            myAllBookListNet.Add(myBookInfo);

        }
        SettingMyRoom();
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

        print(www.result);

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
