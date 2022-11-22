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

    List<_MyBookInfo> myBookList = new List<_MyBookInfo>(); // ����å

    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>(); // ���� ����
    //public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    List<_MyPastBookInfo> myPastBookList = new List<_MyPastBookInfo>(); // ����������

    public float distance = 1.5f;   // �÷��̾�� ��ü�� �Ÿ�



    void Start()
    {
        player = GameObject.Find("Character");
        myBookList = wm.myAllBookList;
        // ���⼭ �� ������ �� �� �о��� å �ѹ� �ѷ��ְ� ����
        //HttpGetPastBookList();    
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
    // å��տ� ���� ���������� ������ (isDone == true / false ����)
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
                    //HttpGetPastBook();  // ��Ʈ��ũ ��� -> �Լ� ����������
                    print("�������� ��� ���");

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
                    for (int i = 0; i < myBookList.Count; i++)
                    {
                        // ���� isDone �� true �� ������ ��Ͽ� ������
                        if (myBookList[i].isDone)
                        {
                            GameObject go = Instantiate(bookFactory, bookContentIsDoneT);
                            // ���� RawImage �� Texture �� ����Ʈ �������
                            go.GetComponent<RawImage>().texture = myBookList[i].thumbnail.texture;
                            MyBook myBook = go.GetComponent<MyBook>();

                            myBook.thumbnail.texture = myBookList[i].thumbnail.texture;
                            myBook.bookTitle = myBookList[i].bookName;
                            myBook.bookAuthor = myBookList[i].bookAuthor;
                            myBook.bookInfo = myBookList[i].bookPublishInfo;
                            myBook.bookIsbn = myBookList[i].bookISBN;
                            myBook.bookRating = myBookList[i].rating;
                            myBook.bookReview = myBookList[i].review;
                            myBook.isDone = myBookList[i].isDone;
                            // index �� i ���� �Ѱ�����ҵ�
                            myBook.idx = i;
                        }
                        else
                        {
                            GameObject go = Instantiate(bookFactory, bookContent);
                            // ���� RawImage �� Texture �� ����Ʈ �������
                            go.GetComponent<RawImage>().texture = myBookList[i].thumbnail.texture;
                            MyBook myBook = go.GetComponent<MyBook>();

                            myBook.thumbnail.texture = myBookList[i].thumbnail.texture;
                            myBook.bookTitle = myBookList[i].bookName;
                            myBook.bookAuthor = myBookList[i].bookAuthor;
                            myBook.bookInfo = myBookList[i].bookPublishInfo;
                            myBook.bookIsbn = myBookList[i].bookISBN;
                            myBook.bookRating = myBookList[i].rating;
                            myBook.bookReview = myBookList[i].review;
                            myBook.isDone = myBookList[i].isDone;

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

                            pastBook.thumbnail.texture = myBookList[i].thumbnail.texture;
                            pastBook.bookTitle = myBookList[i].bookName;
                            pastBook.bookAuthor = myBookList[i].bookAuthor;
                            pastBook.bookInfo = myBookList[i].bookPublishInfo;
                            pastBook.bookIsbn = myBookList[i].bookISBN;
                            pastBook.bookRating = myBookList[i].rating;
                            pastBook.bookReview = myBookList[i].review;
                            pastBook.isDone = myBookList[i].isDone;

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

    public List<string> titleListNet = new List<string>();
    public List<string> authorListNet = new List<string>();
    public List<string> publishInfoListNet = new List<string>();
    //public List<string> pubdateList = new List<string>();
    public List<string> isbnListNet = new List<string>();
    public List<string> imageListNet = new List<string>();


    // ��� ���� -------------------------
    #region ���絵��
    void HttpGetCurrBook()
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
    }
    #endregion

    void HttpGetPastBookList()
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
    }

}
