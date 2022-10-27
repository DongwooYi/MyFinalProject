using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject pp;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {

        }
    }

        int i = 0;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            i ++;
            if(i<5)
            {
            Instantiate(pp,i * new Vector3(5, 0, 0), Quaternion.identity);

            }
        }
    }
}
