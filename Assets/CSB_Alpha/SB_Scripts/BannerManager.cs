using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


// ���⼭ �ǽð� ��ʿ� �ø� ģ�� ���� �޾ƿ���
// ��� ����
// ���� �ð� �� ���� & ���� ����

public class BannerManager : MonoBehaviour
{
    // �� ģ�� ������ ��� ����
    // �ϴ��� myPastBookInfo ����Ʈ�� �ε��� �� ������ ģ���� ������
    // �̱������� �ø��� �ȵǳ�..
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();

    public GameObject bannerFactory;
    GameObject banneritem;

    // ���� ������ ���صѱ� -> �̰� �� ������


    public float bannerTime = 5f;
    float currTime = 0;

    public int idx;

    void Start()
    {
        HttpGetAllBookReview();
        // AI ���� �޾ƿ��� �� �ӽ÷�
        // ���� �� ���� å list �޾ƿ� -> ���⼭ (��������) ���� �ð����� ����
        // ���� ����Ʈ�� ��������� �ȵ�
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
            // ���� �ε��� �ϳ� �̱�
            idx = Random.Range(0, myPastBookInfoList.Count);

            if (banneritem != null)
            {
                // ������ �ִ°� ����
                Destroy(banneritem);

                // ���ο�� ����
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

    // Http ��� ���� -----------------------------------------------
    public void HttpGetAllBookReview()
    {
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:8080//v1/records/all";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetAllBookReview;

        // HttpManager ���� ��û
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnCompleteGetAllBookReview(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        // ��� ����
        if (type == 200)
        {
            print("��ż���.���ٸ���");
            // 1. PlayerPref�� key�� jwt, value�� token
            print(jObject);
            //PhotonNetwork.ConnectUsingSettings();
        }
    }

}
