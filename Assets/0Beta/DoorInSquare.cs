using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class DoorInSquare : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PhotonNetwork.LeaveLobby();
        }
    }
    
}
