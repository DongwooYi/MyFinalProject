using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using TMPro;


// å�� �տ� ���� �� �������� �� 
public class CurrBookInfoPanel : MonoBehaviour
{
    public GameObject bookFactory;  // �������� ��� ����

    GameObject worldManager;

    public string rateNumber;

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public Text rating;
    //public InputField review;
    public TMP_InputField reviewTMP;
    public RawImage thumbnail;
    public Texture texture;

    bool isDone;
    string isDoneString;
    public int idx;

    public Dropdown dropdown;

    public TMP_InputField inputFieldReview; // ���� �Է� ĭ
    public Button btnEnter; // ����ϱ� ��ư

    public GameObject player;   // �÷��̾�
    public GameObject showBook;

    public _MyBookInfo[] myAllBookListToArray;

    GameObject book;

    // ��ϵ� �ȳ� �޽��� ����
    public GameObject alarmFactory;

    public Toggle headBook;
    public Toggle checkIsDone;

    MyBookManager bookManager;
    WorldManager2D wm;

    GameObject myCurrBookPanel;

    public void ToggleHead(Toggle headBook)
    {
        print("���");
        if (headBook.isOn)
        {
            print("���" + headBook.isOn);
            showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            //player.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
        }
    }

    void Start()
    {
        player = GameObject.Find("Character");
        showBook = GameObject.Find("ShowBook");

        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();

        myAllBookListToArray = wm.myAllBookListNet.ToArray();
        book = GameObject.Find("Book");

        myCurrBookPanel = GameObject.Find("MyCurrBookPanel");
        bookManager = GameObject.Find("MyBookManager").GetComponent<MyBookManager>();

        inputFieldReview.onValueChanged.AddListener(OnValueChanged);
        checkIsDone.onValueChanged.AddListener(OnisDoneToggleClicked);
    }

    void OnValueChanged(string s)
    {
        btnEnter.interactable = s.Length > 0;  // ��� ��ư Ȱ��ȭ
    }


