using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDesk : MonoBehaviour
{
    public GameObject player;   // �÷��̾�

    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            print("�ѹ��� ���;��ϴµ�");

        }
    }
}
