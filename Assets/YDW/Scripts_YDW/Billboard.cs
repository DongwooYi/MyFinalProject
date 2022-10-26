using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //나의 앞방향을 카메라 앞방향으로 셋팅하자
        transform.forward = Camera.main.transform.forward;
    }
}
