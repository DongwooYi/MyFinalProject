using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��� ����
public class ReviewManager : MonoBehaviour
{
    // �� ģ�� ������ ��� ����
    // �ϴ��� myPastBookInfo ����Ʈ�� �ε��� �� ������ ģ���� ������
    // �̱������� �ø��� �ȵǳ�..
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();

    public GameObject title;
    public GameObject nickname;
    public GameObject oneLineReview;
    public GameObject thumbnail;

    void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;

    }

    void Update()
    {
        
    }
}
