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

    public float distance = 1.5f;

    // 아바타 머리에 띄우고 싶은 책 선택 관련

    void Start()
    {
        player = GameObject.FindWithTag("Player");


    }

    void Update()
    {
        // 만약 플레이어가 책상 가까이 가면(거리 1정도)
        if(Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            // 쿼드를 띄워준다
            myDesk.transform.GetChild(0).gameObject.SetActive(true);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.transform.gameObject.tag == "ClickHere")
                {
                    

                }


            }
        }


    }

    /*    private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                print("한번만 들어와야하는데");

            }
        }*/
    // 

    public bool isDoneMyCurrList = false;

    public void OpenMyCurrBookList()
    {
        // 읽고있는책 받아오기
        HttpGetCurrBook();
        print("한번만 들어와라");
        if (isDoneMyCurrList)
        {
            return;
        }
    }


    /* 
     *             // 현재 읽고 있는 책 리스트 받아와야함 (계속 받아와야함..? / 현재 읽고 있는 책 열때만..?)
            myCurrBookList = worldManager.myBookList;

            // 현재 읽고 있는 책 Panel
            myCurrBookPanel.SetActive(true);
            //HttpGetCurrBook();
            print("언제끝나");

            // 버튼에 각 정보들 뿌려줌
            // MyCurrBookPanel 의 자식의 인덱스와 myCurrBookList 의 인덱스 맞춰서 넣어줌
            for (int i=0; i < myCurrBookList.Count; i++)
            {
                myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myCurrBookList[i].thumbnail.texture;
            }
     */


    // 현재 읽고 있는 도서 정보 내용 상세보기 함수
    public void OnClickCurrBook()
    {
        // 내가 누구지(나의 정보)
        GameObject me = EventSystem.current.currentSelectedGameObject;

        // 내가 부모(myCurrBookPanel)의 몇번째 자식인지
        int idx = me.transform.GetSiblingIndex();
        print("CurrButtonIdx: " + idx);

        // 생성
        GameObject go = Instantiate(currBookInfoPanelFactory, canvas);
    }

    // 뒤로 가기 버튼
    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }


    // 통신 관련 -------------------------
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
       
        // 통신 성공
        if (type == 200)
        {
            print("통신성공.현재도서");
            // 1. PlayerPref에 key는 jwt, value는 token
            print(jObject);
            //PhotonNetwork.ConnectUsingSettings();
        }
    }
}
