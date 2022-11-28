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
    //GameObject worldManager;
   // List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();

    public GameObject bannerFactory;
    GameObject banneritem;

    // ���� ������ ���صѱ� -> �̰� �� ������


    public float bannerTime = 5f;
    float currTime = 0;

    public int idx;

    void Start()
    {
        HttpGetGetOneLineReview();
        // AI ���� �޾ƿ��� �� �ӽ÷�
        // ���� �� ���� å list �޾ƿ� -> ���⼭ (��������) ���� �ð����� ����
        // ���� ����Ʈ�� ��������� �ȵ�
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
            // ���� �ε��� �ϳ� �̱�
            idx = Random.Range(0, myAllBookListNet.Count);

            if (banneritem != null)
            {
                // ������ �ִ°� ����
                Destroy(banneritem);

                // ���ο�� ����
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

    public List<_MyBookInfo> myAllBookListNet = new List<_MyBookInfo>();   // ��������


    // ��Ʈ��ũ���� �޾ƿ� ���� ������ ����Ʈ
    public List<string> titleListNet = new List<string>();
    public List<string> thumbnailLinkListNet = new List<string>();
    public List<string> reviewListNet = new List<string>();
    public List<string> nicknameList = new List<string>();
    public List<Texture> thumbnailImgListNet = new List<Texture>();

    // (�ٲ� ����) Http ��� ���� -------------------------------------------------------
    // 1. ���� ����� ��û�� API : ���� å(å��), �λ�å (���� å��) ���� �����ֱ�
    public void HttpGetGetOneLineReview()
    {            // �� data ����Ʈ�� �ʱ�ȭ
        titleListNet.Clear();
        thumbnailLinkListNet.Clear();
        thumbnailImgListNet.Clear();
        reviewListNet.Clear();
        nicknameList.Clear();

        myAllBookListNet.Clear();
        print("��û");
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:80/v1/records/all";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnCompleteGetOneLineReview;

        // HttpManager ���� ��û
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnCompleteGetOneLineReview(DownloadHandler handler)
    {
        // ������ ó��
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("��ż���. ���");
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
                print("����");
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

    // data ���� key ���� parsing
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
