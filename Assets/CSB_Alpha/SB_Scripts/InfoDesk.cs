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
        // �÷��̾���� �Ÿ��� 1f �̳���
        if(Vector3.Distance(player.transform.position, transform.position) < triggerDistance)
        {
            // ���� ���� ���� UI Ȱ��ȭ
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
