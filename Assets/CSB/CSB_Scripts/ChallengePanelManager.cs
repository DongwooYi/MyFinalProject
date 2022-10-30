using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        newChallengeObj = GameObject.Find("NewChallenge");  // 물체 찾기
        ingChallengeObj = GameObject.Find("IngChallenge");  // 진행중 트리거 물체 찾기

    }

    void Update()
    {
        NewChallengeList();
        IngChallengeList();

    }


    /* 챌린지 참가 관련 */

    // 모집 중인 챌린지 목록 보여줌
    public void NewChallengeList()
    {
        // 만약 물체와의 거리가 1.5보다 작으면
        if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1.5f)
        {
            // <챌린지 참가> 오브젝트(쿼드) 가 뜸, setActive
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(true);
            //newChallengeObj.transform.GetChild(0).LookAt(Camera.main.transform);    // 카메라 방향을 향하도록

            // 만약 물체와의 거리가 1 보다 작으면 
            if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1f)
            {
                newChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
                // 참가할 수 있는 챌린지 목록(UI)이 뜸  (나의 자식오브젝트 중 인덱스 몇 번)
                transform.GetChild(0).gameObject.SetActive(true);
                // 하나를 선택하면 챌린지 월드로 입장(?)
                // 챌린지 참가 신청
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // 모집 중인 챌린지의 세부 내용 확인
    public void ShowGoalList()
    {

    }

    // 새 챌린지 생성
    public void OpenNewChallenge()
    {
        // 새 챌린지 생성 UI 띄우기
    }

    // 챌린지 참가 



    /* 진행 중인 챌린지(마이챌린지) 관련 */
    // 진행 중인 챌린지 목록 보여줌
    public void IngChallengeList()
    {
        // 만약 물체와의 거리가 1.5보다 작으면
        if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1.5f)
        {
            // <내 챌린지> 오브젝트(쿼드) 가 뜸, setActive
            ingChallengeObj.transform.GetChild(0).gameObject.SetActive(true);
            // 주변에 이펙트

            // 만약 물체와의 거리가 1 보다 작으면 
            if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1f)
            {
                ingChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
                //ingChallengeObj.transform.GetChild(0).LookAt(Camera.main.transform);    // 카메라 방향을 향하도록


                // 진행 중인 챌린지 목록(UI)이 뜸  (나의 자식오브젝트 중 인덱스 몇 번)
                transform.GetChild(3).gameObject.SetActive(true);
                // 하나를 선택하면 챌린지 월드로 입장
                // 목록의 버튼
            }
            else
            {
                transform.GetChild(3).gameObject.SetActive(false);
            }
        }
        else
        {
            ingChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void GotoMyChallengeWorld()
    {
        ingChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        // 챌린지 월드 입장
        SceneManager.LoadScene("ChallengeWorld_YDW");

    }


}
