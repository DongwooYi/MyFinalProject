using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToworld : MonoBehaviour
{
    public bool GotoMainWorld;

    private void Start()
    {

        GotoMainWorld = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {            
             GotoMainWorld = true;
        }
    }
}
