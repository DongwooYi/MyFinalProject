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
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //OnPhotonSerializeView »£√‚ ∫Ûµµ
        PhotonNetwork.SerializationRate = 60;
        //Rpc »£√‚ ∫Ûµµ
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        
    //playerPrefab.GetComponentInChildren<MeshRenderer>().material.mainTexture= 
    }
  
    private void Update()
    {
        if(NPC.isTiggerEnter || NPC.isTriggershowRoomList)
        {
            LeaveRoom();
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.LoadLevel("MyRoomScene_Beta 1");
    }
    
}
