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
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            print(collider.gameObject.name);
            GotoMainWorld = true;
        }
    }
}