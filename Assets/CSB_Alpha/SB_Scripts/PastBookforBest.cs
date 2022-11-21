using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ �� MyBestBook.cs �� toggles 
// ���� �ε����� ã�Ƽ� �Ѱ���
public class PastBookforBest : MonoBehaviour
{
    public Transform bestBookContent;

    MyBestBook bestBook;

    void Start()
    {
        bestBookContent = GameObject.Find("Canvas").transform.Find("BestBookPanel").Find("Scroll View_BestBook").Find("Viewport").Find("Content_Best");
        bestBook = GameObject.Find("myroom/MyBestBookshelf").GetComponent<MyBestBook>();
        bestBook.toggleList.Add(false);
    }

    void Update()
    {
        
    }
}
