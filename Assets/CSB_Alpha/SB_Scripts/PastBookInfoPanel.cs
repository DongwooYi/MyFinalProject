using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;

public class PastBookInfoPanel : MonoBehaviour
{
    public int myIndex;

    public RawImage thumbnail;
    public Texture texture;

    public Text bookTitle;
    public TMP_Text title;

    public Text bookAuthor;
    public Text bookIsbn;
    public Text bookInfo;
    public Text bookRating;
    public Text bookReview;

    public Toggle isBestToggle;
    public bool isBest; // 내가 인생책인가
    bool temp;

    public Transform myPastBookPanel;
    public Transform contentDoneBook;

    public GameObject myBookManager;

    private void Start()
    {
        myBookManager = GameObject.Find("MyBookManager");
        myPastBookPanel = GameObject.Find("MyPastBookPanel").transform;///Scroll View_Done/Viewport/Content").transform;
        contentDoneBook = GameObject.Find("Scroll View_Done/Viewport/Content").transform;//")//.transform;///MyPastBookPanel/Scroll View_Done/Viewport/Content").transform;
        isBestToggle.isOn = isBest;
        isBestToggle.onValueChanged.AddListener(OnCheck);

    }

    public bool tempMyBook;
    public bool isBestMyBook;
    public void OnCheck(bool checkBool)
    {
        temp = isBest;  // 이전 값 저장
        isBest = checkBool; // 변한 값
        // 받은 인덱스에 해당하는 책의 isBest 바꾸고
        tempMyBook = contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBestTemp;
        isBestMyBook = contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBest;
        tempMyBook = temp;
        isBestMyBook = isBest;
    }

    public List<GameObject> BestBookList = new List<GameObject>();
    public List<BestBook> httpList = new List<BestBook>();
    // 패널의 <확인> 버튼
    public void OnClickSetBook()
    {
        // 만약

        for (int i = 0; i < contentDoneBook.childCount; i++)
        {
            // isBest == true 인 책은 리스트에 추가
            if (contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isBest)
            {
                if (BestBookList.Count > 3)
                {
                    // 가장 앞 도서 삭제
                    BestBookList.RemoveAt(0);
                }
                // 리스트에 추가
                BestBookList.Add(contentDoneBook.GetChild(i).gameObject);
                // 인생책으로 등록되었습니다 UI 띄우기
                // 
            }
        }


        // 만약 temp 와 isBest 가 다르다면 전송할 리스트에 담기
        if (temp = !isBest)
        {
            string isBestStr;
            BestBook bestBook = new BestBook();
            bestBook.bookISBN = bookIsbn.text;
            if (isBest) isBestStr = "Y";
            else isBestStr = "N";
            bestBook.isBest = isBestStr;

            httpList.Add(bestBook);
            print(bestBook);
            print(bestBook.bookISBN + bestBook.isBest);
        }
        HttpPostMyBestBook();
        // MyBookManager.cs 의 bestBookList 에 값 담아줌
        myBookManager.GetComponent<MyBookManager>().bestBookList = BestBookList;
        Destroy(gameObject);
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
            recordDTOList = httpList,
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
            print("인생책 통신 됨?");
        }
    }
    public void OnClickExit()
    {
        Destroy(gameObject);
    }

    public void OnClickConfirmBestBook()
    {
        Destroy(gameObject);
    }

    #region 텍스트 세팅 관련

    public void SetMyIndex(int num)
    {
        myIndex = num;  // 다시 값을 넘겨주기 위함
    }

    public void SetTitle(string s)
    {
        bookTitle.text = s;
    }

    public void SetAuthor(string s)
    {
        bookAuthor.text = s;
    }

    public void SetIsbn(string s)
    {
        bookIsbn.text = s;
    }

    public void SetInfo(string s)
    {
        bookInfo.text = s;
    }

    public void SetRating(string s)
    {
        bookRating.text = s;
    }

    public void SetReview(string s)
    {
        title.text = s;
    }

    public void SetThumbnail(Texture texture)
    {
        thumbnail.texture = texture;
    }

    public void SetBestBook(bool best)
    {
        isBest = best;
    }
    #endregion
}
