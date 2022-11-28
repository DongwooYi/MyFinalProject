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
   // public Text bookRating;
    public Text bookReview;

    public Toggle isBestToggle;
    public bool isBest; // 내가 인생책인가
    bool temp;

    public Transform myPastBookPanel;
    public Transform contentDoneBook;

    public GameObject myBookManager;
    public GameObject confirmFactory;

    GameObject worldManager;
    Transform canvas;
    WorldManager2D wm;

    private void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();

        canvas = GameObject.Find("Canvas").transform;
        myBookManager = GameObject.Find("MyBookManager");
        myPastBookPanel = GameObject.Find("MyPastBookPanel").transform;///Scroll View_Done/Viewport/Content").transform;
        contentDoneBook = GameObject.Find("Scroll View_Done/Viewport/Content").transform;//")//.transform;///MyPastBookPanel/Scroll View_Done/Viewport/Content").transform;
        isBestToggle.isOn = isBest;
        isBestToggle.onValueChanged.AddListener(OnCheck);

        // BestBookList 에 한번 추가
        for (int i = 0; i < contentDoneBook.childCount; i++)
        {
            if (contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isBest)
            {
                BestBookList.Add(contentDoneBook.GetChild(i).gameObject);   // 인생도서 리스트에 추가
            }
        }

    }

    public bool tempMyBook;
    public bool isBestMyBook;
    public void OnCheck(bool checkBool)
    {
        temp = isBest;  // 이전 값 저장
        isBest = checkBool; // 변한 값
        // 받은 인덱스에 해당하는 책의 isBest 바꾸고
        //tempMyBook = contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBestTemp;
        //isBestMyBook = contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBest;
        contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBestTemp = temp;
        contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBest = isBest;
        //tempMyBook = temp;
        //isBestMyBook = isBest;
        print(temp);
        print(checkBool);
        print(isBest);
    
            OnClickSetBook();
       
    }

    public List<GameObject> BestBookList = new List<GameObject>();
    public List<BestBook> httpList = new List<BestBook>();

    string isBestStr;
    public void OnClickSetBook()
    {
        // 만
        // isBest == true 면 BestBookList 에 추가
        // BestBookList.Count > 3 이면 리스트 맨 앞 값 삭제

        if (isBest)
        {
            BestBookList.Add(contentDoneBook.GetChild(myIndex).gameObject);
            if (BestBookList.Count > 3) BestBookList.RemoveAt(0);
        }

/*        for (int i = 0; i < contentDoneBook.childCount; i++)
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

            }
        }*/



        // 만약 temp 와 isBest 가 다르다면 전송할 리스트에 담기
        if (temp != isBest)
        {
            BestBook bestBook = new BestBook();
            bestBook.bookISBN = bookIsbn.text;
            if (isBest) 
            { 
                isBestStr = "Y";
                
            }
            else isBestStr = "N";
            bestBook.isBest = isBestStr;

            httpList.Add(bestBook);
            print("인생책: " + bestBook);
            print(bestBook.bookISBN + bestBook.isBest);

            HttpPostMyBestBook();

            if (isBest)
            {        
                // 인생책으로 등록되었습니다 UI 띄우기
                GameObject go = Instantiate(confirmFactory, canvas);
            }
        }


        // MyBookManager.cs 의 bestBookList 에 값 담아줌
        myBookManager.GetComponent<MyBookManager>().bestBookList = BestBookList;
        //Destroy(gameObject);
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
            recordList = httpList,
        };
        print(bookData);
        requester.body = JsonUtility.ToJson(bookData, true);
        print(requester.body);
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
            print("인생책 통신 완료");
           // wm.myAllBookListNet.Clear();
            HttpGetMyBookData();
        }
    }

    // 1. 월드 입장시 요청할 API : 읽은 책(책장), 인생책 (낮은 책장) 정보 보내주기
    public void HttpGetMyBookData()
    {
        // 각 data 리스트들 초기화
        wm.titleListNet.Clear();
        wm.authorListNet.Clear();
        wm.publishInfoListNet.Clear();
        wm.thumbnailLinkListNet.Clear();
        wm.thumbnailImgListNet.Clear();
        wm.isbnListNet.Clear();
        wm.ratingListNet.Clear();
        wm.reviewListNet.Clear();
        wm.isDoneListNet.Clear();
        wm.isBestsListNet.Clear();

        wm.myAllBookListNet.Clear();
        print("요청11");
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, 완료되었을 때 호출되는 함수
        requester.url = "http://15.165.28.206:80/v1/records/myroom";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetMyBookData;

        // HttpManager 에게 요청
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnCompleteGetMyBookData(DownloadHandler handler)
    {
        // 데이터 처리
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("통신성공. 모든도서.서재입장");
            string result_data = ParseGETJson("[" + handler.text + "]", "data");

            wm.titleListNet = ParseMyBookData(result_data, "bookName");
            wm.authorListNet = ParseMyBookData(result_data, "bookAuthor");
            wm.publishInfoListNet = ParseMyBookData(result_data, "bookPublishInfo");
            wm.thumbnailLinkListNet = ParseMyBookData(result_data, "thumbnailLink");
            wm.isbnListNet = ParseMyBookData(result_data, "bookISBN");
            wm.ratingListNet = ParseMyBookData(result_data, "rating");
            wm.reviewListNet = ParseMyBookData(result_data, "bookReview");
            wm.isDoneListNet = ParseMyBookData(result_data, "isDone");
            wm.isBestsListNet = ParseMyBookData(result_data, "isBest");

            GETThumbnailTexture();
        }
    }

    public void GETThumbnailTexture()
    {
        StartCoroutine(GetThumbnailImg(wm.thumbnailLinkListNet.ToArray()));
    }

    public IEnumerator GetThumbnailImg(string[] url)
    {
        for (int j = 0; j < url.Length; j++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url[j]);
            yield return www.SendWebRequest();


            if (www.result != UnityWebRequest.Result.Success)
            {
                print("실패");
                break;
            }
            else
            {
                Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                wm.thumbnailImgListNet.Add(myTexture);
            }
            yield return null;
        }
        for (int i = 0; i < wm.titleListNet.Count; i++)
        {
            _MyBookInfo myBookInfo = new _MyBookInfo();

            myBookInfo.bookName = wm.titleListNet[i];
            myBookInfo.bookAuthor = wm.authorListNet[i];
            myBookInfo.bookPublishInfo = wm.publishInfoListNet[i];
            myBookInfo.thumbnailLink = wm.thumbnailLinkListNet[i];
            myBookInfo.bookISBN = wm.isbnListNet[i];
            myBookInfo.rating = wm.ratingListNet[i];
            myBookInfo.review = wm.reviewListNet[i];
            myBookInfo.isDoneString = wm.isDoneListNet[i];
            myBookInfo.isBestString = wm.isBestsListNet[i];
            //myBookInfo.thumbnail = rawImages[i];
            myBookInfo.texture = wm.thumbnailImgListNet[i];
            wm.myAllBookListNet.Add(myBookInfo);
        }
        wm.SettingMyRoom();
        print("월드세팅");
    }
    public string ParseGETJson(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        string result = "";

        foreach (JObject obj in parseData.Children())
        {
            result = obj.GetValue(key).ToString();
        }

        return result;
    }

    // data 에서 key 별로 parsing
    public List<string> ParseMyBookData(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        List<string> result = new List<string>();

        foreach (JObject obj in parseData.Children())
        {
            result.Add(obj.GetValue(key).ToString());
        }
        return result;
    }
    public void OnClickExit()
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
    [Header("평점 버튼")]
    public Button[] starButton;
    public Button acceptButton;
    [HideInInspector] public int ratedApp;
    public void SetRating(string s)
    {
        // bookRating.text = s;
        int rate = int.Parse(s);
        for (int i = 0; i < rate; i++)
        {
            foreach(Transform t in starButton[i].transform)
            {
                t.gameObject.SetActive(true);
            }
        }

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
        isBestToggle.isOn = best;
    }
    #endregion
}
