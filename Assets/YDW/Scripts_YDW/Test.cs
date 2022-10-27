using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject QuadPrefabs;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        //   Instantiate(QuadPrefabs, new Vector3(i - 45, -45, 1), Quaternion.identity);
        for (int i = 0; i < 10; i++)
        {

            Instantiate(QuadPrefabs,i* new Vector3(1,0,0 ) ,Quaternion.identity);
        }
        
        // QuadPrefabs.transform.position = new Vector3(-45, 0, -45);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(i != 0)
            {
            i++;

            }
        }
    }
}
