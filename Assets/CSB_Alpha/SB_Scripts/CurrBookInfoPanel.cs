using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using TMPro;


// 책상 앞에 가서 뜬 <담은도서> 중 
// 
public class CurrBookInfoPanel : MonoBehaviour
{
    Transform canvas;
    public GameObject bookFactory;  // 담은도서 목록 공장
    public GameObject headConfirm; // <대표책이 설정되었습니다> 안내
    public GameObject doneBookConfirm;  // <나의 서재에 담겼습니다> 설정되었습니다> 안내

    GameObject worldManager;
    GameObject myBookManager;

    public Transform myBookPanel;
    public Transform contentBook;
    public Transform contentDoneBook;


    public string rateNumber;

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public Text rating;
    //public InputField review;
    public TMP_InputField reviewTMP;
    public RawImage thumbnail;
    public Texture texture;

    bool isDone;
    string isDoneString;
    public Toggle checkIsDone;  // 

    public int idx;

    public Dropdown dropdown;

    //public TMP_InputField inputFieldReview; // 리뷰 입력 칸
    public InputField review;
    public Button btnEnter; // 등록하기 버튼

    public GameObject player;   // 플레이어
    public GameObject showBook;

    public _MyBookInfo[] myAllBookListToArray;

    GameObject book;


    public Toggle headBook; // 머리책(대표) 
    public string isOverHeadString;
    public bool isOverHead;

    WorldManager2D wm;

    /*    public void ToggleHead(Toggle headBook)
        {
            print("토글");
            if (headBook.isOn)
            {
                print("토글" + headBook.isOn);
                showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
                //player.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
            }
        }*/

    HttpManager httpManager;
    void Start()
    {
       
        myBookPanel = GameObject.Find("MyBookPanel").transform;
        contentBook = GameObject.Find("Scroll View_Book/Viewport/Content").transform;
        contentDoneBook = GameObject.Find("Scroll View_Done/Viewport/Content").transform;

        canvas = GameObject.Find("Canvas").transform;

        player = GameObject.Find("Character");
        showBook = GameObject.Find("ShowBook");

        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();

        myAllBookListToArray = wm.myAllBookListNet.ToArray();
        book = GameObject.Find("Book");

        review.onValueChanged.AddListener(OnValueChanged);
        checkIsDone.onValueChanged.AddListener(OnisDoneToggleClicked);
        headBook.onValueChanged.AddListener(OnOverHeadToggle);
    }

    void OnValueChanged(string s)
    {
       btnEnter.interactable = s.Length >= 0;  // 등록 버튼 활성화
    }

    // 등록 버튼
    // isDone == true 면 WorldManager 의 myDoneBookList 에
    // isDone == false 면 review 저장
    public void OnClickEnter()
    {
        if (checkIsDone.isOn) isDone = true;
        else if (!checkIsDone.isOn) isDone = false;

        print(isDone);

        if (isDone)
        {
            isDoneString = "Y";
            // 0. WorldManager 의 myAllBookList 업데이트
            // isDone = true 포함
            myAllBookListToArray[idx].bookName = title.text;
            myAllBookListToArray[idx].bookAuthor = author.text;
            myAllBookListToArray[idx].bookISBN = isbn.text;
            myAllBookListToArray[idx].bookPublishInfo = publishInfo.text;
            myAllBookListToArray[idx].isDone = true;
            myAllBookListToArray[idx].isDoneString = "Y";
            myAllBookListToArray[idx].rating = rateNumber;
            myAllBookListToArray[idx].review = review.text;
            myAllBookListToArray[idx].isOverHead = isOverHead;
            myAllBookListToArray[idx].isOverHeadString = isOverHeadString;

            // POSt 로 보내기
            HttpPostMyBookDataD();
        }
        else
        {
            isDoneString = "N";
            myAllBookListToArray[idx].bookName = title.text;
            myAllBookListToArray[idx].bookAuthor = author.text;
            myAllBookListToArray[idx].bookISBN = isbn.text;
            myAllBookListToArray[idx].bookPublishInfo = publishInfo.text;
            myAllBookListToArray[idx].isDone = false;
            myAllBookListToArray[idx].isDoneString = "N";
            myAllBookListToArray[idx].rating = rateNumber;
            myAllBookListToArray[idx].review = review.text;
            myAllBookListToArray[idx].isOverHead = isOverHead;
            myAllBookListToArray[idx].isOverHeadString = isOverHeadString;

            // POST 로 보내기
            HttpPostMyBookData();
        }


        // <등록 되었습니다>
        confirmMsg = Instantiate(doneBookConfirm, gameObject.transform);    // 나의 자식으로 생성
       // myBookManager.SetActive(false);
    }

    GameObject confirmMsg;

    // 나가기 버튼 (누르면 저장되지 않음)
    public void OnClickExit()
    {
        Destroy(gameObject);
       // HttpGetMyBookData();

    }
    /*
        // <나의 서재에 담겼었습니다> 닫기 버튼
        public void OnClickConfirmMyBook()
        {
            Destroy(gameObject);

        }*/

