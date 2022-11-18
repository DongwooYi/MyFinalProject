using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyBestBook : MonoBehaviour
{
    public GameObject player;   // 플레이어
    public GameObject myBestBookshelf;
    public GameObject pastBookFactory; // 다읽은도서 상세 내용


    public WorldManager2D worldManager;
    List<_MyPastBookInfo> myPastBookList = new List<_MyPastBookInfo>(); // 다읽은도서

    public GameObject myPastBookPanel;  // 다읽은 책 목록 UI

    public float distance = 1.5f;   // 플레이어와 물체의 거리

    public Transform content;

    void Start()
    {
        player = GameObject.Find("Character");

    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < distance)
        {
            ShowClickHereBestBook();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ShowClickHereBestBook()
    {
        // 손가락 쿼드를 띄워준다
        transform.GetChild(0).gameObject.SetActive(true);
        // 손가락 쿼드 항상 카메라 방향
        transform.GetChild(0).forward = Camera.main.transform.forward;

        myPastBookList = worldManager.myPastBookList;   // 다읽은도서 목록 가져오기

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.tag == "ClickHere")
                {
                    // 다읽은도서 받아오기  // 네트워크 통신
                    // 받아온 다읽은도서 넣기
                    //HttpGetPastBook();  // 네트워크 통신 -> 함수 만들어줘야함
                    print("완독도서 목록 출력");

                    // 자식이 있다면 삭제
                    Transform[] childList = content.GetComponentsInChildren<Transform>();
                    if (childList != null)
                    {
                        for (int i = 1; i < childList.Length; i++)
                        {
                            Destroy(childList[i].gameObject);
                        }
                    }

                    // myPastBookList 의 크기만큼 프리펩 생성
                    for (int i = 0; i < myPastBookList.Count; i++)
                    {
                        GameObject go = Instantiate(pastBookFactory, content);
                        // 얘의 RawImage 의 Texture 를 리스트 순서대로
                        go.GetComponent<RawImage>().texture = myPastBookList[i].thumbnail.texture;
                        PastBook pastBook = go.GetComponent<PastBook>();

                        pastBook.thumbnail.texture = myPastBookList[i].thumbnail.texture;
                        pastBook.bookTitle = myPastBookList[i].bookName;
                        pastBook.bookAuthor = myPastBookList[i].bookAuthor;
                        pastBook.bookInfo = myPastBookList[i].bookPublishInfo;
                        pastBook.bookIsbn = myPastBookList[i].bookISBN;
                        pastBook.bookRating = myPastBookList[i].rating;
                        pastBook.bookReview = myPastBookList[i].review;
                    }
                    myPastBookPanel.SetActive(true);
                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}
