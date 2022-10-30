using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        newChallengeObj = GameObject.Find("NewChallenge");  // ��ü ã��
        ingChallengeObj = GameObject.Find("IngChallenge");  // ������ Ʈ���� ��ü ã��

    }

    void Update()
    {
        NewChallengeList();
        IngChallengeList();

    }


    /* ç���� ���� ���� */

    // ���� ���� ç���� ��� ������
    public void NewChallengeList()
    {
        // ���� ��ü���� �Ÿ��� 1.5���� ������
        if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1.5f)
        {
            // <ç���� ����> ������Ʈ(����) �� ��, setActive
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(true);
            //newChallengeObj.transform.GetChild(0).LookAt(Camera.main.transform);    // ī�޶� ������ ���ϵ���

            // ���� ��ü���� �Ÿ��� 1 ���� ������ 
            if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1f)
            {
                newChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
                // ������ �� �ִ� ç���� ���(UI)�� ��  (���� �ڽĿ�����Ʈ �� �ε��� �� ��)
                transform.GetChild(0).gameObject.SetActive(true);
                // �ϳ��� �����ϸ� ç���� ����� ����(?)
                // ç���� ���� ��û
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // ���� ���� ç������ ���� ���� Ȯ��
    public void ShowGoalList()
    {

    }

    // �� ç���� ����
    public void OpenNewChallenge()
    {
        // �� ç���� ���� UI ����
    }

    // ç���� ���� 



    /* ���� ���� ç����(����ç����) ���� */
    // ���� ���� ç���� ��� ������
    public void IngChallengeList()
    {
        // ���� ��ü���� �Ÿ��� 1.5���� ������
        if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1.5f)
        {
            // <�� ç����> ������Ʈ(����) �� ��, setActive
            ingChallengeObj.transform.GetChild(0).gameObject.SetActive(true);
            // �ֺ��� ����Ʈ

            // ���� ��ü���� �Ÿ��� 1 ���� ������ 
            if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1f)
            {
                ingChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
                //ingChallengeObj.transform.GetChild(0).LookAt(Camera.main.transform);    // ī�޶� ������ ���ϵ���


                // ���� ���� ç���� ���(UI)�� ��  (���� �ڽĿ�����Ʈ �� �ε��� �� ��)
                transform.GetChild(3).gameObject.SetActive(true);
                // �ϳ��� �����ϸ� ç���� ����� ����
                // ����� ��ư
            }
            else
            {
                transform.GetChild(3).gameObject.SetActive(false);
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
        // ç���� ���� ����
        SceneManager.LoadScene("ChallengeWorld_YDW");

    }


}