    #region 텍스트 세팅 관련
    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetAuthor(string s)
    {
        author.text = s;
    }

    public void SetPublishInfo(string s)
    {
        publishInfo.text = s;
    }

    public void SetIsbn(string s)
    {
        isbn.text = s;
    }

    public void SetThumbnail(Texture texture)
    {
        thumbnail.texture = texture;
    }

    public void SetRating(string s)
    {
        //rating.text = s;
        RateApplication(int.Parse(s));
    }

    public void SetReview(string s)
    {
        review.text = s;
    }

    public void SetIndex(int num)
    {
        idx = num;
    }
    
    public void SetOverHeadBook(bool overHead)
    {
        isOverHead = overHead;
        // 토글에 표시
        headBook.isOn = overHead;
        print("대표책 : " + isOverHead + "토글 : " + headBook.isOn);
    }

    GameObject headBookInfo;
    public void OnOverHeadToggle(bool isOverHead)
    {
        print("토글 리스너: " + isOverHead);
        if(isOverHead)
        {
            isOverHeadString = "Y";
            headBookInfo = Instantiate(headConfirm, canvas);
            // 다른 책들 isOverHead = false & isOverHeadString = "N" 로 
            if (isDone) // 만약 내가 다읽은도서라면
            {
                for (int i = 0; i < contentBook.childCount; i++)    // 담은도서는 모두 N 으로
                {
                    contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                    contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                }
                for (int i = 0; i < contentDoneBook.childCount; i++)    // 다읽은도서는 나 제외 N 으로
                {
                    if (i == idx)
                    {
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = true;
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "Y";
                    }
                    else
                    {
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                        contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                    }

                }
            }
            else if (!isDone)   // 만약 담은도서라면 
            {
                for (int i = 0; i < contentBook.childCount; i++)
                {
                    if (i == idx)
                    {
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = true;
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "Y";
                    }
                    else
                    {
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                        contentBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                    }

                }
                for (int i = 0; i < contentDoneBook.childCount; i++)
                {
                    contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHead = false;
                    contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isOverHeadString = "N";
                }
            }
            showBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", thumbnail.texture);
        }
        else
        {
            isOverHeadString = "N";
        }
    }

    // 대표책 설정완료 안내 이후, 닫기 버튼 누르면 대표책 등록 완료
    public void OnClickHeadBookConfirmBook()
    {
        Destroy(headBookInfo);
    }

    public void SetIsDone(bool done)
    {
        isDone = done;
        // 토글에 체크 표시
        checkIsDone.isOn = done;
        print("체크 : " + checkIsDone.isOn);
    }
    public void OnisDoneToggleClicked(bool isDone)
    {
        print(isDone);
        print("왜 안바껴");
    }
    #endregion


    // (바뀐 버전) Http 통신 관련 ---------------------------------------------
    // 4. 독서 기록 쓰기
    // 비고) 이미 담는 과정에서 이미지 파일은 업로드 했기 때문에, 이미지 파일 제외하고 보내주세요
    // 비고) 호출 후 1번 다시 호출해줘야 함
    void HttpPostMyBookDataD()
    {
        print("000");
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:80/v1/records/write";
        requester.requestType = RequestType.POST;

        BookData bookData = new()
        {
            bookName = title.text,
            bookAuthor = author.text,
            bookPublishInfo = publishInfo.text,
            bookISBN = isbn.text,
            rating = rateNumber,
            bookReview = review.text,
            isDone = "Y",
            isOverHead = isOverHeadString,
        };

        requester.body = JsonUtility.ToJson(bookData, true);
        requester.onComplete = OnCompletePostMyBookData;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void HttpPostMyBookData()
    {
        //서버에 게시물 조회 요청
        //HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:80/v1/records/write";
        requester.requestType = RequestType.POST;

        BookData bookData = new()
        {
            bookName = title.text,
            bookAuthor = author.text,
            bookPublishInfo = publishInfo.text,
            bookISBN = isbn.text,
            rating = rateNumber,
            bookReview = review.text,
            isOverHead = isOverHeadString,
        };

        requester.body = JsonUtility.ToJson(bookData, true);
        print(requester.body);
        requester.onComplete = OnCompletePostMyBookData;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void OnCompletePostMyBookData(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("되나?");
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
        myBookManager.GetComponent<MyBookManager>().MakePrefab();
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

    [Header("평점 버튼")]
    public Button[] starButton;
    public Button acceptButton;
    [HideInInspector] public int ratedApp;
    public void RateApplication(int rate)
    {
        ratedApp = rate;

        // active rate button if use click some stars
        if (rate > 0)
            acceptButton.GetComponent<Button>().interactable = true;

        // enable stars equal than user rated
        for (int i = 0; i < rate; i++)
        {
            foreach (Transform t in starButton[i].transform)
            {
                t.gameObject.SetActive(true);
            }
        }

        // enable stars greater than user rated
        for (int i = rate; i < starButton.Length; i++)
        {
            foreach (Transform t in starButton[i].transform)
            {
                t.gameObject.SetActive(false);
            }

        }
        rateNumber = rate.ToString();
    }

}
