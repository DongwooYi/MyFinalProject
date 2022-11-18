using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastBook : MonoBehaviour
{
    public string bookTitle;
    public string bookAuthor;
    public string bookReview;
    public string bookIsbn;
    public string bookInfo;
    public string bookRating;

    public RawImage thumbnail;

    public GameObject pastBookInfoPanelFactory;
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
        GameObject go = Instantiate(pastBookInfoPanelFactory, canvas);
        go.GetComponent<PastBookInfoPanel>().SetTitle(bookTitle);
        go.GetComponent<PastBookInfoPanel>().SetAuthor(bookAuthor);
        go.GetComponent<PastBookInfoPanel>().SetIsbn(bookIsbn);
        go.GetComponent<PastBookInfoPanel>().SetInfo(bookInfo);
        go.GetComponent<PastBookInfoPanel>().SetRating(bookRating);
        go.GetComponent<PastBookInfoPanel>().SetReview(bookReview);
        go.GetComponent<PastBookInfoPanel>().SetThumbnail(thumbnail.texture);
    }
}
