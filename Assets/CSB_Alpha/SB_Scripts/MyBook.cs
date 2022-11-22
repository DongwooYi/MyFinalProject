using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
