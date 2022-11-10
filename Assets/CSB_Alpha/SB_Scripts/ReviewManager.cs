using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 리뷰 배너 관련
public class ReviewManager : MonoBehaviour
{
    // 이 친구 생성은 어디서 하지
    // 일단은 myPastBookInfo 리스트의 인덱스 젤 마지막 친구를 데려옴
    // 싱글톤으로 올리면 안되낭..
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
