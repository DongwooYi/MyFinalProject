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
        i = 0;
        //OnPhotonSerializeView È£Ãâ ºóµµ
        PhotonNetwork.SerializationRate = 60;
        //Rpc È£Ãâ ºóµµ
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
  
    private void Update()
    {
        if(NPC.isShowRoomList)
        {
            LeaveRoom();
        }
    }
    int i = 0;
    public void LeaveRoom()
    {
        if(i>0)
        {
            return;
        }
        PhotonNetwork.LeaveRoom(true);
        i ++;
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("MyRoomScene_Beta");
        base.OnLeftRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    
}
