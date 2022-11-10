using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MyReviewPanel : MonoBehaviour
{
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();

    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;

    public Dropdown dropdown;

    public InputField inputFieldReview; // ���� �Է� ĭ
    public Button btnEnter; // ����ϱ� ��ư

    // ��ϵ� �ȳ� �޽��� ����
    public GameObject alarmFactory;

    void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;

        inputFieldReview.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string s)
    {
        btnEnter.interactable = s.Length > 0;  // ��� ��ư Ȱ��ȭ
    }

    // ��� ��ư (������ <������ å���>�� �߰�)
    public void OnClickAddPastBook()
    {
        _MyPastBookInfo myPastBookInfo = new _MyPastBookInfo();

        myPastBookInfo.title = title.text;
        myPastBookInfo.author = author.text;
        myPastBookInfo.publishInfo = publishInfo.text;
        myPastBookInfo.isbn = isbn.text;
        myPastBookInfo.thumbnail = thumbnail;
        myPastBookInfo.isDone = true;
        myPastBookInfo.rating = dropdown.captionText.text;
        myPastBookInfo.review = inputFieldReview.text;

        // <������å���> �� �߰�
        myPastBookInfoList.Add(myPastBookInfo);

        // å�忡 �ֱ�

        // <��� �Ǿ����ϴ�>
        GameObject go = Instantiate(alarmFactory, gameObject.transform);    // ���� �ڽ����� ����

    }

    // ������ ��ư (������ ������� ����)
    public void OnClickExit()
    {
        // �ۼ��� ����� ���� �ʱ�ȭ... �� �ʿ䰡 ����
        // �� �ڽ� �ʱ�ȭ
        Destroy(gameObject);
    }

    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetAuthor(string s)
    {
        author.text = s;
    }
    
    public void SetPublishInfo(string s)
    {
        publishInfo.text = s;
    }

    public void SetIsbn(string s)
    {
        isbn.text = s;
    }

    public void SetImage(Texture texture)
    {
        thumbnail.texture = texture;
    }
}
