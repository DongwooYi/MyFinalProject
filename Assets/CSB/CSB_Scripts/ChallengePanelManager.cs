using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 찾기
// Trigger 발생시키는 오브젝트 찾기

public class ChallengePanelManager : MonoBehaviour
{
    //public GameObject ingChallengeListPanel;    // 현재 진행중인 챌린지 목록 Panel - 나의 자식
    GameObject player;  // 플레이어
    GameObject ingChallengeObj; // 진행 중 트리거 발생시키는 오브젝트
    GameObject newChallengeObj; // 새 챌린지 트리거 발생시키는 오브젝트


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    // 플레이어 찾기
        ingChallengeObj = GameObject.Find("IngChallenge");  // 진행중 트리거 물체 찾기

    }

    void Update()
    {
        NewChallengeList();
        IngChallengeList();
    }

    // 모집 중인 챌린지 목록 보여줌
    public void NewChallengeList()
    {
        newChallengeObj = GameObject.Find("NewChallenge");  // 물체 찾기
        // 만약 물체와의 거리가 1보다 작으면
        if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1f)
        {
            // <내 챌린지> 오브젝트(쿼드) 가 뜸, setActive
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(true);

            // 만약 물체와의 거리가 0.5 보다 작으면 
            if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 0.5f)
            {
                // 진행 중인 챌린지 목록(UI)이 뜸  (나의 자식오브젝트 중 인덱스 몇 번)
                transform.GetChild(0).gameObject.SetActive(true);
                // 하나를 선택하면 챌린지 월드로 입장
            }

        }
    }

    // 진행 중인 챌린지 목록 보여줌
    public void IngChallengeList()
    {
        ingChallengeObj = GameObject.Find("IngChallenge");  // 진행중 트리거 물체 찾기
        // 만약 물체와의 거리가 1보다 작으면
        if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1f)
        {
            print("들어오니?");
            // <내 챌린지> 오브젝트(쿼드) 가 뜸, setActive
            ingChallengeObj.transform.GetChild(0).gameObject.SetActive(true);

            // 만약 물체와의 거리가 0.5 보다 작으면 
            if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 0.5f)
            {
                // 진행 중인 챌린지 목록(UI)이 뜸  (나의 자식오브젝트 중 인덱스 몇 번)
                transform.GetChild(1).gameObject.SetActive(true);
                // 하나를 선택하면 챌린지 월드로 입장
            }

        }
    }


}
