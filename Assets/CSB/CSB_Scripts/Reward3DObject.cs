using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 리워드 3D
public class Reward3DObject : MonoBehaviour
{
    // 생성 된 이후에 손가락 터치를 따라 이동
    // transform.position = 손가락.transform,position

    // 만약 터치가 사라지면(null)이면 인벤토리 창에 아이템 넣어줌
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)   // 만약 터치가 들어오면
        {
            // 3D 오브젝트 위치가 손가락을 따르도록
            transform.position = Input.GetTouch(0).position;


        }
        // 손가락을 떼면 (터치가 들어오지 않으면)
        else
        {
            // ButtonManager.cs 의 rewardList 에 추가
        }
    }
}
