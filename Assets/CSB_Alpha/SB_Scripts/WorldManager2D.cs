using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// å �˻�, å ��� ����

public class WorldManager2D : MonoBehaviour
{
    public InputField inputBookTitleName;
    public Button btnSearch;

    public APIManager manager;

    void Start()
    {
        // å ���� �Է�
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);



    }

    void Update()
    {
        
    }


    void OnValueChanged(string s)
    {
        btnSearch.interactable = s.Length > 0;  // �˻� ��ư Ȱ��ȭ
    }

    // �˻� ��ư ����
    public void OnClickSearchBook()
    {
        // �˻� ��ư�� Ŭ���ϸ� 
        APIRequester requester = new APIRequester();

        requester.onComplete = OnCompleteSearchBook;

        manager.SendRequest(requester);
    }

    // ���� �˻� ��� ���
    public void OnCompleteSearchBook(DownloadHandler handler)
    {
        BookInfo info = JsonUtility.FromJson<BookInfo>(handler.text);
        print("���� ���� ���");
    }


    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
    }

}
