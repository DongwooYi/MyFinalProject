using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// å ���� �Է� ������ Request url�� title �־ requests
[Serializable]
public class BookInfo   // �޾ƿ��� å����
{
    //public 
/*    public string title;    // ����
    //public string contents; // ����
    public string image;  // ����� �̹��� url
    public string isbn;
    public string author;
    public string publisher;
    public string[] translators;*/
}

public class APIRequester : MonoBehaviour
{

    public Action<DownloadHandler> onComplete;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
