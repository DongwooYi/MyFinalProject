using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;



// �÷��̾ å�� ������ ���� ���� �а� �ִ� å UI �� ���
public class MyBookManager : MonoBehaviour
{
    public GameObject player;   // �÷��̾�
    public GameObject myDesk;   // å��
    public GameObject myCurrBookPanel;  // ���� �а� �ִ� å ��� UI

    public GameObject myBookshelf;    // å��
    public GameObject myPastBookPanel;  // ������ å ��� UI

    public GameObject myBookPanel;  // ������ å ��� UI

    public GameObject bookFactory;  // �������� ��� ����
    public GameObject currBookInfoPanelFactory; // ���� ���� �� ����
    public GameObject pastBookFactory; // ���������� �� ����

    public Transform canvas;
    public Transform content;

    // ��������
    public Transform bookContent;
    public Transform bookContentIsDoneT;

    public WorldManager2D wm;

    //List<_MyBookInfo> myBookList = new List<_MyBookInfo>(); // ����å
    public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();  // ����å ��Ʈ��ũ

    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>(); // ���� ����
    //public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    List<_MyPastBookInfo> myPastBookList = new List<_MyPastBookInfo>(); // ����������

    public float distance = 1.5f;   // �÷��̾�� ��ü�� �Ÿ�



    void Start()
    {
        player = GameObject.Find("Character");
        //myBookList = wm.myAllBookList;
    }

