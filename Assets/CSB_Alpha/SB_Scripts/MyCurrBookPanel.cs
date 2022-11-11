using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// 플레이어가 책상 가까이 가면 현재 읽고 있는 책 UI 가 뜬다
public class MyCurrBookPanel : MonoBehaviour
{
    public GameObject player;   // 플레이어
    public GameObject myDesk;   // 책상
    public GameObject myCurrBookPanel;  // 현재 읽고 있는 책 목록 UI
    public GameObject currBookInfoPanel; // 선택한 책 상세 정보

    public GameObject currBookInfoPanelFactory;

    public Transform canvas;

    public WorldManager2D worldManager;
    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>();

    public float distance = 1f;

    // 아바타 머리에 띄우고 싶은 책 선택 관련

    void Start()
    {
        player = GameObject.FindWithTag("Player");


    }

    void Update()
    {
        // 만약 플레이어가 책상 가까이 가면(거리 1정도)
        if(Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            // 현재 읽고 있는 책 리스트 받아와야함 (계속 받아와야함..? / 현재 읽고 있는 책 열때만..?)
            myCurrBookList = worldManager.myBookList;

            // 버튼에 각 정보들 뿌려줌
            // MyCurrBookPanel 의 자식의 인덱스와 myCurrBookList 의 인덱스 맞춰서 넣어줌

            // 현재 읽고 있는 책 Panel
            myCurrBookPanel.SetActive(true);
        }


    }

    // 현재 읽고 있는 도서 정보 내용 상세보기 함수
    public void OnClickCurrBook()
    {
        // 내가 누구지(나의 정보)
        GameObject me = EventSystem.current.currentSelectedGameObject;

        // 내가 부모(myCurrBookPanel)의 몇번째 자식인지
        int idx = me.transform.GetSiblingIndex();
        print("CurrButtonIdx: " + idx);

        // 나의 인덱스에 해당되는 현재 책 리스트의 정보 뿌리기
        //currBookInfoPanel.SetActive(true);

        // 생성
        GameObject go = Instantiate(currBookInfoPanelFactory, canvas);
    }
}
