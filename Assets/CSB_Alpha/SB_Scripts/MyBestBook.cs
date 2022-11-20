using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// å�忡 �پ��ִ� �ڵ�
public class MyBestBook : MonoBehaviour
{
    // Toggle ��� ������ List
    //public Dictionary<GameObject, bool> toggles =

    public GameObject player;   // �÷��̾�


    public WorldManager2D worldManager;


    public GameObject myPastBookPanel;  // ������ å ��� UI

    public float distance = 1.5f;   // �÷��̾�� ��ü�� �Ÿ�

    public Transform content;

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
                    // ���������� �޾ƿ���  // ��Ʈ��ũ ���
                    // �޾ƿ� ���������� �ֱ�
                    //HttpGetPastBook();  // ��Ʈ��ũ ��� -> �Լ� ����������
                    print("�ϵ����� ��� ���");


                    myPastBookPanel.SetActive(true);
                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}
