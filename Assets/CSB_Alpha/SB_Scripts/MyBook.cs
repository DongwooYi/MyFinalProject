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

    // 담은도서 
    // 나를 클릭하면 canvas 에 나의 정보 Panel 생성
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

        go.GetComponent<CurrBookInfoPanel>().SetIndex(idx); // myAllBookList 의 인덱스 값과
        go.GetComponent<CurrBookInfoPanel>().SetIsDone(isDone); // isDone 여부 넘겨줌

    }

    // ----------------------------------------------------------------------
    // isDone == true 인 도서 -> isDone == true 면 toggle 을 isOn 상태로
    // 나를 클릭하면 canvas
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

    // 인생책 선정 관련
    // 만약 나의 이름이 PastBook 일 때

    public Sprite checkMark;
    public Sprite checkMarkOutline;

    bool temp;
    public void OnClickBestBook()
    {
        temp = isBest;
        // bool 값 
        // 만약 bool 값이 false 면 true 로
        if (!isBest)
        {
            isBest = true;
            isBestStr = "Y";
            // 스프라이트 CheckMark 로 변경
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
    // 인생책 저장(<인생책 등록>버튼 클릭)
    public void OnClickSetBestBook()
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

                bestBookList.Add(bestBook);
            }
        }
        HttpPostMyBestBook();
    }

    // (바뀐 버전) Http 통신 관련 ----------------------------------
    // 5. 책장 -> Post요청 (체크에 변화가 있는 것들의 값을 보내면 될 것 같습니다.)
    // 비고) 호출 완료 후 1번 호출 다시 해줘야 함.
    void HttpPostMyBestBook()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
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

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void OnCompletePostMyBestBook(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("인생책 통신 되나?");
        }
    }

}
