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
      
        transform.position = ButtonManager.GetMouseWorldPosition();
                
    }
 
}
