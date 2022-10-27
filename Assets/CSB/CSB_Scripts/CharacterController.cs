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

   // 플레이어 이동
    public void Move(Vector2 inputDir)
    {
        // 이동 방향키 입력 값 가져오기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        // 만약 월드면
        if (enterTheWorld || isChallengeWorld)
        {
            // 이동 방향
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // 플레이어 이동
            transform.position += moveDir * Time.deltaTime * speed;

            // 카메라 이동
            Camera.main.transform.position += moveDir * Time.deltaTime * speed * 0.8f;
        }
    }
}
