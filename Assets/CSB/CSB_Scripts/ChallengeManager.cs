using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public Transform panelManager;
    public GameObject makeNewChallengePanel;

    GameObject player;  // 플레이어
    GameObject ingChallengeObj; // 진행 중 트리거 발생시키는 오브젝트
    GameObject newChallengeObj; // 새 챌린지 트리거 발생시키는 오브젝트



    // 챌린지 목록 content
    public Transform content;

    // 챌린지 생성 공장
    public GameObject challengeFactory;

    // 만들 챌린지 제목
    public InputField inputTitleName;
    // 생성 Button
    public Button btnConnect;

   public PeriodToggle periodToggle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    // 플레이어 찾기
        newChallengeObj = GameObject.Find("NewChallenge");  // 물체 찾기
        ingChallengeObj = GameObject.Find("IngChallenge");  // 진행중 트리거 물체 찾기

        /* 새 챌린지 생성 관련 */
        // inputTitleName 값이 변할 때마다 호출되는 함수 등록, 인스펙터 창 상에 등록하는 것을 코드로 구현
        inputTitleName.onValueChanged.AddListener(OnValueChanged);

/*        // inputTitleName 에서 Enter 키 누르면 호출되는 함수 등록
        inputTitleName.onSubmit.AddListener(OnSubmit);*/

        // inputTitleName 에서 Focusing 이 사라졌을ㄹ 때 호출되는 함수 등록
        inputTitleName.onEndEdit.AddListener(OnEndEdit);

        periodToggle.GetComponent<PeriodToggle>();
    }

    void Update()
    {
        NewChallengeList();
        IngChallengeList();
    }


    /* 새 챌린지 생성 관련 */
    // "개설" 버튼 활성화
    void OnValueChanged(string s)
    {
        // 만약 s의 길이가 0보다 크면
        // 버튼을 동작하게 설정
        // 그렇지 않다면
        // 버튼을 동작하지 않게 설정
        btnConnect.interactable = s.Length > 0;
    }

    // 포커싱이 사라졌을 때
    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);

    }
    public Toggle toggle;
    public CalendarController calendarController_1;
    public CalendarController calendarController_2;
    public CalendarController calendarController_3;
    public CalendarController calendarController_4;

    // "개설" 버튼 누르면 개설
    public void OnClickCreateChallenge()
    {
        // 새로운 챌린지가 생성
        // NewChallengeList 에 Btn_Challenge 프리펩 생성
        GameObject go = Instantiate(challengeFactory, content);    // content 자식으로 챌린지들 생성

        // 주기 text 에 periodInfo 넣어줌
        Challenge challenge = go.GetComponent<Challenge>();
        challenge.SetTitle(inputTitleName.text);    // 제목
        challenge.SetPeriod(periodToggle.a);    // 주기 및 일정
        challenge.SetParticipants("(1/" + toggle.participantInfo[0] + ")");  // 참가 인원
        challenge.SetRePeriod(calendarController_1._target.text + "~" + calendarController_2._target2.text);    // 모집 기간
        challenge.SetChallPeriod(calendarController_3._target3.text + "~" + calendarController_4._target4.text); // 챌린지 기간

        makeNewChallengePanel.SetActive(false);

        // 내용도 지워줘야함
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
                panelManager.GetChild(0).gameObject.SetActive(true);
                // 하나를 선택하면 챌린지 월드로 입장(?)
                // 챌린지 참가 신청
            }
            else
            {
                panelManager.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // 모집 중인 챌린지의 세부 내용 확인
    // 챌린지 목록의 챌린지를 누르면 

    public Transform challengeInfo;
    public void ShowGoalList()
    {
        // 버튼의 이름
        string btn = EventSystem.current.currentSelectedGameObject.name;

        // 버튼의 인덱스 찾기
        int idx = content.FindChild(btn).GetSiblingIndex();

        // 그 인덱스에 해당하는 ChallengeInfoManager 게임오브젝트의 자식 켜주기
        challengeInfo.GetChild(idx).gameObject.SetActive(true);

    }

    // 새 챌린지 생성
    public void OpenNewChallenge()
    {
        // 새 챌린지 생성 UI 띄우기
    }

    // 챌린지 참가 버튼을 누르면 챌린지가 <나의 챌린지>에 생성
    public void JoinNewChallenge()
    {

    }



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
                panelManager.GetChild(3).gameObject.SetActive(true);
                // 하나를 선택하면 챌린지 월드로 입장
                // 목록의 버튼
            }
            else
            {
                panelManager.GetChild(3).gameObject.SetActive(false);
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
        SceneManager.LoadScene("ChallengeWorld");

    }



}
