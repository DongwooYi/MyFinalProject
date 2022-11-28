using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using TMPro;


// å�� �տ� ���� �� <��������> �� 
// 
public class CurrBookInfoPanel : MonoBehaviour
{
    Transform canvas;
    public GameObject bookFactory;  // �������� ��� ����
    public GameObject headConfirm; // <��ǥå�� �����Ǿ����ϴ�> �ȳ�
    public GameObject doneBookConfirm;  // <���� ���翡 �����ϴ�> �����Ǿ����ϴ�> �ȳ�

    GameObject worldManager;
    GameObject myBookManager;

    public Transform myBookPanel;
    public Transform contentBook;
    public Transform contentDoneBook;


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
    public Toggle checkIsDone;  // 

    public int idx;

    public Dropdown dropdown;

    //public TMP_InputField inputFieldReview; // ���� �Է� ĭ
    public InputField review;
    public Button btnEnter; // ����ϱ� ��ư

    public GameObject player;   // �÷��̾�
    public GameObject showBook;

    public _MyBookInfo[] myAllBookListToArray;

    GameObject book;


    public Toggle headBook; // �Ӹ�å(��ǥ) 
    public string isOverHeadString;
    public bool isOverHead;

    WorldManager2D wm;

    /*    public void ToggleHead(Toggle headBook)
        {
            print("���");
            if (headBook.isOn)
            {
                print("���" + headBook.isOn);
                showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
                //player.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            }
        }*/

    HttpManager httpManager;
    void Start()
    {
       
        myBookPanel = GameObject.Find("MyBookPanel").transform;
        contentBook = GameObject.Find("Scroll View_Book/Viewport/Content").transform;
        contentDoneBook = GameObject.Find("Scroll View_Done/Viewport/Content").transform;

        canvas = GameObject.Find("Canvas").transform;

        player = GameObject.Find("Character");
        showBook = GameObject.Find("ShowBook");

        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();

        myAllBookListToArray = wm.myAllBookListNet.ToArray();
        book = GameObject.Find("Book");

        review.onValueChanged.AddListener(OnValueChanged);
        checkIsDone.onValueChanged.AddListener(OnisDoneToggleClicked);
        headBook.onValueChanged.AddListener(OnOverHeadToggle);
    }

    void OnValueChanged(string s)
    {
       btnEnter.interactable = s.Length >= 0;  // ��� ��ư Ȱ��ȭ
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
            myAllBookListToArray[idx].review = review.text;
            myAllBookListToArray[idx].isOverHead = isOverHead;
            myAllBookListToArray[idx].isOverHeadString = isOverHeadString;

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
            myAllBookListToArray[idx].review = review.text;
            myAllBookListToArray[idx].isOverHead = isOverHead;
            myAllBookListToArray[idx].isOverHeadString = isOverHeadString;

            // POST �� ������
            HttpPostMyBookData();
        }


        // <��� �Ǿ����ϴ�>
        confirmMsg = Instantiate(doneBookConfirm, gameObject.transform);    // ���� �ڽ����� ����
       // myBookManager.SetActive(false);
    }

    GameObject confirmMsg;

    // ������ ��ư (������ ������� ����)
    public void OnClickExit()
    {
        Destroy(gameObject);
       // HttpGetMyBookData();

    }
    /*
        // <���� ���翡 �������ϴ�> �ݱ� ��ư
        public void OnClickConfirmMyBook()
        {
            Destroy(gameObject);

        }*/

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
        //rating.text = s;
        RateApplication(int.Parse(s));
    }

    public void SetReview(string s)
    {
        review.text = s;
    }

    public void SetIndex(int num)
    {
        idx = num;
    }
    
    public void SetOverHeadBook(bool overHead)
    {
        isOverHead = overHead;
        // ��ۿ� ǥ��
        headBook.isOn = overHead;
        print("��ǥå : " + isOverHead + "��� : " + headBook.isOn);
    }

    GameObject headBookInfo;
    public void OnOverHeadToggle(bool isOverHead)
    {
        print("��� ������: " + isOverHead);
        if(isOverHead)
        {
            isOverHeadString = "Y";
            headBookInfo = Instantiate(headConfirm, canvas);
            // �ٸ� å�� isOverHead = false & isOverHeadString = "N" �� 
            if (isDone) // ���� ���� �������������
            {
                for (int i = 0; i < contentBook.childCount; i++)    // ���������� ��� N ����
                {
                    contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                    contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                }
                for (int i = 0; i < contentDoneBook.childCount; i++)    // ������������ �� ���� N ����
                {
                    if (i == idx)
                    {
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = true;
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "Y";
                    }
                    else
                    {
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                    }

                }
            }
            else if (!isDone)   // ���� ����������� 
            {
                for (int i = 0; i < contentBook.childCount; i++)
                {
                    if (i == idx)
                    {
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = true;
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "Y";
                    }
                    else
                    {
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                    }

                }
                for (int i = 0; i < contentDoneBook.childCount; i++)
                {
                    contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                    contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                }
            }
            showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
        }
        else
        {
            isOverHeadString = "N";
        }
    }

    // ��ǥå �����Ϸ� �ȳ� ����, �ݱ� ��ư ������ ��ǥå ��� �Ϸ�
    public void OnClickHeadBookConfirmBook()
    {
        Destroy(headBookInfo);
    }

    public void SetIsDone(bool done)
    {
        isDone = done;
        // ��ۿ� üũ ǥ��
        checkIsDone.isOn = done;
        print("üũ : " + checkIsDone.isOn);
    }
    public void OnisDoneToggleClicked(bool isDone)
    {
        print(isDone);
        print("�� �ȹٲ�");
    }
    #endregion


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
            bookReview = review.text,
            isDone = "Y",
            isOverHead = isOverHeadString,
        };

        requester.body = JsonUtility.ToJson(bookData, true);
        requester.onComplete = OnCompletePostMyBookData;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void HttpPostMyBookData()
    {
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
            bookReview = review.text,
            isOverHead = isOverHeadString,
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
            HttpGetMyBookData();
        }
    }

    // 1. ���� ����� ��û�� API : ���� å(å��), �λ�å (���� å��) ���� �����ֱ�
    public void HttpGetMyBookData()
    {            
        // �� data ����Ʈ�� �ʱ�ȭ
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
        myBookManager.GetComponent<MyBookManager>().MakePrefab();
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
