using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDesk : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float triggerDistance = 1f;
    [SerializeField]
    private GameObject challengeList;

    void Start()
    {
        
    }

    void Update()
    {
        // 플레이어와의 거리가 1f 이내면
        if(Vector3.Distance(player.transform.position, transform.position) < triggerDistance)
        {
            // 독서 모임 개설 UI 활성화
            challengeList.SetActive(true);
        }
    }
}


class ChallengeInfo
{
    public string bookTitle;
    public string date;
    public int people;
    public string rules;
}
