using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

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
        // ���� �� ��µǴϱ� List �� ����

        //BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(handler.text);
        // ���� ���
        //bookInfo.title 
        print("���� ���� ���\t" + handler.text);
        string result_items = ParseJson("[" +handler.text + "]", "items");
        string result = ParseJson(result_items, "title", 5
            );
        print(result);
    }


    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);
    }

    string ParseJson(string jsonText, string key)
    {
        JArray parseData = JArray.Parse(jsonText);
        string result = "";

        foreach(JObject obj in parseData.Children())
        {
            result = obj.GetValue(key).ToString(); 
        }

        return result;
    }

    string ParseJson(string jsonText, string key, int childCount)
    {
        JArray parseData = JArray.Parse(jsonText);
        string result = "";

        int index = 0;
        foreach (JObject obj in parseData.Children())
        {
            if (index == childCount)
            {
                result = obj.GetValue(key).ToString();
                break;
            }
            else
            {
                index++;
            }
        }

        return result;
    }
}
