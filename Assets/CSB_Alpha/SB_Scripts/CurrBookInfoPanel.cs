using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class CurrBookInfoPanel : MonoBehaviour
{
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();
    //List<_MyPastBookInfo> myPastBookListNet = new List<_MyPastBookInfo>();

    List<_MyBookInfo> myBookInfoList = new List<_MyBookInfo>();
    //List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;

    public Dropdown dropdown;

    public InputField inputFieldReview; // ���� �Է� ĭ
    public Button btnEnter; // ����ϱ� ��ư

    public GameObject player;   // �÷��̾�
    public GameObject bookQuad0;   // �Ӹ� �� å ����
    public GameObject bookQuad1;   // �Ӹ� �� å ����

    GameObject book;

    // ��ϵ� �ȳ� �޽��� ����
    public GameObject alarmFactory;

    public Toggle headBook;

    MyBookManager bookManager;
    WorldManager2D wm;

    GameObject myCurrBookPanel;

    public void ToggleHead(Toggle headBook)
    {
        print("���");
        if (headBook.isOn)
        {
            print("���" + headBook.isOn);
            bookQuad0.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            bookQuad1.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            player.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
        }
    }

    void Start()
    {
        player = GameObject.Find("Character");

        bookQuad0 = GameObject.Find("BookQuad");
        bookQuad1 = GameObject.Find("BookQuad(1)");


        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();
        myPastBookInfoList = wm.myPastBookList;
        //myPastBookListNet = worldManager.GetComponent<WorldManager2D>().myPastBookList;



        myBookInfoList = worldManager.GetComponent<WorldManager2D>().myBookList;
        //myBookListNet = worldManager.GetComponent<WorldManager2D>().myBookList;

        book = GameObject.Find("Book");

        myCurrBookPanel = GameObject.Find("MyCurrBookPanel");
        bookManager = GameObject.Find("MyBookManager").GetComponent<MyBookManager>();

        inputFieldReview.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string s)
    {
        btnEnter.interactable = s.Length > 0;  // ��� ��ư Ȱ��ȭ
    }

    // ��� ��ư (������ <������ å���>�� �߰�)
    public void OnClickAddPastBook()
    {
        wm.bookPastCount++;

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
        //myPastBookListNet.Add(myPastBookInfo);

        // texture �� ����
        for (int i = 0; i < 6; i++)
        {
            myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = null;
        }

        // ������Ʈ �� <���� ���� ���> ���� �޾ƿͼ� �Ѹ���
        int destroyBookIdx = bookManager.idx;
        myBookInfoList.RemoveAt(destroyBookIdx);
        //myBookListNet.RemoveAt(destroyBookIdx);

        // MyCurrBookPanel �� �ڽ��� �ε����� myCurrBookList �� �ε��� ���缭 �־���
        for (int i = 0; i < myBookInfoList.Count; i++)
        {
            myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myBookInfoList[i].thumbnail.texture;
        }


        //HttpPostPastBookInfo();

        // å�忡 �ֱ�
        // <������å���>�� ������ �ε��� 
        int idx = myPastBookInfoList.Count - 1;
        //int idx = myPastBookListNet.Count - 1;

        GameObject setBook = book.transform.GetChild(idx).gameObject;
        setBook.SetActive(true);
        setBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);

        // <��� �Ǿ����ϴ�>
        GameObject go = Instantiate(alarmFactory, gameObject.transform);    // ���� �ڽ����� ����

    }

    // ������ ��ư (������ ������� ����)
    public void OnClickExit()
    {
        // �ۼ��� ����� ���� �ʱ�ȭ... �� �ʿ䰡 ����
        // �� �ڽ� �ʱ�ȭ
        Destroy(gameObject);
    }

    #region �ؽ�Ʈ ���� ����
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
