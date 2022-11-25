using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;



// 플레이어가 책상 가까이 가면 현재 읽고 있는 책 UI 가 뜬다
public class MyBookManager : MonoBehaviour
{
    public GameObject player;   // 플레이어
    public GameObject myDesk;   // 책상
    public GameObject myCurrBookPanel;  // 현재 읽고 있는 책 목록 UI

    public GameObject myBookshelf;    // 책장
    public GameObject myPastBookPanel;  // 다읽은 책 목록 UI

    public GameObject myBookPanel;  // 다읽은 책 목록 UI

    public GameObject bookFactory;  // 담은도서 목록 공장
    public GameObject currBookInfoPanelFactory; // 현재 도서 상세 내용
    public GameObject pastBookFactory; // 다읽은도서 상세 내용

    public Transform canvas;
    public Transform content;

    // 담은도서
    public Transform bookContent;
    public Transform bookContentIsDoneT;

    public WorldManager2D wm;

    public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();  // 담은책 네트워크

    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>(); // 현재 도서

    public float distance = 1.5f;   // 플레이어와 물체의 거리
    public float test;   // 플레이어와 물체의 거리
    public float test1;   // 플레이어와 물체의 거리

    void Start()
    {
        player = GameObject.Find("Character");
    }

    void Update()
    {
        test = Vector3.Distance(player.transform.position, myDesk.transform.position);
        test1 = Vector3.Distance(player.transform.position, myBookshelf.transform.position);
        // 만약 플레이어가 책상 가까이 가면(거리 1정도)
        if (Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            ShowMyBookList();
        }
        else
        {
            myDesk.transform.GetChild(0).gameObject.SetActive(false);
        }

        // 만약 플레이어가 책장 가까이 가면
        if(Vector3.Distance(player.transform.position, myBookshelf.transform.position) < 3.5f)
        {
            //print("11");
            ShowBookIsDoneT();
        }
        else
        {
            myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    /* 담은도서 목록 관련 */
    // <책상> 앞에 가면 담은도서들 보여줌 (isDone == true / false 구분)
    // isDoneString 값 "Y" / "N" 에 따라 뿌려지는 곳이 다름
    int rayCount = 0;

    public void ShowMyBookList()
    {
        if (rayCount > 0)
        {
            return;
        }

        // 손가락 쿼드를 띄워준다
        myDesk.transform.GetChild(0).gameObject.SetActive(true);
        // 손가락 쿼드 항상 카메라 방향
        myDesk.transform.GetChild(0).forward = Camera.main.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name == "MyDesk")
                {
                    // 네트워크에서 받아온 정보 저장할 리스트
                    wm.titleListNet.Clear();
                    wm.authorListNet.Clear();
                    wm.publishInfoListNet.Clear();
                    wm.thumbnailLinkListNet.Clear();
                    wm.isbnListNet.Clear();
                    wm.ratingListNet.Clear();
                    wm.reviewListNet.Clear();
                    wm.isDoneListNet.Clear();
                    wm.isBestsListNet.Clear();

                    wm.thumbnailImgListNet.Clear();

                    wm.myAllBookListNet.Clear();  // 담은도서 리스트 초기화
                    HttpGetMyBookData();
                    // 담은도서 리스트인 myBookLIstNet 에 담은도서들 들어있음
                    // 담은도서 리스트인 wm.myAllBookListNet 에 담은도서들 들어있음
                    print("책상 앞 담은도서 전체 스크롤뷰에 배치");

                    // 담은도서의 수만큼 프리펩 생성

                    //MakePrefab();
                    myBookPanel.SetActive(true);
                    myDesk.transform.GetChild(0).gameObject.SetActive(false);

                    rayCount++;
                    return;
                }
            }
        }
    }

    public void MakePrefab()
    {
        #region 자식 삭제 (프리펩)
        // 자식이 있다면 삭제
        Transform[] childList = bookContent.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        // 자식이 있다면 삭제
        Transform[] childList1 = bookContentIsDoneT.GetComponentsInChildren<Transform>();
        if (childList1 != null)
        {
            for (int i = 1; i < childList1.Length; i++)
            {
                Destroy(childList1[i].gameObject);
            }
        }
        #endregion
        for (int i = 0; i < wm.myAllBookListNet.Count; i++)
        {
            // 만약 isDoneString 이 "Y" 면 담은도서(done) 목록에 보여줌
            if (wm.myAllBookListNet[i].isDoneString == "Y")
            {
                GameObject go = Instantiate(bookFactory, bookContentIsDoneT);
                // 얘의 RawImage 의 Texture 를 리스트 순서대로
                go.GetComponent<RawImage>().texture = wm.myAllBookListNet[i].texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookListNet[i].texture;
                myBook.bookTitle = wm.myAllBookListNet[i].bookName;
                myBook.bookAuthor = wm.myAllBookListNet[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookListNet[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookListNet[i].bookISBN;
                myBook.bookRating = wm.myAllBookListNet[i].rating;
                myBook.bookReview = wm.myAllBookListNet[i].review;
                myBook.isDone = true;

                // index 인 i 값도 넘겨줘야할듯
                myBook.idx = i;
            }
            // 만약 isDoneString 이 "N" 이면 담은도서(ing) 목록
            else if (wm.myAllBookListNet[i].isDoneString == "N")
            {
                GameObject go = Instantiate(bookFactory, bookContent);
                // 얘의 RawImage 의 Texture 를 리스트 순서대로
                go.GetComponent<RawImage>().texture = wm.myAllBookListNet[i].texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookListNet[i].texture;
                myBook.bookTitle = wm.myAllBookListNet[i].bookName;
                myBook.bookAuthor = wm.myAllBookListNet[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookListNet[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookListNet[i].bookISBN;
                myBook.bookRating = wm.myAllBookListNet[i].rating;
                myBook.bookReview = wm.myAllBookListNet[i].review;
                myBook.isDone = false;

                // index 인 i 값도 넘겨줘야할듯
                myBook.idx = i;
            }
        }
    }
    /* <책장 앞>에서 isDone == true 인 책 보기 관련 */
    int doneBookCount = 0;
    public void ShowBookIsDoneT()
    {
        if (doneBookCount > 0)
        {
            return;
        }

        print("들어오니 책장");
        // 손가락 쿼드를 띄워준다
        myBookshelf.transform.GetChild(0).gameObject.SetActive(true);
        // 손가락 쿼드 항상 카메라 방향
        myBookshelf.transform.GetChild(0).forward = Camera.main.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            print("클릭했니?");
            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name.Contains("MyBookshelf"))
                {
                    print("완독도서 목록 출력");

                    // 자식이 있다면 삭제
                    Transform[] childList = content.GetComponentsInChildren<Transform>();
                    if (childList != null)
                    {
                        for (int i = 1; i < childList.Length; i++)
                        {
                            Destroy(childList[i].gameObject);
                        }
                    }

                    // WorldManager 의 myAllBookList 의 중 isDone == true 인 것들 프리펩 생성
                    for (int i = 0; i < wm.myAllBookListNet.Count; i++)
                    {
                        if (wm.myAllBookListNet[i].isDoneString == "Y")
                        {
                            // 프리펩 생성
                            GameObject go = Instantiate(pastBookFactory, content);
                            go.GetComponent<RawImage>().texture = wm.myAllBookListNet[i].texture;
                            MyBook pastBook = go.GetComponent<MyBook>();

                            pastBook.thumbnail.texture = wm.myAllBookListNet[i].texture;
                            pastBook.bookTitle = wm.myAllBookListNet[i].bookName;
                            pastBook.bookAuthor = wm.myAllBookListNet[i].bookAuthor;
                            pastBook.bookInfo = wm.myAllBookListNet[i].bookPublishInfo;
                            pastBook.bookIsbn = wm.myAllBookListNet[i].bookISBN;
                            pastBook.bookRating = wm.myAllBookListNet[i].rating;
                            pastBook.bookReview = wm.myAllBookListNet[i].review;
                            pastBook.isDone = true;
                            pastBook.isBestStr = wm.myAllBookListNet[i].isBestString;

                            // index 인 i 값도 넘겨줘야할듯
                            pastBook.idx = i;
                        }
                        else { continue; }
                    }

                    myPastBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);

                    doneBookCount++;
                    return;
                }
            }
        }
    }

    public GameObject me;
    public int idx;

    // 현재 읽고 있는 도서 정보 내용 상세보기 함수
    public void OnClickCurrBook()
    {
        // 내가 누구지(나의 정보)
        me = EventSystem.current.currentSelectedGameObject;

        // 내가 부모(myCurrBookPanel)의 몇번째 자식인지
        idx = me.transform.GetSiblingIndex();
        print("CurrButtonIdx: " + idx);

        // 생성
        GameObject go = Instantiate(currBookInfoPanelFactory, canvas);

        CurrBookInfoPanel currBookInfoPanel = go.GetComponent<CurrBookInfoPanel>();

        currBookInfoPanel.SetTitle(myCurrBookList[idx].bookName);
        currBookInfoPanel.SetAuthor(myCurrBookList[idx].bookAuthor);
        currBookInfoPanel.SetPublishInfo(myCurrBookList[idx].bookPublishInfo);
        currBookInfoPanel.SetThumbnail(myCurrBookList[idx].thumbnail.texture);

        /*        currBookInfoPanel.SetTitle(myBookListNet[idx].bookName);
                currBookInfoPanel.SetAuthor(myBookListNet[idx].bookAuthor);
                currBookInfoPanel.SetPublishInfo(myBookListNet[idx].bookPublishInfo);
                currBookInfoPanel.SetImage(myBookListNet[idx].thumbnail.texture);*/
    }

    // 뒤로 가기 버튼
    public void OnClickExitCurr()
    {
        rayCount = 0;
        myCurrBookPanel.SetActive(false);
    }
    public void OnClickExitPast()
    {
        doneBookCount = 0;
        myPastBookPanel.SetActive(false);
    }

    // 네트워크에서 받아온 정보 저장할 리스트
    public List<string> titleListNet = new List<string>();
    public List<string> authorListNet = new List<string>();
    public List<string> publishInfoListNet = new List<string>();
    public List<string> thumbnailLinkListNet = new List<string>();
    public List<string> isbnListNet = new List<string>();
    public List<string> ratingListNet = new List<string>();
    public List<string> reviewListNet = new List<string>();
    public List<string> isDoneListNet = new List<string>();
    public List<string> isBestsListNet = new List<string>();
    public List<Texture> thumbnailImgListNet = new List<Texture>();


    // (바뀐 버전) Http 통신 관련 ------------------------
    // 2. 책상 앞으로 갔을 때 호출할 API : 담은 책 + 읽은 책 가져오기 (모든 책 정보 다 보내줌)
    void HttpGetMyBookData()
    {
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, 완료되었을 때 호출되는 함수
        requester.url = "http://15.165.28.206:80/v1/records/desk";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetMyBookData;

        // HttpManager 에게 요청
        HttpManager.instance.SendRequest(requester, "");
    }

    void OnCompleteGetMyBookData(DownloadHandler handler)
    {
        // 데이터 처리
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("통신성공. 모든도서.책상앞");
            string result_data = ParseJson("[" + handler.text + "]", "data");

            titleListNet = ParseMyBookData(result_data, "bookName");
            authorListNet = ParseMyBookData(result_data, "bookAuthor");
            publishInfoListNet = ParseMyBookData(result_data, "bookPublishInfo");
            thumbnailLinkListNet = ParseMyBookData(result_data, "thumbnailLink");
            isbnListNet = ParseMyBookData(result_data, "bookISBN");
            ratingListNet = ParseMyBookData(result_data, "rating");
            reviewListNet = ParseMyBookData(result_data, "bookReview");
            isDoneListNet = ParseMyBookData(result_data, "isDone");
            isBestsListNet = ParseMyBookData(result_data, "isBest");

            StartCoroutine(GetThumbnailImg(thumbnailLinkListNet.ToArray()));   //, myBookInfo.thumbnail)

            print(jObject);
        }
    }

    IEnumerator GetThumbnailImg(string[] url)
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
                thumbnailImgListNet.Add(myTexture);
            }
            yield return null;
        }
        for (int i = 0; i < titleListNet.Count; i++)
        {

            _MyBookInfo myBookInfo = new _MyBookInfo();

            myBookInfo.bookName = titleListNet[i];
            myBookInfo.bookAuthor = authorListNet[i];
            myBookInfo.bookPublishInfo = publishInfoListNet[i];
            myBookInfo.thumbnailLink = thumbnailLinkListNet[i];
            myBookInfo.bookISBN = isbnListNet[i];
            myBookInfo.rating = ratingListNet[i];
            myBookInfo.review = reviewListNet[i];
            myBookInfo.isDoneString = isDoneListNet[i];
            myBookInfo.isBestString = isBestsListNet[i];

            myBookInfo.texture = thumbnailImgListNet[i];

            wm.myAllBookListNet.Add(myBookInfo);
        }
        // 책상 앞 panel 세팅
        MakePrefab();
    }

    // data parsing
    string ParseJson(string jsonText, string key)
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
    List<string> ParseMyBookData(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        List<string> result = new List<string>();

        foreach (JObject obj in parseData.Children())
        {
            result.Add(obj.GetValue(key).ToString());
        }

        return result;
    }
}
