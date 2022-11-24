using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class MyBook : MonoBehaviour
{
    public string bookTitle;
    public string bookAuthor;
    public string bookReview;
    public string bookIsbn;
    public string bookInfo;
    public string bookRating;
    public string isBestStr;

    public bool isDone;
    public bool isBest;

    public int idx;

    public RawImage thumbnail;

    public GameObject bookInfoPanelFactory;
    public GameObject doneBookInfoPanelFactory;
    Transform canvas;

    Transform contentDoneBook;

    public Button btnBestBook;
    void Start()
    {
        contentDoneBook = GameObject.Find("MyPastBookPanel/Scroll View_Done/Viewport/Content").transform;
        print(contentDoneBook.name);
        btnBestBook = GameObject.Find("MyPastBookPanel").transform.GetChild(3).gameObject.GetComponent<Button>();
        btnBestBook.onClick.AddListener(OnClickBestBook);
        canvas = GameObject.Find("Canvas").transform;
        if(isBestStr == "Y")
        {
            isBest = true;
        }
        else
        {
            isBest = false;
        }
    }

    void Update()
    {
        
    }

    // �������� 
    // ���� Ŭ���ϸ� canvas �� ���� ���� Panel ����
    public void OnClickBookInfo()
    {
        GameObject go = Instantiate(bookInfoPanelFactory, canvas);

        go.GetComponent<CurrBookInfoPanel>().SetTitle(bookTitle);
        go.GetComponent<CurrBookInfoPanel>().SetAuthor(bookAuthor);
        go.GetComponent<CurrBookInfoPanel>().SetIsbn(bookIsbn);
        go.GetComponent<CurrBookInfoPanel>().SetPublishInfo(bookInfo);
        go.GetComponent<CurrBookInfoPanel>().SetRating(bookRating);
        go.GetComponent<CurrBookInfoPanel>().SetReview(bookReview);
        go.GetComponent<CurrBookInfoPanel>().SetThumbnail(thumbnail.texture);

        go.GetComponent<CurrBookInfoPanel>().SetIndex(idx); // myAllBookList �� �ε��� ����
        go.GetComponent<CurrBookInfoPanel>().SetIsDone(isDone); // isDone ���� �Ѱ���

    }

    // ----------------------------------------------------------------------
    // isDone == true �� ���� -> isDone == true �� toggle �� isOn ���·�
    // ���� Ŭ���ϸ� canvas
    public void OnClickDoneBookInfo()
    {
        GameObject go = Instantiate(doneBookInfoPanelFactory, canvas);

        go.GetComponent<PastBookInfoPanel>().SetTitle(bookTitle);
        go.GetComponent<PastBookInfoPanel>().SetAuthor(bookAuthor);
        go.GetComponent<PastBookInfoPanel>().SetIsbn(bookIsbn);
        go.GetComponent<PastBookInfoPanel>().SetInfo(bookInfo);
        go.GetComponent<PastBookInfoPanel>().SetRating(bookRating);
        go.GetComponent<PastBookInfoPanel>().SetReview(bookReview);
        go.GetComponent<PastBookInfoPanel>().SetThumbnail(thumbnail.texture);
        go.GetComponent<PastBookInfoPanel>().SetBestBook(isBest);
    }

    // �λ�å ���� ����
    // ���� ���� �̸��� PastBook �� ��

    public Sprite checkMark;
    public Sprite checkMarkOutline;

    bool temp;
    public void OnClickBestBook()
    {
        temp = isBest;
        // bool �� 
        // ���� bool ���� false �� true ��
        if (!isBest)
        {
            isBest = true;
            isBestStr = "Y";
            // ��������Ʈ CheckMark �� ����
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMark;
        }
        else if (isBest)
        {
            isBest = false;
            isBestStr = "N";
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMarkOutline;

        }

    }

    public List<BestBook> bestBookList = new List<BestBook>();
    // �λ�å ����(<�λ�å ���>��ư Ŭ��)
    public void OnClickSetBestBook()
    {
        print("����?");
        // content�� �ڽ� �� temp �� ���� �޶��� �ֵ� ����
        for (int i = 0; i < contentDoneBook.childCount; i++)
        {
            if(temp != isBest)
            {
                BestBook bestBook = new BestBook();
                bestBook.bookISBN = bookIsbn;
                bestBook.isBest = isBestStr;

                bestBookList.Add(bestBook);
            }
        }
        HttpPostMyBestBook();
    }

    // (�ٲ� ����) Http ��� ���� ----------------------------------
    // 5. å�� -> Post��û (üũ�� ��ȭ�� �ִ� �͵��� ���� ������ �� �� �����ϴ�.)
    // ���) ȣ�� �Ϸ� �� 1�� ȣ�� �ٽ� ����� ��.
    void HttpPostMyBestBook()
    {
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:80/v1/records/best";
        requester.requestType = RequestType.POST;

        BestBookData bookData = new()
        {
            recordDTOList = bestBookList,
        };
        print(bookData);
        requester.body = JsonUtility.ToJson(bookData, true);
        requester.onComplete = OnCompletePostMyBestBook;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void OnCompletePostMyBestBook(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("�λ�å ��� �ǳ�?");
        }
    }

}
