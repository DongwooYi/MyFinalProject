using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 챌린지 정보 담을 프리펩
public class Challenge : MonoBehaviour
{
    public Text challengeTitleTxt; // 챌린지 제목
    public Text periodTxt; // 챌린지 주기
    public Text recruitmentPeriod;  // 모집 기간
    public Text challengePeriod;  // 챌린지 기간
    public Text participantsTxt;   // 인원


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 챌린지 제목
    public void SetTitle(string title)
    {
        challengeTitleTxt.text = title;
    }

    // 주기
    public void SetPeriod(string period)
    {
        periodTxt.text = period;
    }

    // 인원
    public void SetParticipants(string people)
    {
        participantsTxt.text = people;
    }

}
