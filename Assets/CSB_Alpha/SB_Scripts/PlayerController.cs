using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어


public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 플레이어 이동
    public void Move(Vector2 inputDir)
    {
        // 이동 방향키 입력 값 가져오기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");


        // 이동 방향
        Vector3 moveDir = new Vector3(inputDir.x, inputDir.y, 0);

        // 플레이어 이동
        transform.position += moveDir * Time.deltaTime * speed;


    }
}
