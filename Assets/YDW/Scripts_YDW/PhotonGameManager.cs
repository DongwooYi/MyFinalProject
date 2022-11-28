using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine.UI;
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
        //OnPhotonSerializeView »£√‚ ∫Ûµµ
        PhotonNetwork.SerializationRate = 60;
        //Rpc »£√‚ ∫Ûµµ
        PhotonNetwork.SendRate = 60;
       GameObject go = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        go = GameObject.FindWithTag("ShowBook");
        go.GetComponent<MeshRenderer>().material.mainTexture = HttpManager.instance.TextureShowBook.texture;
        go.GetComponent<Outline>().OutlineColor = HttpManager.instance.outlineShowBook;
        go.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = HttpManager.instance.TextureShowBook.texture;
        go.transform.GetChild(0).GetComponent<Outline>().OutlineColor = HttpManager.instance.outlineShowBook;
        buttonGoback.onClick.AddListener(LeaveRoom);
    }
    public Button buttonGoback;
    public Recorder recorder;
    public Button buttonMicOn;
    public Button buttonMicOff;
    public void MicOn()
    {
        recorder.TransmitEnabled = true;
        buttonMicOn.gameObject.SetActive(false);
        buttonMicOff.gameObject.SetActive(true);
    }
    public void MicOff()
    {
        recorder.TransmitEnabled = false;
        buttonMicOff.gameObject.SetActive(false);
        buttonMicOn.gameObject.SetActive(true);
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
