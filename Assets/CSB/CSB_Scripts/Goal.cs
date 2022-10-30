using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 목표 list 의 요소
// 목표 list 를 보여줌, 그날 목표 달성을 완료하면 체크 표시, 달성하지 못하면 비활성화
// 
public class Goal : MonoBehaviour
{
    public Text goalList;

    void Start()
    {
   
    }

    void Update()
    {
        // 만약 목표를 달성했다면 체크 표시됨
    }

    // 챌린지 목표를 받음
    public void SetGoalList(string goal)
    {
        goalList.text = goal;
    }
}
