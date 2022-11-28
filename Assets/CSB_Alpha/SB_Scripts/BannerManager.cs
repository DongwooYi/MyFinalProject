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
    //GameObject worldManager;
   // List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();

    public GameObject bannerFactory;
    GameObject banneritem;

    // 스폰 포지션 정해둘까 -> 이건 맵 받으면


    public float bannerTime = 5f;
    float currTime = 0;

    public int idx;

    void Start()
    {
        HttpGetGetOneLineReview();
        // AI 에서 받아오기 전 임시로
        // 내가 다 읽은 책 list 받아옴 -> 여기서 (랜덤으로) 일정 시간마다 생성
        // 만약 리스트가 비어있으면 안됨
        //worldManager = GameObject.Find("WorldManager");
       // myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;

        //banneritem = GameObject.FindGameObjectWithTag("BannerItem");
    }

    public Transform transformBanner;
    void Update()
    {
        if (myAllBookListNet.Count <= 0)
        {
            return;
        }

        currTime += Time.deltaTime;
        if (currTime > bannerTime)
        {
            // 랜덤 인덱스 하나 뽑기
            idx = Random.Range(0, myAllBookListNet.Count);

            if (banneritem != null)
            {
                // 기존에 있는거 삭제
                Destroy(banneritem);

                // 새로운거 생성
                banneritem = Instantiate(bannerFactory);
                banneritem.transform.position = transformBanner.transform.position;
            }
          else
            {
                banneritem = Instantiate(bannerFactory);
                banneritem.transform.position = transformBanner.transform.position;
            }
            MakeBanner(banneritem);
            currTime = 0;
        }
    }

    public void MakeBanner(GameObject bannerItem)
    {
        ReviewManager reviewManager = bannerItem.GetComponent<ReviewManager>();

        reviewManager.SetTitle(myAllBookListNet[idx].bookName);
        reviewManager.SetReview(myAllBookListNet[idx].review);
        reviewManager.SetNickname("Nickname");
        reviewManager.SetThumbnail(myAllBookListNet[idx].texture);
    }

    public List<_MyBookInfo> myAllBookListNet = new List<_MyBookInfo>();   // 담은도서


    // 네트워크에서 받아온 정보 저장할 리스트
    public List<string> titleListNet = new List<string>();
    public List<string> thumbnailLinkListNet = new List<string>();
    public List<string> reviewListNet = new List<string>();
    public List<string> nicknameList = new List<string>();
    public List<Texture> thumbnailImgListNet = new List<Texture>();

    // (바뀐 버전) Http 통신 관련 -------------------------------------------------------
    // 1. 월드 입장시 요청할 API : 읽은 책(책장), 인생책 (낮은 책장) 정보 보내주기
    public void HttpGetGetOneLineReview()
    {            // 각 data 리스트들 초기화
        titleListNet.Clear();
        thumbnailLinkListNet.Clear();
        thumbnailImgListNet.Clear();
        reviewListNet.Clear();
        nicknameList.Clear();

        myAllBookListNet.Clear();
        print("요청");
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, 완료되었을 때 호출되는 함수
        requester.url = "http://15.165.28.206:80/v1/records/all";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetOneLineReview;

        // HttpManager 에게 요청
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnCompleteGetOneLineReview(DownloadHandler handler)
    {
        // 데이터 처리
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("통신성공. 배너");
            string result_data = ParseGETJson("[" + handler.text + "]", "data");
          
            titleListNet = ParseMyBookData(result_data, "bookName");
            nicknameList = ParseMyBookData(result_data, "name");
            reviewListNet = ParseMyBookData(result_data, "oneLineReview");
            thumbnailLinkListNet = ParseMyBookData(result_data, "thumbnailLink");
                
            


            GETThumbnailTexture();
        }
    }

    public void GETThumbnailTexture()
    {
        StartCoroutine(GetThumbnailImg(thumbnailLinkListNet.ToArray()));
    }

    public IEnumerator GetThumbnailImg(string[] url)
    {
        for (int j = 0; j < 40; j++)
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
        for (int i = 0; i < thumbnailImgListNet.Count; i++)
        {
            _MyBookInfo myBookInfo = new _MyBookInfo();

            myBookInfo.bookName = titleListNet[i];
            myBookInfo.thumbnailLink = thumbnailLinkListNet[i];
            myBookInfo.review = reviewListNet[i];
            myBookInfo.nickname = nicknameList[i];

            myBookInfo.texture = thumbnailImgListNet[i];
            myAllBookListNet.Add(myBookInfo);
        }
    }

    // data parsing
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
}
