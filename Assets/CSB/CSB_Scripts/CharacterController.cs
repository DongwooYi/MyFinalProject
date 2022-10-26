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

    Rigidbody rb;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //if (SceneManager.GetActiveScene().name == "CSB_MyProfiletoYDW")
            if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            rb.useGravity = false;
        }


    }


    void Update()
    {
       // if (SceneManager.GetActiveScene().name != "CSB_MyProfiletoYDW")
            if(SceneManager.GetActiveScene().buildIndex != 2)

        {
            rb.useGravity = true;
        }
       // if (SceneManager.GetActiveScene().name == "PlaygroundDemo")
            if(SceneManager.GetActiveScene().buildIndex == 3)

        {
            if (!enterTheWorld)
            {
                rb.useGravity = true;
                spawnPosition = GameObject.Find("PlayerSpawnPosition");
                transform.localScale = new Vector3(10, 10, 10);
                transform.position = spawnPosition.transform.position;
                enterTheWorld = true;
            }
            //IngChallenge();
        }

    }

   // �÷��̾� �̵�
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

/*
    // ���� ���� ç���� ���
    
    public void IngChallenge()
    {
        GameObject obj = GameObject.Find("IngChallenge");   // ��ü ã��

        // �� ���ӿ�����Ʈ�� text �ٿ���/obj�� �ڽ��� ���� (- ������ ī�޶� �������� ���ϰ�)

        // ���� ��ü���� �Ÿ��� 1���� ������
        if(Vector3.Distance(transform.position, obj.transform.position) < 1f)
        {
            // <�� ç����> UI �� ��, setActive
            obj.transform.GetChild(0).gameObject.SetActive(true);

            // ���� ��ü���� �Ÿ��� 0.5 ���� ������ 
            if(Vector3.Distance(transform.position, obj.transform.position) < 0.5f)
            {
                // ���� ���� ç���� ���(UI)�� ��

                // �ϳ��� �����ϸ� ç���� ����� ����

            }

        }

    }*/
}
