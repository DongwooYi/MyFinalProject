using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ 3D
public class Reward3DObject : MonoBehaviour
{
    // ���� �� ���Ŀ� �հ��� ��ġ�� ���� �̵�
    // transform.position = �հ���.transform,position

    // ���� ��ġ�� �������(null)�̸� �κ��丮 â�� ������ �־���
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)   // ���� ��ġ�� ������
        {
            // 3D ������Ʈ ��ġ�� �հ����� ��������
            transform.position = Input.GetTouch(0).position;


        }
        // �հ����� ���� (��ġ�� ������ ������)
        else
        {
            // ButtonManager.cs �� rewardList �� �߰�
        }
    }
}
