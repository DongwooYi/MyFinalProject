using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// 플레이어 움직임
// 조이스틱 입력 값에 따라 상하좌우 이동
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

   // 플레이어 이동
    public void Move(Vector2 inputDir)
    {
        // 이동 방향키 입력 값 가져오기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        // 만약 월드 입장
        if (enterTheWorld)
        {
            // 이동 방향
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // 이동
            transform.position += moveDir * Time.deltaTime * speed;
        }
    }

/*
    // 진행 중인 챌린지 목록
    
    public void IngChallenge()
    {
        GameObject obj = GameObject.Find("IngChallenge");   // 물체 찾기

        // 빈 게임오브젝트에 text 붙여서/obj의 자식은 무언가 (- 무조건 카메라 방향으로 향하게)

        // 만약 물체와의 거리가 1보다 작으면
        if(Vector3.Distance(transform.position, obj.transform.position) < 1f)
        {
            // <내 챌린지> UI 가 뜸, setActive
            obj.transform.GetChild(0).gameObject.SetActive(true);

            // 만약 물체와의 거리가 0.5 보다 작으면 
            if(Vector3.Distance(transform.position, obj.transform.position) < 0.5f)
            {
                // 진행 중인 챌린지 목록(UI)이 뜸

                // 하나를 선택하면 챌린지 월드로 입장

            }

        }

    }*/
}
