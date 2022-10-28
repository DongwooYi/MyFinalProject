using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    // 스크롤뷰의 content
    public Transform content;

    // 챌린지 생성 공장
    public GameObject challengeFactory;

    // 만들 챌린지 제목
    public InputField inputTitleName;
    // 생성 Button
    public Button btnConnect;

    void Start()
    {
        // inputTitleName 값이 변할 때마다 호출되는 함수 등록, 인스펙터 창 상에 등록하는 것을 코드로 구현
        inputTitleName.onValueChanged.AddListener(OnValueChanged);

/*        // inputTitleName 에서 Enter 키 누르면 호출되는 함수 등록
        inputTitleName.onSubmit.AddListener(OnSubmit);*/

        // inputTitleName 에서 Focusing 이 사라졌을ㄹ 때 호출되는 함수 등록
        inputTitleName.onEndEdit.AddListener(OnEndEdit);

        //periodToggleGroup = GetComponent<ToggleGroup>();
    }

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

    // "개설" 버튼 누르면 개설
    public void OnClickCreateChallenge()
    {
        // 새로운 챌린지가 생성
        // NewChallengeList 에 Btn_Challenge 프리펩 생성
        GameObject go = Instantiate(challengeFactory, content);    // content 자식으로 챌린지들 생성

        // 주기 text 에 periodInfo 넣어줌
        Challenge challenge = go.GetComponent<Challenge>();
        challenge.SetPeriod(periodInfo);
        challenge.SetParticipants(participantInfo);
    }


    /* 주기 토글 관련 */
    bool isSelected;
    public ToggleGroup periodToggleGroup;
    string periodInfo;

    public void PeriodToggle()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<Toggle> toggles = periodToggleGroup.ActiveToggles();
        foreach(Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            periodInfo = toggle.name;
        }
    }

    /* 인원 토글 관련 */

    public ToggleGroup participantToggleGroup;
    string participantInfo;
    public void ParticipantToggle()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<Toggle> toggles = participantToggleGroup.ActiveToggles();
        foreach (Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            participantInfo = toggle.name;
        }
    }


    void Update()
    {
        
    }
}
