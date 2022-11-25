using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 생성될 때 MyBestBook.cs 의 toggles 
// 나의 인덱스를 찾아서 넘겨줌
public class PastBookforBest : MonoBehaviour
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


 //   public Button btnBestBook;
    public Transform bestBookContent;

    MyBestBook bestBook;

    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;

        bestBookContent = GameObject.Find("Canvas").transform.Find("BestBookPanel").Find("Scroll View_BestBook").Find("Viewport").Find("Content_Best");
        bestBook = GameObject.Find("myroom/MyBestBookshelf").GetComponent<MyBestBook>();
        //bestBook.toggleList.Add(false);
    }

    // ----------------------------------------------------------------------
    // isDone == true 인 도서 -> isDone == true 면 toggle 을 isOn 상태로
    // 나를 클릭하면 canvas
    public void OnClickDoneBookInfo()
    {
        print("????");
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
    void Update()
    {
        
    }
}
