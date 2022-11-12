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

    public float distance = 1.5f;

    // �ƹ�Ÿ �Ӹ��� ���� ���� å ���� ����

    void Start()
    {
        player = GameObject.FindWithTag("Player");


    }

    void Update()
    {
        // ���� �÷��̾ å�� ������ ����(�Ÿ� 1����)
        if(Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            // ���带 ����ش�
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
                print("�ѹ��� ���;��ϴµ�");

            }
        }*/
    // 

    public bool isDoneMyCurrList = false;

    public void OpenMyCurrBookList()
    {
        // �а��ִ�å �޾ƿ���
        HttpGetCurrBook();
        print("�ѹ��� ���Ͷ�");
        if (isDoneMyCurrList)
        {
            return;
        }
    }


    /* 
     *             // ���� �а� �ִ� å ����Ʈ �޾ƿ;��� (��� �޾ƿ;���..? / ���� �а� �ִ� å ������..?)
            myCurrBookList = worldManager.myBookList;

            // ���� �а� �ִ� å Panel
            myCurrBookPanel.SetActive(true);
            //HttpGetCurrBook();
            print("��������");

            // ��ư�� �� ������ �ѷ���
            // MyCurrBookPanel �� �ڽ��� �ε����� myCurrBookList �� �ε��� ���缭 �־���
            for (int i=0; i < myCurrBookList.Count; i++)
            {
                myCurrBookPanel.transform.GetChild(i).GetComponent<RawImage>().texture = myCurrBookList[i].thumbnail.texture;
            }
     */


    // ���� �а� �ִ� ���� ���� ���� �󼼺��� �Լ�
    public void OnClickCurrBook()
    {
        // ���� ������(���� ����)
        GameObject me = EventSystem.current.currentSelectedGameObject;

        // ���� �θ�(myCurrBookPanel)�� ���° �ڽ�����
        int idx = me.transform.GetSiblingIndex();
        print("CurrButtonIdx: " + idx);

        // ����
        GameObject go = Instantiate(currBookInfoPanelFactory, canvas);
    }

    // �ڷ� ���� ��ư
    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }


    // ��� ���� -------------------------
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
       
        // ��� ����
        if (type == 200)
        {
            print("��ż���.���絵��");
            // 1. PlayerPref�� key�� jwt, value�� token
            print(jObject);
            //PhotonNetwork.ConnectUsingSettings();
        }
    }
}
