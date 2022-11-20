using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


public class MyReviewPanel : MonoBehaviour
{
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();
    //public List<_MyPastBookInfo> myPastBookListNet = new List<_MyPastBookInfo>();

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;

    public Dropdown dropdown;

    public InputField inputFieldReview; // ���� �Է� ĭ

    public Button btnEnter; // ����ϱ� ��ư

    // ��ϵ� �ȳ� �޽��� ����
    public GameObject alarmFactory;

    public GameObject[] bookRewardFactory = new GameObject[3];    // å�忡 ���� å ����

    GameObject book;

    void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;
        //myPastBookListNet = worldManager.GetComponent<WorldManager2D>().myPastBookListNet;

        // �̰� �³�..........
        bestBookContent = GameObject.Find("Canvas").transform.Find("BestBookPanel").Find("Scroll View_BestBook").Find("Viewport").Find("Content_Best");
        book = GameObject.Find("Book");

        inputFieldReview.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string s)
    {
        btnEnter.interactable = s.Length > 0;  // ��� ��ư Ȱ��ȭ
    }

    // ��� ��ư (������ <������ å���>�� �߰�)
    public void OnClickAddPastBook()
    {
        _MyPastBookInfo myPastBookInfo = new _MyPastBookInfo();

        myPastBookInfo.bookName = title.text;
        myPastBookInfo.bookAuthor = author.text;
        myPastBookInfo.bookPublishInfo = publishInfo.text;
        myPastBookInfo.bookISBN = isbn.text;
        myPastBookInfo.thumbnail = thumbnail;
        myPastBookInfo.isDone = true;
        myPastBookInfo.rating = dropdown.captionText.text;
        myPastBookInfo.review = inputFieldReview.text;

        // <������å���> �� �߰�
        myPastBookInfoList.Add(myPastBookInfo);

        ManageBestBook();   // �λ�å ���� 

        //HttpPostPastBookInfo();

        // <������å���>�� ������ �ε��� 
        int idx = myPastBookInfoList.Count - 1;

        GameObject setBook = book.transform.GetChild(idx).gameObject;
        setBook.SetActive(true);
        setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);

/*        // å�忡 �ֱ�
        int idx = Random.Range(0, 3);
        GameObject book = Instantiate(bookRewardFactory[idx]);  // �� ���� ��� �� �ϳ� ����

        book.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);*/

        // <��� �Ǿ����ϴ�>
        GameObject go = Instantiate(alarmFactory, gameObject.transform);    // ���� �ڽ����� ����

    }

    // BestBookPanel �� toggle �߰� ���� -----------
    public Transform bestBookContent;
    public GameObject bestBookFactory;

    public void ManageBestBook()
    {
        // �λ�å�� �־��� ���� ����
        GameObject book = Instantiate(bestBookFactory, bestBookContent);
        GameObject myChild = book.transform.GetChild(1).gameObject;

        myChild.GetComponent<RawImage>().texture = thumbnail.texture;
    }
    // -----------------------------

    // ������ ��ư (������ ������� ����)
    public void OnClickExit()
    {
        // �ۼ��� ����� ���� �ʱ�ȭ... �� �ʿ䰡 ����
        // �� �ڽ� �ʱ�ȭ
        Destroy(gameObject);
    }

    #region �ؽ�Ʈ�� ���� ����

    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetAuthor(string s)
    {
        author.text = s;
    }
    
    public void SetPublishInfo(string s)
    {
        publishInfo.text = s;
    }

    public void SetIsbn(string s)
    {
        isbn.text = s;
    }

    public void SetImage(Texture texture)
    {
        thumbnail.texture = texture;
    }

    #endregion
    
    public void HttpPostPastBookInfo()
    {
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:8080/v1/records/write";
        requester.requestType = RequestType.POST;

        PastBookdata pastBookdata = new PastBookdata();

        pastBookdata.bookName = title.text;
        pastBookdata.bookAuthor = author.text;
        pastBookdata.bookPublishInfo = publishInfo.text;
        pastBookdata.bookISBN = isbn.text;
        pastBookdata.thumbnail = thumbnail;
        pastBookdata.rating = dropdown.captionText.text;
        pastBookdata.bookReview = inputFieldReview.text;

        requester.body = JsonUtility.ToJson(pastBookdata, true);
        requester.onComplete = OnCompletePostMyPastBook;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    public void OnCompletePostMyPastBook(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        //print(jObject + "jobj");
        int type = (int)jObject["status"];
        // UserData user = (UserData)jObject["results"]["data"]["user"];
        // string token = (string)jObject["results"]["data"]["token"];
    }
}
