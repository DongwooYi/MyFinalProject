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
    public GameObject modifyBookInfoPanelFactory;
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
        // 만약 bookReview 가 null 이면
        //if (bookReview == null)
        //{
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

        //}
        // null 이 아니면
/*        else
        {
            GameObject go = Instantiate(modifyBookInfoPanelFactory, canvas);
            
        }*/
    }
}
