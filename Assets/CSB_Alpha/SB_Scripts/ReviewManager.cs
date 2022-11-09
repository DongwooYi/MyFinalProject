using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 나의 책 관련
// 도서 제목(string), 저자(string), 평점(int), 리뷰(string)
public class ReviewManager : MonoBehaviour
{
    [Serializable]
    public class MyBookData // 도서 관련 정보 class
    {
        public string bookTitle;
        public string bookWriter;
        public int bookRating;
        public string bookReview;

    }

    [Serializable]
    public class MyBookDataArray
    {
        public List<MyBookData> myBookData; // 도서 정보 리스트
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
