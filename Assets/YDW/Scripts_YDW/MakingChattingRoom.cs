using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingChattingRoom : MonoBehaviour
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
            print(other.gameObject.name);
            GotoMainWorld = true;
                   }
    }
}
