using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;



// 플레이어가 책상 가까이 가면 현재 읽고 있는 책 UI 가 뜬다
public class MyCurrBookPanel : MonoBehaviour
{
    public GameObject player;   // 플레이어
    public GameObject myDesk;   // 책상
    public GameObject myCurrBookPanel;  // 현재 읽고 있는 책 목록 UI
    public GameObject currBookInfoPanel; // 선택한 책 상세 정보

    public GameObject currBookInfoPanelFactory;

    public Transform canvas;

    public WorldManager2D worldManager;
    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>();
    //public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();


    public float distance = 1.5f;

    // 아바타 머리에 띄우고 싶은 책 선택 관련


    void Start()
    {
        player = GameObject.Find("Character");

        // 여기서 씬 시작할 때 다 읽었던 책 한번 뿌려주고 시작
        //HttpGetPastBookList();

        
    }

    void Update()
    {
        // 만약 플레이어가 책상 가까이 가면(거리 1정도)
        if (Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            ShowClickHereObj();
        }
        else
        {
            myDesk.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    public void ShowClickHereObj()
    {
        // 손가락 쿼드를 띄워준다
        myDesk.transform.GetChild(0).gameObject.SetActive(true);
        myCurrBookList = worldManager.myBookList;
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
        currBookInfoPanel.SetImage(myCurrBookList[idx].thumbnail.texture);

        /*        currBookInfoPanel.SetTitle(myBookListNet[idx].bookName);
                currBookInfoPanel.SetAuthor(myBookListNet[idx].bookAuthor);
                currBookInfoPanel.SetPublishInfo(myBookListNet[idx].bookPublishInfo);
                currBookInfoPanel.SetImage(myBookListNet[idx].thumbnail.texture);*/
    }

    // 뒤로 가기 버튼
    public void OnClickExit()
    {
        myCurrBookPanel.SetActive(false);
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
