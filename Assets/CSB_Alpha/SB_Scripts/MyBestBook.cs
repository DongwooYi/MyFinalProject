using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

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
        me.GetComponent<Button>().onClick.AddListener(ShowClickHereBestBook);
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

    GameObject me;
    Texture texture;
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

            //if (Physics.Raycast(ray, out hitInfo))
            //{
                //if (hitInfo.transform.gameObject.tag == "ClickHere")
                //{
                    myPastBookPanel.SetActive(true);
                    // 클릭된 친구의 인덱스(키 값)에 해당하는 토글의 isOn 값으로 value 값 change
                    me = EventSystem.current.currentSelectedGameObject;
            // 만약 me == BestBook(Clone) 이면 toggles 인덱스 관련 처리
            print("나의 이름은: " + me.name);
            if (me.name.Contains("BestBook"))
            {
                texture = me.transform.GetChild(1).gameObject.GetComponent<RawImage>().texture;

                idx = me.transform.GetSiblingIndex();
                print("나의 인덱스 " + idx);

                toggles[idx] = me.GetComponent<Toggle>().isOn;
               
                Debug.Log("toggles[idx] = me.GetComponent<Toggle>().isOn;" + toggles[idx] + ":" + me.GetComponent<Toggle>().isOn);
            }


            foreach(bool data in toggles.Values)
            {
                print("Values" + data);
            }

                    // 토글 선택하면 
            print("도서 목록 출력");

                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                //}
            //}
        }
    }

    // 확인 버튼
    // 클릭하면 toggles dictionary 의 values 가 true 인 친구들로 인생책이 등록됨
    public void OnClickSetBestBook()
    {
        print("1111111");
        for(int i = 0; i < toggles.Count; i++)
        {
            // 만약 value 값이 true 면
            if (toggles.Values.ToList()[i])
            {
                // 해당하는 
                GameObject bestBook = transform.GetChild(i).gameObject;
                bestBook.SetActive(true);
                bestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
            }
        }
    }
}
