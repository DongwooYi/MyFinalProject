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

    public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();  // ����å ��Ʈ��ũ

    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>(); // ���� ����

    public float distance = 1.5f;   // �÷��̾�� ��ü�� �Ÿ�
    public float test;   // �÷��̾�� ��ü�� �Ÿ�
    public float test1;   // �÷��̾�� ��ü�� �Ÿ�

    void Start()
    {
        player = GameObject.Find("Character");
    }

    void Update()
    {
        test = Vector3.Distance(player.transform.position, myDesk.transform.position);
        test1 = Vector3.Distance(player.transform.position, myBookshelf.transform.position);
        // ���� �÷��̾ å�� ������ ����(�Ÿ� 1����)
        if (Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            ShowMyBookList();
        }
        else
        {
            myDesk.transform.GetChild(0).gameObject.SetActive(false);
        }

        // ���� �÷��̾ å�� ������ ����
        if(Vector3.Distance(player.transform.position, myBookshelf.transform.position) < 3.5f)
        {
            //print("11");
            ShowBookIsDoneT();
        }
        else
        {
            myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    /* �������� ��� ���� */
    // <å��> �տ� ���� ���������� ������ (isDone == true / false ����)
    // isDoneString �� "Y" / "N" �� ���� �ѷ����� ���� �ٸ�
    int rayCount = 0;

    public void ShowMyBookList()
    {
        if (rayCount > 0)
        {
            return;
        }

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
                    // ��Ʈ��ũ���� �޾ƿ� ���� ������ ����Ʈ
                    wm.titleListNet.Clear();
                    wm.authorListNet.Clear();
                    wm.publishInfoListNet.Clear();
                    wm.thumbnailLinkListNet.Clear();
                    wm.isbnListNet.Clear();
                    wm.ratingListNet.Clear();
                    wm.reviewListNet.Clear();
                    wm.isDoneListNet.Clear();
                    wm.isBestsListNet.Clear();

                    wm.thumbnailImgListNet.Clear();

                    wm.myAllBookListNet.Clear();  // �������� ����Ʈ �ʱ�ȭ
                    HttpGetMyBookData();
                    // �������� ����Ʈ�� myBookLIstNet �� ���������� �������
                    // �������� ����Ʈ�� wm.myAllBookListNet �� ���������� �������
                    print("å�� �� �������� ��ü ��ũ�Ѻ信 ��ġ");

                    // ���������� ����ŭ ������ ����

                    //MakePrefab();
                    myBookPanel.SetActive(true);
                    myDesk.transform.GetChild(0).gameObject.SetActive(false);

                    rayCount++;
                    return;
                }
            }
        }
    }

    public void MakePrefab()
    {
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
        for (int i = 0; i < wm.myAllBookListNet.Count; i++)
        {
            // ���� isDoneString �� "Y" �� ��������(done) ��Ͽ� ������
            if (wm.myAllBookListNet[i].isDoneString == "Y")
            {
                GameObject go = Instantiate(bookFactory, bookContentIsDoneT);
                // ���� RawImage �� Texture �� ����Ʈ �������
                go.GetComponent<RawImage>().texture = wm.myAllBookListNet[i].texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookListNet[i].texture;
                myBook.bookTitle = wm.myAllBookListNet[i].bookName;
                myBook.bookAuthor = wm.myAllBookListNet[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookListNet[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookListNet[i].bookISBN;
                myBook.bookRating = wm.myAllBookListNet[i].rating;
                myBook.bookReview = wm.myAllBookListNet[i].review;
                myBook.isDone = true;

                // index �� i ���� �Ѱ�����ҵ�
                myBook.idx = i;
            }
            // ���� isDoneString �� "N" �̸� ��������(ing) ���
            else if (wm.myAllBookListNet[i].isDoneString == "N")
            {
                GameObject go = Instantiate(bookFactory, bookContent);
                // ���� RawImage �� Texture �� ����Ʈ �������
                go.GetComponent<RawImage>().texture = wm.myAllBookListNet[i].texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookListNet[i].texture;
                myBook.bookTitle = wm.myAllBookListNet[i].bookName;
                myBook.bookAuthor = wm.myAllBookListNet[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookListNet[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookListNet[i].bookISBN;
                myBook.bookRating = wm.myAllBookListNet[i].rating;
                myBook.bookReview = wm.myAllBookListNet[i].review;
                myBook.isDone = false;

                // index �� i ���� �Ѱ�����ҵ�
                myBook.idx = i;
            }
        }
    }
    /* <å�� ��>���� isDone == true �� å ���� ���� */
    int doneBookCount = 0;
    public void ShowBookIsDoneT()
    {
        if (doneBookCount > 0)
        {
            return;
        }

        print("������ å��");
        // �հ��� ���带 ����ش�
        myBookshelf.transform.GetChild(0).gameObject.SetActive(true);
        // �հ��� ���� �׻� ī�޶� ����
        myBookshelf.transform.GetChild(0).forward = Camera.main.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            print("Ŭ���ߴ�?");
            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name.Contains("MyBookshelf"))
                {
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
                    for (int i = 0; i < wm.myAllBookListNet.Count; i++)
                    {
                        if (wm.myAllBookListNet[i].isDoneString == "Y")
                        {
                            // ������ ����
                            GameObject go = Instantiate(pastBookFactory, content);
                            go.GetComponent<RawImage>().texture = wm.myAllBookListNet[i].texture;
                            MyBook pastBook = go.GetComponent<MyBook>();

                            pastBook.thumbnail.texture = wm.myAllBookListNet[i].texture;
                            pastBook.bookTitle = wm.myAllBookListNet[i].bookName;
                            pastBook.bookAuthor = wm.myAllBookListNet[i].bookAuthor;
                            pastBook.bookInfo = wm.myAllBookListNet[i].bookPublishInfo;
                            pastBook.bookIsbn = wm.myAllBookListNet[i].bookISBN;
                            pastBook.bookRating = wm.myAllBookListNet[i].rating;
                            pastBook.bookReview = wm.myAllBookListNet[i].review;
                            pastBook.isDone = true;
                            pastBook.isBestStr = wm.myAllBookListNet[i].isBestString;

                            // index �� i ���� �Ѱ�����ҵ�
                            pastBook.idx = i;
                        }
                        else { continue; }
                    }

                    myPastBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);

                    doneBookCount++;
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
        rayCount = 0;
        myCurrBookPanel.SetActive(false);
    }
    public void OnClickExitPast()
    {
        doneBookCount = 0;
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
    public List<string> isBestsListNet = new List<string>();
    public List<Texture> thumbnailImgListNet = new List<Texture>();


    // (�ٲ� ����) Http ��� ���� ------------------------
    // 2. å�� ������ ���� �� ȣ���� API : ���� å + ���� å �������� (��� å ���� �� ������)
    void HttpGetMyBookData()
    {
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:80/v1/records/desk";
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
            print("��ż���. ��絵��.å���");
            string result_data = ParseJson("[" + handler.text + "]", "data");

            titleListNet = ParseMyBookData(result_data, "bookName");
            authorListNet = ParseMyBookData(result_data, "bookAuthor");
            publishInfoListNet = ParseMyBookData(result_data, "bookPublishInfo");
            thumbnailLinkListNet = ParseMyBookData(result_data, "thumbnailLink");
            isbnListNet = ParseMyBookData(result_data, "bookISBN");
            ratingListNet = ParseMyBookData(result_data, "rating");
            reviewListNet = ParseMyBookData(result_data, "bookReview");
            isDoneListNet = ParseMyBookData(result_data, "isDone");
            isBestsListNet = ParseMyBookData(result_data, "isBest");

            StartCoroutine(GetThumbnailImg(thumbnailLinkListNet.ToArray()));   //, myBookInfo.thumbnail)

            print(jObject);
        }
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

            myBookInfo.texture = thumbnailImgListNet[i];

            wm.myAllBookListNet.Add(myBookInfo);
        }
        // å�� �� panel ����
        MakePrefab();
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
}