    // ��� ��ư
    // isDone == true �� WorldManager �� myDoneBookList ��
    // isDone == false �� review ����
    public void OnClickEnter()
    {
        if (checkIsDone.isOn) isDone = true;
        else if (!checkIsDone.isOn) isDone = false;

        print(isDone);

        if (isDone)
        {
            isDoneString = "Y";
            // 0. WorldManager �� myAllBookList ������Ʈ
            // isDone = true ����
            myAllBookListToArray[idx].bookName = title.text;
            myAllBookListToArray[idx].bookAuthor = author.text;
            myAllBookListToArray[idx].bookISBN = isbn.text;
            myAllBookListToArray[idx].bookPublishInfo = publishInfo.text;
            myAllBookListToArray[idx].isDone = true;
            myAllBookListToArray[idx].isDoneString = "Y";
            myAllBookListToArray[idx].rating = rateNumber;
            myAllBookListToArray[idx].review = inputFieldReview.text;

            // POSt �� ������
            HttpPostMyBookDataD();
        }
        else
        {
            isDoneString = "N";
            myAllBookListToArray[idx].bookName = title.text;
            myAllBookListToArray[idx].bookAuthor = author.text;
            myAllBookListToArray[idx].bookISBN = isbn.text;
            myAllBookListToArray[idx].bookPublishInfo = publishInfo.text;
            myAllBookListToArray[idx].isDone = false;
            myAllBookListToArray[idx].isDoneString = "N";
            myAllBookListToArray[idx].rating = rateNumber;
            myAllBookListToArray[idx].review = inputFieldReview.text;

            // POST �� ������
            HttpPostMyBookData();
        }


        // <��� �Ǿ����ϴ�>
        GameObject go = Instantiate(alarmFactory, gameObject.transform);    // ���� �ڽ����� ����
        //bookManager.ShowAllBookList();
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

    public void SetThumbnail(Texture texture)
    {
        thumbnail.texture = texture;
    }

    public void SetRating(string s)
    {
        rating.text = s;
    }

    public void SetReview(string s)
    {
        reviewTMP.text = s;
    }

    public void SetIndex(int num)
    {
        idx = num;
    }

    public void SetIsDone(bool done)
    {
        isDone = done;
        // ��ۿ� üũ ǥ��
        checkIsDone.isOn = done;
        print("check : " + checkIsDone.isOn);
    }

    #endregion
    public void OnisDoneToggleClicked(bool isDone)
    {
        print(isDone);
    }

    // (�ٲ� ����) Http ��� ���� ---------------------------------------------
    // 4. ���� ��� ����
    // ���) �̹� ��� �������� �̹��� ������ ���ε� �߱� ������, �̹��� ���� �����ϰ� �����ּ���
    // ���) ȣ�� �� 1�� �ٽ� ȣ������� ��
    void HttpPostMyBookDataD()
    {
        print("000");
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:80/v1/records/write";
        requester.requestType = RequestType.POST;

        BookData bookData = new()
        {
            bookName = title.text,
            bookAuthor = author.text,
            bookPublishInfo = publishInfo.text,
            bookISBN = isbn.text,
            rating = rateNumber,
            bookReview = inputFieldReview.text,
            isDone = "Y",
        };

        requester.body = JsonUtility.ToJson(bookData, true);
        requester.onComplete = OnCompletePostMyBookData;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void HttpPostMyBookData()
    {
        print("111");
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:80/v1/records/write";
        requester.requestType = RequestType.POST;

        BookData bookData = new()
        {
            bookName = title.text,
            bookAuthor = author.text,
            bookPublishInfo = publishInfo.text,
            bookISBN = isbn.text,
            rating = rateNumber,
            bookReview = inputFieldReview.text,
        };

        requester.body = JsonUtility.ToJson(bookData, true);
        print(requester.body);
        requester.onComplete = OnCompletePostMyBookData;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void OnCompletePostMyBookData(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("�ǳ�?");
            wm.myAllBookListNet.Clear();
        }
    }

    // 1. ���� ����� ��û�� API : ���� å(å��), �λ�å (���� å��) ���� �����ֱ�
    public void HttpGetMyBookData()
    {            // �� data ����Ʈ�� �ʱ�ȭ
        wm.titleListNet.Clear();
        wm.authorListNet.Clear();
        wm.publishInfoListNet.Clear();
        wm.thumbnailLinkListNet.Clear();
        wm.thumbnailImgListNet.Clear();
        wm.isbnListNet.Clear();
        wm.ratingListNet.Clear();
        wm.reviewListNet.Clear();
        wm.isDoneListNet.Clear();
        wm.isBestsListNet.Clear();

        wm.myAllBookListNet.Clear();
        print("��û11");
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

        if (type == 200)
        {
            print("��ż���. ��絵��.��������");
            string result_data = ParseGETJson("[" + handler.text + "]", "data");

            wm.titleListNet = ParseMyBookData(result_data, "bookName");
            wm.authorListNet = ParseMyBookData(result_data, "bookAuthor");
            wm.publishInfoListNet = ParseMyBookData(result_data, "bookPublishInfo");
            wm.thumbnailLinkListNet = ParseMyBookData(result_data, "thumbnailLink");
            wm.isbnListNet = ParseMyBookData(result_data, "bookISBN");
            wm.ratingListNet = ParseMyBookData(result_data, "rating");
            wm.reviewListNet = ParseMyBookData(result_data, "bookReview");
            wm.isDoneListNet = ParseMyBookData(result_data, "isDone");
            wm.isBestsListNet = ParseMyBookData(result_data, "isBest");

            GETThumbnailTexture();
        }
    }

    public void GETThumbnailTexture()
    {
        StartCoroutine(GetThumbnailImg(wm.thumbnailLinkListNet.ToArray()));
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
                wm.thumbnailImgListNet.Add(myTexture);
            }
            yield return null;
        }
        for (int i = 0; i < wm.titleListNet.Count; i++)
        {
            _MyBookInfo myBookInfo = new _MyBookInfo();

            myBookInfo.bookName = wm.titleListNet[i];
            myBookInfo.bookAuthor = wm.authorListNet[i];
            myBookInfo.bookPublishInfo = wm.publishInfoListNet[i];
            myBookInfo.thumbnailLink = wm.thumbnailLinkListNet[i];
            myBookInfo.bookISBN = wm.isbnListNet[i];
            myBookInfo.rating = wm.ratingListNet[i];
            myBookInfo.review = wm.reviewListNet[i];
            myBookInfo.isDoneString = wm.isDoneListNet[i];
            myBookInfo.isBestString = wm.isBestsListNet[i];
            //myBookInfo.thumbnail = rawImages[i];
            myBookInfo.texture = wm.thumbnailImgListNet[i];
            wm.myAllBookListNet.Add(myBookInfo);
        }
        wm.SettingMyRoom();
    }
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

    [Header("���� ��ư")]
    public Button[] starButton;
    public Button acceptButton;
    [HideInInspector] public int ratedApp;
    public void RateApplication(int rate)
    {
        ratedApp = rate;

        // active rate button if use click some stars
        if (rate > 0)
            acceptButton.GetComponent<Button>().interactable = true;

        // enable stars equal than user rated
        for (int i = 0; i < rate; i++)
        {
            foreach (Transform t in starButton[i].transform)
            {
                t.gameObject.SetActive(true);
            }
        }

        // enable stars greater than user rated
        for (int i = rate; i < starButton.Length; i++)
        {
            foreach (Transform t in starButton[i].transform)
            {
                t.gameObject.SetActive(false);
            }

        }
        rateNumber = rate.ToString();
    }

}
