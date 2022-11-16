using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;



// �÷��̾ å�� ������ ���� ���� �а� �ִ� å UI �� ���
public class MyCurrBookPanel : MonoBehaviour
{
    public GameObject player;   // �÷��̾�
    public GameObject myDesk;   // å��
    public GameObject myCurrBookPanel;  // ���� �а� �ִ� å ��� UI
    public GameObject currBookInfoPanel; // ������ å �� ����

    public GameObject currBookInfoPanelFactory;

    public Transform canvas;

    public WorldManager2D worldManager;
    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>();
    //public List<_MyBookInfo> myBookListNet = new List<_MyBookInfo>();


    public float distance = 1.5f;

    // �ƹ�Ÿ �Ӹ��� ���� ���� å ���� ����


    void Start()
    {
        player = GameObject.Find("Character");

        // ���⼭ �� ������ �� �� �о��� å �ѹ� �ѷ��ְ� ����
        //HttpGetPastBookList();

        
    }

    void Update()
    {
        // ���� �÷��̾ å�� ������ ����(�Ÿ� 1����)
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
        // �հ��� ���带 ����ش�
        myDesk.transform.GetChild(0).gameObject.SetActive(true);
        myCurrBookList = worldManager.myBookList;
        //myBookListNet = worldManager.myBookListNet;

        // MyCurrBookPanel �� �ڽ��� �ε����� myCurrBookList �� �ε��� ���缭 �־���
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
                    //HttpGetCurrBook();  // ��Ʈ��ũ ���
                    print("�̹��� �ѹ��� ��������");
                    myCurrBookPanel.SetActive(true);
                    myDesk.transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    public GameObject me;
    public int idx;

    // ���� �а� �ִ� ���� ���� ���� �󼼺��� �Լ�
    public void OnClickCurrBook()
    {
        // ���� ������(���� ����)
        me = EventSystem.current.currentSelectedGameObject;

        // ���� �θ�(myCurrBookPanel)�� ���° �ڽ�����
        idx = me.transform.GetSiblingIndex();
        print("CurrButtonIdx: " + idx);

        // ����
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

    // �ڷ� ���� ��ư
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


    // ��� ���� -------------------------
    #region ���絵��
    void HttpGetCurrBook()
    {
        // ������ �Խù� ��ȸ ��û(/post/1, GET)
        // HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:8080/v1/records/reading";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnComplteGetMyCurrBook;

        // HttpManager ���� ��û
        HttpManager.instance.SendRequest(requester, "");
    }


    public void OnComplteGetMyCurrBook(DownloadHandler handler)
    {

        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];
        //string type = (int)jObject["data"]["recordCode"];
       
        //string result_data = ParseJson("[" + handler.text + "]", "data");

        // ��� ����
        if (type == 200)
        {
            print("��ż���.���絵��");
            // 1. PlayerPref�� key�� jwt, value�� token

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

        // /posts/1. GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        requester.url = "http://15.165.28.206:8080/v1/records/count";
        requester.requestType = RequestType.GET;
        requester.onComplete = OnComplteGetMyPastBookList;

        // HttpManager ���� ��û
        HttpManager.instance.SendRequest(requester, "");
    }

    public void OnComplteGetMyPastBookList(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];
        //string type = (int)jObject["data"]["recordCode"];

        // ��� ����
        if (type == 200)
        {
            print("��ż���.�������� ���");
            // 1. PlayerPref�� key�� jwt, value�� token
            print(jObject);
            //PhotonNetwork.ConnectUsingSettings();
        }
    }

}
