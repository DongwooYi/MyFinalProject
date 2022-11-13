using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


// 여기서 실시간 배너에 올린 친구 정보 받아오고
// 배너 생성
// 일정 시간 후 삭제 & 새로 생성

public class BannerManager : MonoBehaviour
{
    // 이 친구 생성은 어디서 하지
    // 일단은 myPastBookInfo 리스트의 인덱스 젤 마지막 친구를 데려옴
    // 싱글톤으로 올리면 안되낭..
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();

    public GameObject bannerFactory;
    GameObject banneritem;

    // 스폰 포지션 정해둘까 -> 이건 맵 받으면


    public float bannerTime = 5f;
    float currTime = 0;

    public int idx;

    void Start()
    {
        HttpGetAllBookReview();
        // AI 에서 받아오기 전 임시로
        // 내가 다 읽은 책 list 받아옴 -> 여기서 (랜덤으로) 일정 시간마다 생성
        // 만약 리스트가 비어있으면 안됨
        worldManager = GameObject.Find("WorldManager");
        myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;

        //banneritem = GameObject.FindGameObjectWithTag("BannerItem");
    }

    void Update()
    {
        if(myPastBookInfoList.Count <= 0)
        {
            return;
        }

        currTime += Time.deltaTime;
        if(currTime > bannerTime)
        {
            // 랜덤 인덱스 하나 뽑기
            idx = Random.Range(0, myPastBookInfoList.Count);

            if (banneritem != null)
            {
                // 기존에 있는거 삭제
                Destroy(banneritem);

                // 새로운거 생성
                banneritem = Instantiate(bannerFactory);
            }
            else
            {
                banneritem = Instantiate(bannerFactory);
            }
            MakeBanner(banneritem);
            currTime = 0;
        }
    }

    public void MakeBanner(GameObject bannerItem)
    {
        ReviewManager reviewManager = bannerItem.GetComponent<ReviewManager>();

        reviewManager.SetTitle(myPastBookInfoList[idx].bookName);
        reviewManager.SetReview(myPastBookInfoList[idx].review);
        reviewManager.SetNickname("Nickname");
        reviewManager.SetThumbnail(myPastBookInfoList[idx].thumbnail.texture);
    }

    // Http 통신 관련 -----------------------------------------------
    public void HttpGetAllBookReview()
    {
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:8080//v1/records/all";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetAllBookReview;

        // HttpManager 에게 요청
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnCompleteGetAllBookReview(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        // 통신 성공
        if (type == 200)
        {
            print("통신성공.한줄리뷰");
            // 1. PlayerPref에 key는 jwt, value는 token
            print(jObject);
            //PhotonNetwork.ConnectUsingSettings();
        }
    }

}
