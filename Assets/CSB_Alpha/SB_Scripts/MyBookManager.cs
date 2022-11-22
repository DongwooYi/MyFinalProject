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

    List<_MyBookInfo> myBookList = new List<_MyBookInfo>(); // 담은책

    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>(); // 현재 도서
    //public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();

    List<_MyPastBookInfo> myPastBookList = new List<_MyPastBookInfo>(); // 다읽은도서

    public float distance = 1.5f;   // 플레이어와 물체의 거리



    void Start()
    {
        player = GameObject.Find("Character");
        myBookList = wm.myAllBookList;
        // 여기서 씬 시작할 때 다 읽었던 책 한번 뿌려주고 시작
        //HttpGetPastBookList();    
    }

    void Update()
    {
        // 만약 플레이어가 책상 가까이 가면(거리 1정도)
        if (Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            //ShowClickHereCurrBook();
            ShowMyBookList();
        }
        else
        {
            myDesk.transform.GetChild(0).gameObject.SetActive(false);
        }

        // 만약 플레이어가 책장 가까이 가면
        if(Vector3.Distance(player.transform.position, myBookshelf.transform.position) < distance)
        {
            //ShowClickHerePastBook();
            ShowBookIsDoneT();
        }
        else
        {
            myBookshelf.transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    /* 담은도서 목록 관련 */
    // 책상앞에 가면 담은도서들 보여줌 (isDone == true / false 구분)
    public void ShowMyBookList()
    {
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
                    //HttpGetPastBook();  // 네트워크 통신 -> 함수 만들어줘야함
                    print("담은도서 목록 출력");

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

                    // 담은도서의 수만큼 프리펩 생성
                    for (int i = 0; i < myBookList.Count; i++)
                    {
                        // 만약 isDone 이 true 면 다읽음 목록에 보여줌
                        if (myBookList[i].isDone)
                        {
                            GameObject go = Instantiate(bookFactory, bookContentIsDoneT);
                            // 얘의 RawImage 의 Texture 를 리스트 순서대로
                            go.GetComponent<RawImage>().texture = myBookList[i].thumbnail.texture;
                            MyBook myBook = go.GetComponent<MyBook>();

                            myBook.thumbnail.texture = myBookList[i].thumbnail.texture;
                            myBook.bookTitle = myBookList[i].bookName;
                            myBook.bookAuthor = myBookList[i].bookAuthor;
                            myBook.bookInfo = myBookList[i].bookPublishInfo;
                            myBook.bookIsbn = myBookList[i].bookISBN;
                            myBook.bookRating = myBookList[i].rating;
                            myBook.bookReview = myBookList[i].review;
                            myBook.isDone = myBookList[i].isDone;
                            // index 인 i 값도 넘겨줘야할듯
                            myBook.idx = i;
                        }
                        else
                        {
                            GameObject go = Instantiate(bookFactory, bookContent);
                            // 얘의 RawImage 의 Texture 를 리스트 순서대로
                            go.GetComponent<RawImage>().texture = myBookList[i].thumbnail.texture;
                            MyBook myBook = go.GetComponent<MyBook>();

                            myBook.thumbnail.texture = myBookList[i].thumbnail.texture;
                            myBook.bookTitle = myBookList[i].bookName;
                            myBook.bookAuthor = myBookList[i].bookAuthor;
                            myBook.bookInfo = myBookList[i].bookPublishInfo;
                            myBook.bookIsbn = myBookList[i].bookISBN;
                            myBook.bookRating = myBookList[i].rating;
                            myBook.bookReview = myBookList[i].review;
                            myBook.isDone = myBookList[i].isDone;

                            // index 인 i 값도 넘겨줘야할듯
                            myBook.idx = i;
                        }

                    }
                    myBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    public void ShowAllBookList()
    {
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

        // 담은도서의 수만큼 프리펩 생성
        for (int i = 0; i < wm.myAllBookList.Count; i++)
        {
            // 만약 isDone 이 true 면 다읽음 목록에 보여줌
            if (wm.myAllBookList[i].isDone)
            {
                GameObject go = Instantiate(bookFactory, bookContentIsDoneT);
                // 얘의 RawImage 의 Texture 를 리스트 순서대로
                go.GetComponent<RawImage>().texture = wm.myAllBookList[i].thumbnail.texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookList[i].thumbnail.texture;
                myBook.bookTitle = wm.myAllBookList[i].bookName;
                myBook.bookAuthor = wm.myAllBookList[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookList[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookList[i].bookISBN;
                myBook.bookRating = wm.myAllBookList[i].rating;
                myBook.bookReview = wm.myAllBookList[i].review;
                myBook.isDone = wm.myAllBookList[i].isDone;
                // index 인 i 값도 넘겨줘야할듯
                myBook.idx = i;
            }
            else
            {
                GameObject go = Instantiate(bookFactory, bookContent);
                // 얘의 RawImage 의 Texture 를 리스트 순서대로
                go.GetComponent<RawImage>().texture = wm.myAllBookList[i].thumbnail.texture;
                MyBook myBook = go.GetComponent<MyBook>();

                myBook.thumbnail.texture = wm.myAllBookList[i].thumbnail.texture;
                myBook.bookTitle = wm.myAllBookList[i].bookName;
                myBook.bookAuthor = wm.myAllBookList[i].bookAuthor;
                myBook.bookInfo = wm.myAllBookList[i].bookPublishInfo;
                myBook.bookIsbn = wm.myAllBookList[i].bookISBN;
                myBook.bookRating = wm.myAllBookList[i].rating;
                myBook.bookReview = wm.myAllBookList[i].review;
                myBook.isDone = wm.myAllBookList[i].isDone;

                // index 인 i 값도 넘겨줘야할듯
                myBook.idx = i;
            }

        }
    }

        /* 책장 앞에서 isDone == true 인 책 보기 관련 */
    public void ShowBookIsDoneT()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name == "MyBookshelf")
                {
                    //HttpGetPastBook();  // 네트워크 통신 -> 함수 만들어줘야함
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
                    for (int i = 0; i < wm.myAllBookList.Count; i++)
                    {
                        if (wm.myAllBookList[i].isDone)
                        {
                            // 프리펩 생성
                            GameObject go = Instantiate(pastBookFactory, content);
                            go.GetComponent<RawImage>().texture = wm.myAllBookList[i].thumbnail.texture;
                            MyBook pastBook = go.GetComponent<MyBook>();

                            pastBook.thumbnail.texture = myBookList[i].thumbnail.texture;
                            pastBook.bookTitle = myBookList[i].bookName;
                            pastBook.bookAuthor = myBookList[i].bookAuthor;
                            pastBook.bookInfo = myBookList[i].bookPublishInfo;
                            pastBook.bookIsbn = myBookList[i].bookISBN;
                            pastBook.bookRating = myBookList[i].rating;
                            pastBook.bookReview = myBookList[i].review;
                            pastBook.isDone = myBookList[i].isDone;

                            // index 인 i 값도 넘겨줘야할듯
                            pastBook.idx = i;
                        }
                        else { continue; }
                    }

                    myPastBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }


    /* 현재도서 목록 관련 */
    public void ShowClickHereCurrBook()
    {
        // 손가락 쿼드를 띄워준다
        myDesk.transform.GetChild(0).gameObject.SetActive(true);
        // 손가락 쿼드 항상 카메라 방향
        myDesk.transform.GetChild(0).forward = Camera.main.transform.forward;

        myCurrBookList = wm.myBookList;
        //myBookListNet = worldManager.myBookListNet;

        // MyCurrBookPanel 의 자식의 인덱스와 myCurrBookList 의 인덱스 맞춰서 넣어줌
        for (int i = 0; i < myCurrBookList.Count; i++)
        {
            myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myCurrBookList[i].thumbnail.texture;
            //myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myBookListNet[i].thumbnail.texture;

        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.tag == "ClickHere")
                {
                    //HttpGetCurrBook();  // 네트워크 통신
                    print("이번엔 한번만 들어오겠지");
                    myCurrBookPanel.SetActive(true);
                    myDesk.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    /* 다읽은도서 목록 관련 */
    public void ShowClickHerePastBook()
    {
        // 손가락 쿼드를 띄워준다
        myBookshelf.transform.GetChild(0).gameObject.SetActive(true);
        // 손가락 쿼드의 앞방향을 항상 카메라
        myBookshelf.transform.GetChild(0).forward = Camera.main.transform.forward;

        myPastBookList = wm.myPastBookList;
        //myBookListNet = worldManager.myBookListNet;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                print(hitInfo.transform.name);
                if (hitInfo.transform.gameObject.tag == "ClickHere" || hitInfo.transform.gameObject.name == "MyBookshelf")
                {
                    //HttpGetPastBook();  // 네트워크 통신 -> 함수 만들어줘야함
                    print("완독도서 목록 출력");

                    // 자식이 있다면 삭제
                    Transform[] childList = content.GetComponentsInChildren<Transform>();
                    if(childList != null)
                    {
                        for(int i = 1; i < childList.Length; i++)
                        {
                            Destroy(childList[i].gameObject);
                        }
                    }


                    // myPastBookList 의 크기만큼 프리펩 생성
                    for (int i = 0; i < myPastBookList.Count; i++)
                    {
                        GameObject go = Instantiate(pastBookFactory, content);
                        // 얘의 RawImage 의 Texture 를 리스트 순서대로
                        go.GetComponent<RawImage>().texture = myPastBookList[i].thumbnail.texture;
                        MyBook pastBook = go.GetComponent<MyBook>();

                        pastBook.thumbnail.texture = myPastBookList[i].thumbnail.texture;
                        pastBook.bookTitle = myPastBookList[i].bookName;
                        pastBook.bookAuthor = myPastBookList[i].bookAuthor;
                        pastBook.bookInfo = myPastBookList[i].bookPublishInfo;
                        pastBook.bookIsbn = myPastBookList[i].bookISBN;
                        pastBook.bookRating = myPastBookList[i].rating;
                        pastBook.bookReview = myPastBookList[i].review;
                    }
                    myPastBookPanel.SetActive(true);
                    myBookshelf.transform.GetChild(0).gameObject.SetActive(false);
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
        myCurrBookPanel.SetActive(false);
    }
    public void OnClickExitPast()
    {
        myPastBookPanel.SetActive(false);
    }

    public List<string> titleListNet = new List<string>();
    public List<string> authorListNet = new List<string>();
    public List<string> publishInfoListNet = new List<string>();
    //public List<string> pubdateList = new List<string>();
    public List<string> isbnListNet = new List<string>();
    public List<string> imageListNet = new List<string>();


    // 통신 관련 -------------------------
    #region 현재도서
    void HttpGetCurrBook()
    {
        // 서버에 게시물 조회 요청(/post/1, GET)
        // HttpRequester를 생성
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, 완료되었을 때 호출되는 함수
        requester.url = "http://15.165.28.206:8080/v1/records/reading";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnComplteGetMyCurrBook;

        // HttpManager 에게 요청
        HttpManager.instance.SendRequest(requester, "");
    }


    public void OnComplteGetMyCurrBook(DownloadHandler handler)
    {

        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];
        //string type = (int)jObject["data"]["recordCode"];
       
        //string result_data = ParseJson("[" + handler.text + "]", "data");

        // 통신 성공
        if (type == 200)
        {
            print("통신성공.현재도서");
            // 1. PlayerPref에 key는 jwt, value는 token

            string result_data = ParseJson("[" + handler.text + "]", "data");

            titleListNet = ParseCurrBookList(result_data, "bookName");
            authorListNet = ParseCurrBookList(result_data, "bookAuthor");
            publishInfoListNet = ParseCurrBookList(result_data, "bookPublishInfo");
            //pubdateList = ParseCurrBookList(result_data, "pubdate");
            isbnListNet = ParseCurrBookList(result_data, "bookISBN");
            imageListNet = ParseCurrBookList(result_data, "thumbnailLink");


            for(int i = 0; i < titleListNet.Count; i++)
            {
                _MyBookInfo myCurrBookInfo = new _MyBookInfo();
                
                myCurrBookInfo.bookName = titleListNet[i];
                myCurrBookInfo.bookAuthor = authorListNet[i];
                myCurrBookInfo.bookPublishInfo = publishInfoListNet[i];
                myCurrBookInfo.bookISBN = isbnListNet[i];
                //myCurrBookInfo.thumbnail = imageListNet[i];

                //myBookListNet.Add(myCurrBookInfo);
            }

            print(jObject);
            //PhotonNetwork.ConnectUsingSettings();
        }
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

    List<string> ParseCurrBookList(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        List<string> result = new List<string>();

        foreach (JObject obj in parseData.Children())
        {
            result.Add(obj.GetValue(key).ToString());
        }

        return result;
    }
    #endregion

    void HttpGetPastBookList()
    {
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, 완료되었을 때 호출되는 함수
        requester.url = "http://15.165.28.206:8080/v1/records/count";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnComplteGetMyPastBookList;

        // HttpManager 에게 요청
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnComplteGetMyPastBookList(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];
        //string type = (int)jObject["data"]["recordCode"];

        // 통신 성공
        if (type == 200)
        {
            print("통신성공.읽은도서 모두");
            // 1. PlayerPref에 key는 jwt, value는 token
            print(jObject);
            //PhotonNetwork.ConnectUsingSettings();
        }
    }

}
