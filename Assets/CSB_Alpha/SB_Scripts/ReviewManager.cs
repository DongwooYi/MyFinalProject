using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� å ����
// ���� ����(string), ����(string), ����(int), ����(string)
public class ReviewManager : MonoBehaviour
{
    [Serializable]
    public class MyBookData // ���� ���� ���� class
    {
        public string bookTitle;
        public string bookWriter;
        public int bookRating;
        public string bookReview;

    }

    [Serializable]
    public class MyBookDataArray
    {
        public List<MyBookData> myBookData; // ���� ���� ����Ʈ
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
