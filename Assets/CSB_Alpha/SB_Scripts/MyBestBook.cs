using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// å�忡 �پ��ִ� �ڵ�
public class MyBestBook : MonoBehaviour
{
    // Toggle ��� ������ List
    public Dictionary<int, bool> toggles = new Dictionary<int, bool>();
    
    public GameObject player;   // �÷��̾�
    public GameObject myPastBookPanel;  // ������ å ��� UI

    public float distance = 1.5f;   // �÷��̾�� ��ü�� �Ÿ�

    public int idx; // ������ BestBook �������� �ε���

    void Start()
    {
        player = GameObject.Find("Character");

    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < distance)
        {
            ShowClickHereBestBook();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ShowClickHereBestBook()
    {
        // �հ��� ���带 ����ش�
        transform.GetChild(0).gameObject.SetActive(true);
        // �հ��� ���� �׻� ī�޶� ����
        transform.GetChild(0).forward = Camera.main.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.tag == "ClickHere")
                {
                    myPastBookPanel.SetActive(true);
                    // ��� �����ϸ� 
                    print("���� ��� ���");

                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    // Ȯ�� ��ư
    // Ŭ���ϸ� �λ�å�� ��ϵ�
    public void OnClickSetBestBook()
    {

    }
}
