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
    }
}
