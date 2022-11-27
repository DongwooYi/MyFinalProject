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

    public bool isDone; // �ϵ� ����
    public bool isBest;
    public bool isBestTemp; // �λ�å ���� ���� �� ����
    public bool isOverHead;   // ��ǥå
    public string isOverHeadString;

    public int idx; // ���� �ε��� (�θ� ����)
    public int myIndex;

    public RawImage thumbnail;

    public GameObject bookInfoPanelFactory;
    public GameObject doneBookInfoPanelFactory;
    Transform canvas;

    Transform contentDoneBook;

    Transform myPastBookPanel;
    Transform scroll;


    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        myPastBookPanel = canvas.Find("MyPastBookPanel").transform;
        scroll = myPastBookPanel.Find("Scroll View_Done").transform;
        contentDoneBook = scroll.Find("Viewport/Content").transform;

/*        if (gameObject.name.Contains("PastBook"))
        {
            if (isBestStr == "Y")
            {
                isBest = true;
                transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMark;
            }
            else
            {
                isBest = false;
                transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMarkOutline;
            }
        }*/
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
        //go.GetComponent<CurrBookInfoPanel>().SetIsbn(bookIsbn);
        go.GetComponent<CurrBookInfoPanel>().SetPublishInfo(bookInfo);
       // go.GetComponent<CurrBookInfoPanel>().SetRating(bookRating);
        go.GetComponent<CurrBookInfoPanel>().SetReview(bookReview);
        go.GetComponent<CurrBookInfoPanel>().SetThumbnail(thumbnail.texture);

        go.GetComponent<CurrBookInfoPanel>().SetIndex(idx); // myAllBookList �� �ε��� ����
        go.GetComponent<CurrBookInfoPanel>().SetIsDone(isDone); // isDone ���� �Ѱ���
        go.GetComponent<CurrBookInfoPanel>().SetOverHeadBook(isOverHead);
    }

    // ----------------------------------------------------------------------
    // isDone == true �� ���� -> isDone == true �� toggle �� isOn ���·�
    // ���� Ŭ���ϸ� canvas
    public void OnClickDoneBookInfo()
    {
        // ���� �ε��� ã��
        int idxDone = contentDoneBook.GetSiblingIndex();
        print("�������� �ε���: " + idx);

        GameObject go = Instantiate(doneBookInfoPanelFactory, canvas);

        // ���� �ε��� �ѱ��
        go.GetComponent<PastBookInfoPanel>().SetMyIndex(idxDone);

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
/*
    public Sprite checkMark;
    public Sprite checkMarkOutline;

    bool temp;
    public void OnClickBestBook()
    {
        print(temp);
        temp = isBest;
        print(temp);
        // bool �� 
        // ���� bool ���� false �� true ��
        if (!isBest)
        {
            print(temp);
            isBest = true;
            isBestStr = "Y";
            // ��������Ʈ CheckMark �� ����
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMark;
        }
        else if (isBest)
        {
            print(temp);
            isBest = false;
            isBestStr = "N";
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMarkOutline;

        }

    }*/

    public List<BestBook> bestBookList = new List<BestBook>();
    // �λ�å ����(<�λ�å ���>��ư Ŭ��)
/*    public void OnClickSetBestBook()
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
                print(bestBook.bookISBN + bestBook.isBest);
                bestBookList.Add(bestBook);
            }
        }
        //HttpPostMyBestBook();
    }*/



}
