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
        //OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        //Rpc ȣ�� ��
        PhotonNetwork.SendRate = 60;
        GameObject go = PhotonNetwork.Instantiate("Player", Vector3.zero , Quaternion.identity) as GameObject;
        go.transform.parent = GameObject.Find("Spawner").transform;

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
