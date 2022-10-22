using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ������
// ���̽�ƽ �Է� ���� ���� �����¿� �̵�
public class CharacterController : MonoBehaviour
{
    public float speed = 5f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
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

        // �̵� ����
        Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

        // �̵�
        transform.position += moveDir * Time.deltaTime * speed;

    }
}
