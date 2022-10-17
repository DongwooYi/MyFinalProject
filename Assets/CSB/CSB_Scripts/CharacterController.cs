using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 움직임
// 조이스틱 입력 값에 따라 상하좌우 이동
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
        // 이동 방향키 입력 값 가져오기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        Vector2 moveInput = inputDir;

        // 이동 방향
        Vector3 moveDir = inputDir;

        // 이동
        transform.position += moveDir * Time.deltaTime * speed;

    }
}
