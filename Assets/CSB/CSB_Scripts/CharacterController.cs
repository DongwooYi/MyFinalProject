using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// �÷��̾� ������
// ���̽�ƽ �Է� ���� ���� �����¿� �̵�
public class CharacterController : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody rb;
    public GameObject spawnPosition;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (SceneManager.GetActiveScene().name == "CSB_MyProfile")
        {
            rb.useGravity = false;
        }


    }
    public bool enterTheWorld = false;


    void Update()
    {
        if (SceneManager.GetActiveScene().name != "CSB_MyProfile")
        {
            rb.useGravity = true;
        }
        if (SceneManager.GetActiveScene().name == "PlaygroundDemo")
        {
            if (!enterTheWorld)
            {
                rb.useGravity = true;
                spawnPosition = GameObject.Find("PlayerSpawnPosition");
                transform.localScale = new Vector3(10, 10, 10);
                transform.position = spawnPosition.transform.position;
                enterTheWorld = true;
            }
        }
    }

    public void Move(Vector2 inputDir)
    {
        // �̵� ����Ű �Է� �� ��������
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        // ���� ���� ����
        if (enterTheWorld)
        {
            // �̵� ����
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // �̵�
            transform.position += moveDir * Time.deltaTime * speed;

        }

    }
}
