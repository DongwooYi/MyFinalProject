using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ 3D
public class Reward3DObject : MonoBehaviour
{
   
    // ���� �� ���Ŀ� �հ��� ��ġ�� ���� �̵�
    // transform.position = �հ���.transform,position

    // ���� ��ġ�� �������(null)�̸� �κ��丮 â�� ������ �־���
    void Start()
    {
      
    }

    void Update()
    {
      
        transform.position = ButtonManager.GetMouseWorldPosition();
                
    }
 
}
