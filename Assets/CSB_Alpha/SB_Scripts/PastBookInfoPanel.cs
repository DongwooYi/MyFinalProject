using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;

public class PastBookInfoPanel : MonoBehaviour
{
    public int myIndex;

    public RawImage thumbnail;
    public Texture texture;

    public Text bookTitle;
    public TMP_Text title;

    public Text bookAuthor;
    public Text bookIsbn;
    public Text bookInfo;
    public Text bookRating;
    public Text bookReview;

    public Toggle isBestToggle;
    public bool isBest; // ���� �λ�å�ΰ�
    bool temp;

    public Transform myPastBookPanel;
    public Transform contentDoneBook;

    public GameObject myBookManager;
    public GameObject confirmFactory;
    
    Transform canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        myBookManager = GameObject.Find("MyBookManager");
        myPastBookPanel = GameObject.Find("MyPastBookPanel").transform;///Scroll View_Done/Viewport/Content").transform;
        contentDoneBook = GameObject.Find("Scroll View_Done/Viewport/Content").transform;//")//.transform;///MyPastBookPanel/Scroll View_Done/Viewport/Content").transform;
        isBestToggle.isOn = isBest;
        isBestToggle.onValueChanged.AddListener(OnCheck);

        // BestBookList �� �ѹ� �߰�
        for (int i = 0; i < contentDoneBook.childCount; i++)
        {
            if (contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isBest)
            {
                BestBookList.Add(contentDoneBook.GetChild(i).gameObject);   // �λ����� ����Ʈ�� �߰�
            }
        }

    }

    public bool tempMyBook;
    public bool isBestMyBook;
    public void OnCheck(bool checkBool)
    {
        temp = isBest;  // ���� �� ����
        isBest = checkBool; // ���� ��
        // ���� �ε����� �ش��ϴ� å�� isBest �ٲٰ�
        //tempMyBook = contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBestTemp;
        //isBestMyBook = contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBest;
        contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBestTemp = temp;
        contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBest = isBest;
        //tempMyBook = temp;
        //isBestMyBook = isBest;
        print(temp);
        print(checkBool);
        print(isBest);
    
            OnClickSetBook();
       
    }

    public List<GameObject> BestBookList = new List<GameObject>();
    public List<BestBook> httpList = new List<BestBook>();

    string isBestStr;
    public void OnClickSetBook()
    {
        // ��
        // isBest == true �� BestBookList �� �߰�
        // BestBookList.Count > 3 �̸� ����Ʈ �� �� �� ����

        if (isBest)
        {
            BestBookList.Add(contentDoneBook.GetChild(myIndex).gameObject);
            if (BestBookList.Count > 3) BestBookList.RemoveAt(0);
        }

/*        for (int i = 0; i < contentDoneBook.childCount; i++)
        {
            // isBest == true �� å�� ����Ʈ�� �߰�
            if (contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isBest)
            {
                if (BestBookList.Count > 3)
                {
                    // ���� �� ���� ����
                    BestBookList.RemoveAt(0);
                }
                // ����Ʈ�� �߰�
                BestBookList.Add(contentDoneBook.GetChild(i).gameObject);

            }
        }*/



        // ���� temp �� isBest �� �ٸ��ٸ� ������ ����Ʈ�� ���
        if (temp != isBest)
        {
            BestBook bestBook = new BestBook();
            bestBook.bookISBN = bookIsbn.text;
            if (isBest) isBestStr = "Y";
            else isBestStr = "N";
            bestBook.isBest = isBestStr;

            httpList.Add(bestBook);
            print("�λ�å: " + bestBook);
            print(bestBook.bookISBN + bestBook.isBest);

            HttpPostMyBestBook();

            if (isBest)
            {        
                // �λ�å���� ��ϵǾ����ϴ� UI ����
                GameObject go = Instantiate(confirmFactory, canvas);
            }
        }


        // MyBookManager.cs �� bestBookList �� �� �����
        myBookManager.GetComponent<MyBookManager>().bestBookList = BestBookList;
        //Destroy(gameObject);
    }

    // (�ٲ� ����) Http ��� ���� ----------------------------------
    // 5. å�� -> Post��û (üũ�� ��ȭ�� �ִ� �͵��� ���� ������ �� �� �����ϴ�.)
    // ���) ȣ�� �Ϸ� �� 1�� ȣ�� �ٽ� ����� ��.
    void HttpPostMyBestBook()
    {
        //������ �Խù� ��ȸ ��û
        //HttpRequester�� ����
        HttpRequester requester = new HttpRequester();

        requester.url = "http://15.165.28.206:80/v1/records/best";
        requester.requestType = RequestType.POST;

        BestBookData bookData = new()
        {
            recordDTOList = httpList,
        };
        print(bookData);
        requester.body = JsonUtility.ToJson(bookData, true);
        print(requester.body);
        requester.onComplete = OnCompletePostMyBestBook;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester, "application/json");
    }

    void OnCompletePostMyBestBook(DownloadHandler handler)
    {
        JObject jObject = JObject.Parse(handler.text);

        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("�λ�å ��� �Ϸ�");
        }
    }
    public void OnClickExit()
    {
        Destroy(gameObject);
    }

    public void OnClickConfirmBestBook()
    {
        Destroy(gameObject);
    }

    #region �ؽ�Ʈ ���� ����

    public void SetMyIndex(int num)
    {
        myIndex = num;  // �ٽ� ���� �Ѱ��ֱ� ����
    }

    public void SetTitle(string s)
    {
        bookTitle.text = s;
    }

    public void SetAuthor(string s)
    {
        bookAuthor.text = s;
    }

    public void SetIsbn(string s)
    {
        bookIsbn.text = s;
    }

    public void SetInfo(string s)
    {
        bookInfo.text = s;
    }

    public void SetRating(string s)
    {
        bookRating.text = s;
    }

    public void SetReview(string s)
    {
        title.text = s;
    }

    public void SetThumbnail(Texture texture)
    {
        thumbnail.texture = texture;
    }

    public void SetBestBook(bool best)
    {
        isBest = best;
    }
    #endregion
}
