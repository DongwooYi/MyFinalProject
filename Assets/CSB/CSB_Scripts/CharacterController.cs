using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ������
// ���̽�ƽ �Է� ���� ���� �����¿� �̵�
public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move(Vector2 inputDir)
    {
        // �̵� ����Ű �Է� �� ��������
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        Vector2 moveInput = inputDir;

        // �̵� ����
        Vector3 moveDir = inputDir;

        // �̵�
        transform.position += moveDir * Time.deltaTime * speed;

    }
}
