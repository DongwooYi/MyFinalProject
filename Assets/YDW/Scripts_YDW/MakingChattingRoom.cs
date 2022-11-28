using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingChattingRoom : MonoBehaviour
{
    
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            anim.SetTrigger("DoorOpen");
        }
    }
}
