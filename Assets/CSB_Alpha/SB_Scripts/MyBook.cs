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

    // 담은도서 
    // 나를 클릭하면 canvas 에 나의 정보 Panel 생성
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

        go.GetComponent<CurrBookInfoPanel>().SetIndex(idx); // myAllBookList 의 인덱스 값과
        go.GetComponent<CurrBookInfoPanel>().SetIsDone(isDone); // isDone 여부 넘겨줌

    }

    // isDone == true 인 도서
    // 나를 클릭하면 canvas
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

    // 인생책 선정 관련
    // 만약 나의 이름이 PastBook 일 때
    public bool isBest = false;
    public Sprite checkMark;
    public Sprite checkMarkOutline;

    public void OnClickBestBook()
    {
        // bool 값 
        // 만약 bool 값이 false 면 true 로
        if (!isBest)
        {
            isBest = true;
            // 스프라이트 CheckMark 로 변경
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMark;
        }
        else if (isBest)
        {
            isBest = false;
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = checkMarkOutline;

        }

    }

    // 인생책 저장
    public void OnClickSetBestBook()
    {
        // 인생책으로 <등록> 클릭하면 
        // 
    }
}
