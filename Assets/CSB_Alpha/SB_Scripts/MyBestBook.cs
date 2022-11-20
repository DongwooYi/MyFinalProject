using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 책장에 붙어있는 코드
public class MyBestBook : MonoBehaviour
{
    // Toggle 들로 구성된 List
    public Dictionary<int, bool> toggles = new Dictionary<int, bool>();
    
    public GameObject player;   // 플레이어
    public GameObject myPastBookPanel;  // 다읽은 책 목록 UI

    public float distance = 1.5f;   // 플레이어와 물체의 거리

    public int idx; // 생성된 BestBook 프리펩의 인덱스

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
                    myPastBookPanel.SetActive(true);
                    // 토글 선택하면 
                    print("도서 목록 출력");

                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    // 확인 버튼
    // 클릭하면 인생책이 등록됨
    public void OnClickSetBestBook()
    {

    }
}
