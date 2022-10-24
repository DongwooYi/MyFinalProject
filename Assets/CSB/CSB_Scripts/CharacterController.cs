using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// 플레이어 움직임
// 조이스틱 입력 값에 따라 상하좌우 이동
public class CharacterController : MonoBehaviour
{
    public float speed = 5f;

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
    }

    public void Move(Vector2 inputDir)
    {
        // 이동 방향키 입력 값 가져오기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        // 이동 방향
        Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

        // 이동
        transform.position += moveDir * Time.deltaTime * speed;

    }
}
