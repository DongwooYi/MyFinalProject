using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

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
        me.GetComponent<Button>().onClick.AddListener(ShowClickHereBestBook);
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

    GameObject me;
    Texture texture;
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

            //if (Physics.Raycast(ray, out hitInfo))
            //{
                //if (hitInfo.transform.gameObject.tag == "ClickHere")
                //{
                    myPastBookPanel.SetActive(true);
                    // Ŭ���� ģ���� �ε���(Ű ��)�� �ش��ϴ� ����� isOn ������ value �� change
                    me = EventSystem.current.currentSelectedGameObject;
            // ���� me == BestBook(Clone) �̸� toggles �ε��� ���� ó��
            print("���� �̸���: " + me.name);
            if (me.name.Contains("BestBook"))
            {
                texture = me.transform.GetChild(1).gameObject.GetComponent<RawImage>().texture;

                idx = me.transform.GetSiblingIndex();
                print("���� �ε��� " + idx);

                toggles[idx] = me.GetComponent<Toggle>().isOn;
               
                Debug.Log("toggles[idx] = me.GetComponent<Toggle>().isOn;" + toggles[idx] + ":" + me.GetComponent<Toggle>().isOn);
            }


            foreach(bool data in toggles.Values)
            {
                print("Values" + data);
            }

                    // ��� �����ϸ� 
            print("���� ��� ���");

                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                //}
            //}
        }
    }

    // Ȯ�� ��ư
    // Ŭ���ϸ� toggles dictionary �� values �� true �� ģ����� �λ�å�� ��ϵ�
    public void OnClickSetBestBook()
    {
        print("1111111");
        for(int i = 0; i < toggles.Count; i++)
        {
            // ���� value ���� true ��
            if (toggles.Values.ToList()[i])
            {
                // �ش��ϴ� 
                GameObject bestBook = transform.GetChild(i).gameObject;
                bestBook.SetActive(true);
                bestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
            }
        }
    }
}
