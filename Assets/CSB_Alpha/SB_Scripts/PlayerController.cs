using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾�


public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // �÷��̾� �̵�
    public void Move(Vector2 inputDir)
    {
        // �̵� ����Ű �Է� �� ��������
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");


        // �̵� ����
        Vector3 moveDir = new Vector3(inputDir.x, inputDir.y, 0);

        // �÷��̾� �̵�
        transform.position += moveDir * Time.deltaTime * speed;


    }
}
