using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ǥ list �� ���
// ��ǥ list �� ������, �׳� ��ǥ �޼��� �Ϸ��ϸ� üũ ǥ��, �޼����� ���ϸ� ��Ȱ��ȭ
// 
public class Goal : MonoBehaviour
{
    public Text goalList;
    public bool isPass;

    void Start()
    {
        isPass = false;   
    }

    void Update()
    {
        // ���� ��ǥ�� �޼��ߴٸ� üũ ǥ�õ�
    }

    // ç���� ��ǥ�� ����
    public void SetGoalList(string goal)
    {
        goalList.text = goal;
    }
}
