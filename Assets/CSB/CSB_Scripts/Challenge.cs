using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ç���� ���� ���� ������
public class Challenge : MonoBehaviour
{
    public Text challengeTitleTxt; // ç���� ����
    public Text periodTxt; // ç���� �ֱ�
    public Text recruitmentPeriod;  // ���� �Ⱓ
    public Text challengePeriod;  // ç���� �Ⱓ
    public Text participantsTxt;   // �ο�


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ç���� ����
    public void SetTitle(string title)
    {
        challengeTitleTxt.text = title;
    }

    // �ֱ�
    public void SetPeriod(string period)
    {
        periodTxt.text = period;
    }

    // �ο�
    public void SetParticipants(string people)
    {
        participantsTxt.text = people;
    }

}
