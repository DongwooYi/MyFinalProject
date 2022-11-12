using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonGameManager : MonoBehaviourPunCallbacks
{
    public static PhotonGameManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //OnPhotonSerializeView »£√‚ ∫Ûµµ
        PhotonNetwork.SerializationRate = 60;
        //Rpc »£√‚ ∫Ûµµ
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.Instantiate("Player_PhotonTest_Camera", Vector3.zero , Quaternion.identity);
    }
}
