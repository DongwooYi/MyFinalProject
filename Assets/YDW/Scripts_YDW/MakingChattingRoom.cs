using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingChattingRoom : MonoBehaviour
{
   public GameObject lobbyManager;
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            print(collider.gameObject.name);
            lobbyManager.gameObject.GetComponent<LobbyManager>().CreateChatroom();
        }
        else
        {
            return;
        }
    }
}
