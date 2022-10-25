using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ã��
// Trigger �߻���Ű�� ������Ʈ ã��

public class ChallengePanelManager : MonoBehaviour
{
    //public GameObject ingChallengeListPanel;    // ���� �������� ç���� ��� Panel - ���� �ڽ�
    GameObject player;  // �÷��̾�
    GameObject ingChallengeObj; // ���� �� Ʈ���� �߻���Ű�� ������Ʈ
    GameObject newChallengeObj; // �� ç���� Ʈ���� �߻���Ű�� ������Ʈ


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    // �÷��̾� ã��
        ingChallengeObj = GameObject.Find("IngChallenge");  // ������ Ʈ���� ��ü ã��

    }

    void Update()
    {
        NewChallengeList();
        IngChallengeList();
    }

    // ���� ���� ç���� ��� ������
    public void NewChallengeList()
    {
        newChallengeObj = GameObject.Find("NewChallenge");  // ��ü ã��
        // ���� ��ü���� �Ÿ��� 1���� ������
        if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1f)
        {
            // <�� ç����> ������Ʈ(����) �� ��, setActive
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(true);

            // ���� ��ü���� �Ÿ��� 0.5 ���� ������ 
            if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 0.5f)
            {
                // ���� ���� ç���� ���(UI)�� ��  (���� �ڽĿ�����Ʈ �� �ε��� �� ��)
                transform.GetChild(0).gameObject.SetActive(true);
                // �ϳ��� �����ϸ� ç���� ����� ����
            }

        }
    }

    // ���� ���� ç���� ��� ������
    public void IngChallengeList()
    {
        ingChallengeObj = GameObject.Find("IngChallenge");  // ������ Ʈ���� ��ü ã��
        // ���� ��ü���� �Ÿ��� 1���� ������
        if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1f)
        {
            print("������?");
            // <�� ç����> ������Ʈ(����) �� ��, setActive
            ingChallengeObj.transform.GetChild(0).gameObject.SetActive(true);

            // ���� ��ü���� �Ÿ��� 0.5 ���� ������ 
            if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 0.5f)
            {
                // ���� ���� ç���� ���(UI)�� ��  (���� �ڽĿ�����Ʈ �� �ε��� �� ��)
                transform.GetChild(1).gameObject.SetActive(true);
                // �ϳ��� �����ϸ� ç���� ����� ����
            }

        }
    }


}
