using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingChattingRoom : MonoBehaviour
{
    
    public Animator anim;

    private void Start()
    {
        anim.ResetTrigger("DoorOpen");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            anim.SetTrigger("DoorOpen");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Character")
        {
            anim.ResetTrigger("DoorOpen");

        }
    }
}
