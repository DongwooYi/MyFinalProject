using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// �÷��̾� ������
// ���̽�ƽ �Է� ���� ���� �����¿� �̵�
public class CharacterController : MonoBehaviour
{
    public GameObject spawnPosition;
    public GameObject ingChallengeList;
    
    public float speed = 5f;
    public bool enterTheWorld = false;
    public bool isChallengeWorld = false;

    Rigidbody rb;

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
                //rb.useGravity = true;
                spawnPosition = GameObject.Find("PlayerSpawnPosition");
                transform.localScale = new Vector3(10, 10, 10);
                transform.position = spawnPosition.transform.position;
                enterTheWorld = true;
            }
        }
        if(SceneManager.GetActiveScene().name == "CSB_YDW_Combine")
        {
            if (!isChallengeWorld)
            {
                Vector3 spawnPos = Vector3.zero;
                transform.localScale = new Vector3(10, 10, 10);
                transform.position = spawnPos;
                isChallengeWorld = true;
            }
        }

    }

   // �÷��̾� �̵�
    public void Move(Vector2 inputDir)
    {
        // �̵� ����Ű �Է� �� ��������
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        // ���� �����
        if (enterTheWorld || isChallengeWorld)
        {
            // �̵� ����
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // �÷��̾� �̵�
            transform.position += moveDir * Time.deltaTime * speed;

            // ī�޶� �̵�
            Camera.main.transform.position += moveDir * Time.deltaTime * speed * 0.8f;
        }
    }
}
