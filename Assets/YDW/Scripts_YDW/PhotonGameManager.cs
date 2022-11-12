using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonGameManager : MonoBehaviourPunCallbacks
{
    public static PhotonGameManager instance;

    public GameObject gameObjectChatting;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObjectChatting.SetActive( false);
        //OnPhotonSerializeView »£√‚ ∫Ûµµ
        PhotonNetwork.SerializationRate = 60;
        //Rpc »£√‚ ∫Ûµµ
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.Instantiate("Player", Vector3.zero , Quaternion.identity);
    }
    public void OnClickChatting()
    {
        gameObjectChatting.SetActive(true);
    }
    public void OnclickChattingEnd()
    {
        gameObjectChatting.SetActive(false);
    }
}
