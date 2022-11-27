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

    public bool isDone; // 완독 여부
    public bool isBest;
    public bool isBestTemp; // 인생책 여부 이전 값 저장
    public bool isOverHead;   // 대표책
    public string isOverHeadString;

    public int idx; // 나의 인덱스 (부모 기준)
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

    // 담은도서 
    // 나를 클릭하면 canvas 에 나의 정보 Panel 생성
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

        go.GetComponent<CurrBookInfoPanel>().SetIndex(idx); // myAllBookList 의 인덱스 값과
        go.GetComponent<CurrBookInfoPanel>().SetIsDone(isDone); // isDone 여부 넘겨줌
        go.GetComponent<CurrBookInfoPanel>().SetOverHeadBook(isOverHead);
    }

    // ----------------------------------------------------------------------
    // isDone == true 인 도서 -> isDone == true 면 toggle 을 isOn 상태로
    // 나를 클릭하면 canvas
    public void OnClickDoneBookInfo()
    {
        // 나의 인덱스 찾기
        int idxDone = contentDoneBook.GetSiblingIndex();
        print("담은도서 인덱스: " + idx);

        GameObject go = Instantiate(doneBookInfoPanelFactory, canvas);

        // 나의 인덱스 넘기기
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

    // 인생책 선정 관련
    // 만약 나의 이름이 PastBook 일 때
/*
    public Sprite checkMark;
    public Sprite checkMarkOutline;

    bool temp;
    public void OnClickBestBook()
    {
        print(temp);
        temp = isBest;
        print(temp);
        // bool 값 
        // 만약 bool 값이 false 면 true 로
        if (!isBest)
        {
            print(temp);
            isBest = true;
            isBestStr = "Y";
            // 스프라이트 CheckMark 로 변경
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
    // 인생책 저장(<인생책 등록>버튼 클릭)
/*    public void OnClickSetBestBook()
    {
        print("들어와?");
        // content의 자식 중 temp 와 값이 달라진 애들 전송
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
