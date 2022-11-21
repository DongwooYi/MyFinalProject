using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 생성될 때 MyBestBook.cs 의 toggles 
// 나의 인덱스를 찾아서 넘겨줌
public class PastBookforBest : MonoBehaviour
{
    public Transform bestBookContent;

    MyBestBook bestBook;

    void Start()
    {
        bestBookContent = GameObject.Find("Canvas").transform.Find("BestBookPanel").Find("Scroll View_BestBook").Find("Viewport").Find("Content_Best");
        bestBook = GameObject.Find("myroom/MyBestBookshelf").GetComponent<MyBestBook>();
        //bestBook.idx = FindMyIndex();
        bestBook.toggles.Add(FindMyIndex(), false);
    }

    void Update()
    {
        
    }

    // 나의 인덱스를 찾음
    public int FindMyIndex()
    {
        // 내가 내 부모의 몇번째인지
        int idx = transform.GetSiblingIndex();

        return idx;
    }
}