    void Update()
    {
        // ���� �÷��̾ å�� ������ ����(�Ÿ� 1����)
        if (Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            //ShowClickHereCurrBook();
            ShowMyBookList();
        }
        else
        {
            myDesk.transform.GetChild(0).gameObject.SetActive(false);
        }

        // ���� �÷��̾ å�� ������ ����
        if(Vector3.Distance(player.transform.position, myBookshelf.transform.position) < distance)
        {
            //ShowClickHerePastBook();
            ShowBookIsDoneT();
        }
        else
        {
            myBookshelf.transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    /* �������� ��� ���� */
    // <å��> �տ� ���� ���������� ������ (isDone == true / false ����)
    // isDoneString �� "Y" / "N" �� ���� �ѷ����� ���� �ٸ�
    public void ShowMyBookList()
    {
        // �հ��� ���带 ����ش�
        myDesk.transform.GetChild(0).gameObject.SetActive(true);
        // �հ��� ���� �׻� ī�޶� ����
        myDesk.transform.GetChild(0).forward = Camera.main.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name == "MyDesk")
                {
                    myBookListNet.Clear();  // �������� ����Ʈ �ʱ�ȭ
                    HttpGetMyBookData();
                    // �������� ����Ʈ�� myBookLIstNet �� ���������� �������
                    print("å�� �� �������� ��ü");
                    #region �ڽ� ���� (������)
                    // �ڽ��� �ִٸ� ����
                    Transform[] childList = bookContent.GetComponentsInChildren<Transform>();
                    if (childList != null)
                    {
                        for (int i = 1; i < childList.Length; i++)
                        {
                            Destroy(childList[i].gameObject);
                        }
                    }

                    // �ڽ��� �ִٸ� ����
                    Transform[] childList1 = bookContentIsDoneT.GetComponentsInChildren<Transform>();
                    if (childList1 != null)
                    {
                        for (int i = 1; i < childList1.Length; i++)
                        {
                            Destroy(childList1[i].gameObject);
                        }
                    }
                    #endregion
                    // ���������� ����ŭ ������ ����
                    for (int i = 0; i < myBookListNet.Count; i++)
                    {
                        // ���� isDoneString �� "Y" �� ��������(done) ��Ͽ� ������
                        if (myBookListNet[i].isDoneString == "Y")
                        {
                            GameObject go = Instantiate(bookFactory, bookContentIsDoneT);
                            // ���� RawImage �� Texture �� ����Ʈ �������
                            // go.GetComponent<RawImage>().texture = myBookList[i].thumbnail.texture;
                            MyBook myBook = go.GetComponent<MyBook>();

                            //myBook.thumbnail.texture = myBookLIstNet[i].thumbnail.texture;
                            myBook.bookTitle = myBookListNet[i].bookName;
                            myBook.bookAuthor = myBookListNet[i].bookAuthor;
                            myBook.bookInfo = myBookListNet[i].bookPublishInfo;
                            myBook.bookIsbn = myBookListNet[i].bookISBN;
                            myBook.bookRating = myBookListNet[i].rating;
                            myBook.bookReview = myBookListNet[i].review;
                            myBook.isDone = true;
                            
                            // index �� i ���� �Ѱ�����ҵ�
                            myBook.idx = i;
                        }
                        // ���� isDoneString �� "N" �̸� ��������(ing) ���
                        else if (myBookListNet[i].isDoneString == "N")
                        {
                            GameObject go = Instantiate(bookFactory, bookContent);
                            // ���� RawImage �� Texture �� ����Ʈ �������
                            //go.GetComponent<RawImage>().texture = myBookLIstNet[i].thumbnail.texture;
                            MyBook myBook = go.GetComponent<MyBook>();

                            //myBook.thumbnail.texture = myBookLIstNet[i].thumbnail.texture;
                            myBook.bookTitle = myBookListNet[i].bookName;
                            myBook.bookAuthor = myBookListNet[i].bookAuthor;
                            myBook.bookInfo = myBookListNet[i].bookPublishInfo;
                            myBook.bookIsbn = myBookListNet[i].bookISBN;
                            myBook.bookRating = myBookListNet[i].rating;
                            myBook.bookReview = myBookListNet[i].review;
                            myBook.isDone = false;

                            // index �� i ���� �Ѱ�����ҵ�
                            myBook.idx = i;
                        }

                    }
                    myBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    // ���� �ۼ� �� ������
    public void ShowAllBookList()
    {
        // �ڽ��� �ִٸ� ����
        Transform[] childList = bookContent.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        // �ڽ��� �ִٸ� ����
        Transform[] childList1 = bookContentIsDoneT.GetComponentsInChildren<Transform>();
        if (childList1 != null)
        {
            for (int i = 1; i < childList1.Length; i++)
            {
                Destroy(childList1[i].gameObject);
            }
        }

        // ���������� ����ŭ ������ ����
        for (int i = 0; i < wm.myAllBookList.Count; i++)
        {
            // ���� isDone �� true �� ������ ��Ͽ� ������
            if (wm.myAllBookList[i].isDone)
            {
                GameObject go = Instantiate(bookFactory, bookContentIsDoneT);
                // ���� RawImage �� Texture �� ����Ʈ �������
                go.GetComponent<RawImage>().texture = wm.myAllBookList[i].thumbnail.texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookList[i].thumbnail.texture;
                myBook.bookTitle = wm.myAllBookList[i].bookName;
                myBook.bookAuthor = wm.myAllBookList[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookList[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookList[i].bookISBN;
                myBook.bookRating = wm.myAllBookList[i].rating;
                myBook.bookReview = wm.myAllBookList[i].review;
                myBook.isDone = wm.myAllBookList[i].isDone;
                // index �� i ���� �Ѱ�����ҵ�
                myBook.idx = i;
            }
            else
            {
                GameObject go = Instantiate(bookFactory, bookContent);
                // ���� RawImage �� Texture �� ����Ʈ �������
                go.GetComponent<RawImage>().texture = wm.myAllBookList[i].thumbnail.texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookList[i].thumbnail.texture;
                myBook.bookTitle = wm.myAllBookList[i].bookName;
                myBook.bookAuthor = wm.myAllBookList[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookList[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookList[i].bookISBN;
                myBook.bookRating = wm.myAllBookList[i].rating;
                myBook.bookReview = wm.myAllBookList[i].review;
                myBook.isDone = wm.myAllBookList[i].isDone;

                // index �� i ���� �Ѱ�����ҵ�
                myBook.idx = i;
            }

        }
    }

    /* å�� �տ��� isDone == true �� å ���� ���� */
    public void ShowBookIsDoneT()
    {
        // �հ��� ���带 ����ش�
        myDesk.transform.GetChild(0).gameObject.SetActive(true);
        // �հ��� ���� �׻� ī�޶� ����
        myDesk.transform.GetChild(0).forward = Camera.main.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name == "MyBookshelf")
                {
                    //HttpGetPastBook();  // ��Ʈ��ũ ��� -> �Լ� ����������
                    print("�ϵ����� ��� ���");

                    // �ڽ��� �ִٸ� ����
                    Transform[] childList = content.GetComponentsInChildren<Transform>();
                    if (childList != null)
                    {
                        for (int i = 1; i < childList.Length; i++)
                        {
                            Destroy(childList[i].gameObject);
                        }
                    }

                    // WorldManager �� myAllBookList �� �� isDone == true �� �͵� ������ ����
                    for (int i = 0; i < wm.myAllBookList.Count; i++)
                    {
                        if (wm.myAllBookList[i].isDone)
                        {
                            // ������ ����
                            GameObject go = Instantiate(pastBookFactory, content);
                            go.GetComponent<RawImage>().texture = wm.myAllBookList[i].thumbnail.texture;
                            MyBook pastBook = go.GetComponent<MyBook>();

                            pastBook.thumbnail.texture = myBookListNet[i].thumbnail.texture;
                            pastBook.bookTitle = myBookListNet[i].bookName;
                            pastBook.bookAuthor = myBookListNet[i].bookAuthor;
                            pastBook.bookInfo = myBookListNet[i].bookPublishInfo;
                            pastBook.bookIsbn = myBookListNet[i].bookISBN;
                            pastBook.bookRating = myBookListNet[i].rating;
                            pastBook.bookReview = myBookListNet[i].review;
                            pastBook.isDone = myBookListNet[i].isDone;

                            // index �� i ���� �Ѱ�����ҵ�
                            pastBook.idx = i;
                        }
                        else { continue; }
                    }

                    myPastBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    #region (���� ����) å ���� 
    /* ���絵�� ��� ���� */
    public void ShowClickHereCurrBook()
    {
        // �հ��� ���带 ����ش�
        myDesk.transform.GetChild(0).gameObject.SetActive(true);
        // �հ��� ���� �׻� ī�޶� ����
        myDesk.transform.GetChild(0).forward = Camera.main.transform.forward;

        myCurrBookList = wm.myBookList;
        //myBookListNet = worldManager.myBookListNet;

        // MyCurrBookPanel �� �ڽ��� �ε����� myCurrBookList �� �ε��� ���缭 �־���
        for (int i = 0; i < myCurrBookList.Count; i++)
        {
            myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myCurrBookList[i].thumbnail.texture;
            //myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myBookListNet[i].thumbnail.texture;

        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.tag == "ClickHere")
                {
                    //HttpGetCurrBook();  // ��Ʈ��ũ ���
                    print("�̹��� �ѹ��� ��������");
                    myCurrBookPanel.SetActive(true);
                    myDesk.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    /* ���������� ��� ���� */
    public void ShowClickHerePastBook()
    {
        // �հ��� ���带 ����ش�
        myBookshelf.transform.GetChild(0).gameObject.SetActive(true);
        // �հ��� ������ �չ����� �׻� ī�޶�
        myBookshelf.transform.GetChild(0).forward = Camera.main.transform.forward;

        myPastBookList = wm.myPastBookList;
        //myBookListNet = worldManager.myBookListNet;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name == "MyBookshelf")
                {
                    //HttpGetPastBook();  // ��Ʈ��ũ ��� -> �Լ� ����������
                    print("�ϵ����� ��� ���");

                    // �ڽ��� �ִٸ� ����
                    Transform[] childList = content.GetComponentsInChildren<Transform>();
                    if(childList != null)
                    {
                        for(int i = 1; i < childList.Length; i++)
                        {
                            Destroy(childList[i].gameObject);
                        }
                    }


                    // myPastBookList �� ũ�⸸ŭ ������ ����
                    for (int i = 0; i < myPastBookList.Count; i++)
                    {
                        GameObject go = Instantiate(pastBookFactory, content);
                        // ���� RawImage �� Texture �� ����Ʈ �������
                        go.GetComponent<RawImage>().texture = myPastBookList[i].thumbnail.texture;
                        MyBook pastBook = go.GetComponent<MyBook>();

                        pastBook.thumbnail.texture = myPastBookList[i].thumbnail.texture;
                        pastBook.bookTitle = myPastBookList[i].bookName;
                        pastBook.bookAuthor = myPastBookList[i].bookAuthor;
                        pastBook.bookInfo = myPastBookList[i].bookPublishInfo;
                        pastBook.bookIsbn = myPastBookList[i].bookISBN;
                        pastBook.bookRating = myPastBookList[i].rating;
                        pastBook.bookReview = myPastBookList[i].review;
                    }
                    myPastBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
    #endregion

    public GameObject me;
    public int idx;

    // ���� �а� �ִ� ���� ���� ���� �󼼺��� �Լ�
    public void OnClickCurrBook()
    {
        // ���� ������(���� ����)
        me = EventSystem.current.currentSelectedGameObject;

        // ���� �θ�(myCurrBookPanel)�� ���° �ڽ�����
        idx = me.transform.GetSiblingIndex();
        print("CurrButtonIdx: " + idx);

        // ����
        GameObject go = Instantiate(currBookInfoPanelFactory, canvas);

        CurrBookInfoPanel currBookInfoPanel = go.GetComponent<CurrBookInfoPanel>();

        currBookInfoPanel.SetTitle(myCurrBookList[idx].bookName);
        currBookInfoPanel.SetAuthor(myCurrBookList[idx].bookAuthor);
        currBookInfoPanel.SetPublishInfo(myCurrBookList[idx].bookPublishInfo);
        currBookInfoPanel.SetThumbnail(myCurrBookList[idx].thumbnail.texture);

        /*        currBookInfoPanel.SetTitle(myBookListNet[idx].bookName);
                currBookInfoPanel.SetAuthor(myBookListNet[idx].bookAuthor);
                currBookInfoPanel.SetPublishInfo(myBookListNet[idx].bookPublishInfo);
                currBookInfoPanel.SetImage(myBookListNet[idx].thumbnail.texture);*/
    }

    // �ڷ� ���� ��ư
    public void OnClickExitCurr()
    {
        myCurrBookPanel.SetActive(false);
    }
    public void OnClickExitPast()
    {
        myPastBookPanel.SetActive(false);
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


    // (�ٲ� ����) Http ��� ���� ------------------------
    // 2. å�� ������ ���� �� ȣ���� API : ���� å + ���� å �������� (��� å ���� �� ������)
    void HttpGetMyBookData()
    {
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:8080/v1/records/desk";
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

        if (type == 200)
        {
            print("��ż���. ��絵��");
            string result_data = ParseJson("[" + handler.text + "]", "data");

            titleListNet = ParseMyBookData(result_data, "bookName");
            authorListNet = ParseMyBookData(result_data, "bookAuthor");
            publishInfoListNet = ParseMyBookData(result_data, "bookPublishInfo");
            thumbnailLinkListNet = ParseMyBookData(result_data, "thumbnailLink");
            isbnListNet = ParseMyBookData(result_data, "bookISBN");
            ratingListNet = ParseMyBookData(result_data, "rating");
            reviewListNet = ParseMyBookData(result_data, "bookReview");
            isDoneListNet = ParseMyBookData(result_data, "isDone");

            // �������� �����ϴ� List �ʱ�ȭ
            myBookListNet.Clear();

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

                myBookListNet.Add(myBookInfo);
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

            print(jObject);

        }
    }

    // data parsing
    string ParseJson(string jsonText, string key)
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


    // (���� ����) ��� ���� -------------------------
    #region ���絵��
    /*    void HttpGetCurrBook()
        {
            // ������ �Խù� ��ȸ ��û(/post/1, GET)
            // HttpRequester�� ����
            HttpRequester requester = new HttpRequester();

            // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
            requester.url = "http://15.165.28.206:8080/v1/records/reading";
            requester.requestType = RequestType.GET;
            requester.onComplete = OnComplteGetMyCurrBook;

            // HttpManager ���� ��û
            HttpManager.instance.SendRequest(requester, "");
        }


        public void OnComplteGetMyCurrBook(DownloadHandler handler)
        {

            JObject jObject = JObject.Parse(handler.text);
            int type = (int)jObject["status"];
            //string type = (int)jObject["data"]["recordCode"];

            //string result_data = ParseJson("[" + handler.text + "]", "data");

            // ��� ����
            if (type == 200)
            {
                print("��ż���.���絵��");
                // 1. PlayerPref�� key�� jwt, value�� token

                string result_data = ParseJson("[" + handler.text + "]", "data");

                titleListNet = ParseCurrBookList(result_data, "bookName");
                authorListNet = ParseCurrBookList(result_data, "bookAuthor");
                publishInfoListNet = ParseCurrBookList(result_data, "bookPublishInfo");
                //pubdateList = ParseCurrBookList(result_data, "pubdate");
                isbnListNet = ParseCurrBookList(result_data, "bookISBN");
                imageListNet = ParseCurrBookList(result_data, "thumbnailLink");


                for(int i = 0; i < titleListNet.Count; i++)
                {
                    _MyBookInfo myCurrBookInfo = new _MyBookInfo();

                    myCurrBookInfo.bookName = titleListNet[i];
                    myCurrBookInfo.bookAuthor = authorListNet[i];
                    myCurrBookInfo.bookPublishInfo = publishInfoListNet[i];
                    myCurrBookInfo.bookISBN = isbnListNet[i];
                    //myCurrBookInfo.thumbnail = imageListNet[i];

                    //myBookListNet.Add(myCurrBookInfo);
                }

                print(jObject);
                //PhotonNetwork.ConnectUsingSettings();
            }
        }

        // data parsing
        string ParseJson(string jsonText, string key)
        {
            JArray parseData = JArray.Parse(jsonText);
            string result = "";

            foreach (JObject obj in parseData.Children())
            {
                result = obj.GetValue(key).ToString();
            }

            return result;
        }

        List<string> ParseCurrBookList(string jsonText, string key)
        {
            JArray parseData = JArray.Parse(jsonText);
            List<string> result = new List<string>();

            foreach (JObject obj in parseData.Children())
            {
                result.Add(obj.GetValue(key).ToString());
            }

            return result;
        }*/
    #endregion

    #region ����������
    /*    void HttpGetPastBookList()
        {
            HttpRequester requester = new HttpRequester();

            // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
            requester.url = "http://15.165.28.206:8080/v1/records/count";
            requester.requestType = RequestType.GET;
            requester.onComplete = OnComplteGetMyPastBookList;

            // HttpManager ���� ��û
            HttpManager.instance.SendRequest(requester, "");
        }

        public void OnComplteGetMyPastBookList(DownloadHandler handler)
        {
            JObject jObject = JObject.Parse(handler.text);
            int type = (int)jObject["status"];
            //string type = (int)jObject["data"]["recordCode"];

            // ��� ����
            if (type == 200)
            {
                print("��ż���.�������� ���");
                // 1. PlayerPref�� key�� jwt, value�� token
                print(jObject);
                //PhotonNetwork.ConnectUsingSettings();
            }
        }*/
    #endregion
}
