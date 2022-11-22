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
    public bool isDone;

    public int idx;

    public RawImage thumbnail;

    public GameObject bookInfoPanelFactory;
    public GameObject doneBookInfoPanelFactory;
    Transform canvas;
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
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
            //go.GetComponent<CurrBookInfoPanel>().SetRating(bookRating);
            go.GetComponent<CurrBookInfoPanel>().SetReview(bookReview);
            go.GetComponent<CurrBookInfoPanel>().SetThumbnail(thumbnail.texture);

        go.GetComponent<CurrBookInfoPanel>().SetIndex(idx); // myAllBookList �� �ε��� ����
        go.GetComponent<CurrBookInfoPanel>().SetIsDone(isDone); // isDone ���� �Ѱ���

    }

    // isDone == true �� ����
    // ���� Ŭ���ϸ� canvas
    public void OnClickDoneBookInfo()
    {
        GameObject go = Instantiate(doneBookInfoPanelFactory, canvas);
        go.GetComponent<PastBookInfoPanel>().SetTitle(bookTitle);
        go.GetComponent<PastBookInfoPanel>().SetAuthor(bookAuthor);
        go.GetComponent<PastBookInfoPanel>().SetIsbn(bookIsbn);
        go.GetComponent<PastBookInfoPanel>().SetInfo(bookInfo);
        go.GetComponent<PastBookInfoPanel>().SetRating(bookReview);
        go.GetComponent<PastBookInfoPanel>().SetReview(bookReview);
        go.GetComponent<PastBookInfoPanel>().SetThumbnail(thumbnail.texture);
        //go.GetComponent<PastBookInfoPanel>().SetBestBook(isBest);
    }

    // �λ�å ���� ����
    // ���� ���� �̸��� PastBook �� ��
    public bool isBest = false;
    public Sprite checkMark;
    public Sprite checkMarkOutline;

    public void OnClickBestBook()
    {
        // bool �� 
        // ���� bool ���� false �� true ��
        if (!isBest)
        {
            isBest = true;
            // ��������Ʈ CheckMark �� ����
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMark;
        }
        else if (isBest)
        {
            isBest = false;
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMarkOutline;

        }

    }

    // �λ�å ����
    public void OnClickSetBestBook()
    {
        // �λ�å���� <���> Ŭ���ϸ� 
        // 
    }

    // (�ٲ� ����) Http ��� ���� ----------------------------------
    // 5. å�� -> Post��û (üũ�� ��ȭ�� �ִ� �͵��� ���� ������ �� �� �����ϴ�.)
    // ���) ȣ�� �Ϸ� �� 1�� ȣ�� �ٽ� ����� ��.
    void HttpPostMyBestBook()
    {
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:8080/v1/records/best";
        requester.requestType = RequestType.POST;

        PastBookdata pastBookdata = new PastBookdata();

/*        pastBookdata.bookName = title.text;
        pastBookdata.bookAuthor = author.text;
        pastBookdata.bookPublishInfo = publishInfo.text;
        pastBookdata.bookISBN = isbn.text;
        pastBookdata.thumbnail = thumbnail;
        pastBookdata.rating = dropdown.captionText.text;
        pastBookdata.bookReview = inputFieldReview.text;*/

        requester.body = JsonUtility.ToJson(pastBookdata, true);
        requester.onComplete = OnCompletePostMyBestBook;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void OnCompletePostMyBestBook(DownloadHandler handler)
    {

    }

}
