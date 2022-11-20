using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 책장에 붙어있는 코드
public class MyBestBook : MonoBehaviour
{
    // Toggle 들로 구성된 List
    //public Dictionary<GameObject, bool> toggles =

    public GameObject player;   // 플레이어


    public WorldManager2D worldManager;


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


                    myPastBookPanel.SetActive(true);
                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}
