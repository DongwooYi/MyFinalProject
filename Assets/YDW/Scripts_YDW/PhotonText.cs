using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class PhotonText : MonoBehaviourPunCallbacks
{
    public Text ListText;
    public Text RoomInfoText;
    // Start is called before the first frame update
    void Start()
    {
        RoomRenewal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
    }
    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "¸í / " + PhotonNetwork.CurrentRoom.MaxPlayers + "ÃÖ´ë";
    }
}
