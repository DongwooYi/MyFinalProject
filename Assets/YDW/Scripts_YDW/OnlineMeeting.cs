using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using Agora.Rtc;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.Unity;

public class OnlineMeeting : MonoBehaviourPunCallbacks
{
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };

    // Fill in your app ID.
    private string _appID = "851fe70440134a8d9fc8b79026988088";
    // Fill in your channel name.
    private string _channelName = "Test";
    // Fill in the temporary token you obtained from Agora Console.
    private string _token = "007eJxTYPjdfiCG9/22LTbOOqKlSQyzMkpWdgrMrOC+tLxaZf+c9L0KDBamhmmp5gYmJgaGxiaJFimWackWSeaWBkZmlhYWBhYWv2eXJDcEMjK8L17LwsgAgSA+C0NIanEJAwMAFCUe/Q==/EWWx2nu+TUlq0+BwcLUMC3V3MDExMDQ2CTRIsUyLdkiydzSwMjM0sLCwMIiuK0kuSGQkeFBUxkzIwMEgvicDCEZqQoeiUmZJQwMALtMHVs=";
    // A variable to save the remote user uid.
    private uint remoteUid;
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal VideoSurface RemoteViewOne;
    internal VideoSurface RemoteViewTwo;
    internal IRtcEngine RtcEngine;
    [Header(" È­»óÄ· À§Ä¡ È¦¼ö&Â¦¼ö °æ¿ì")]
    public Transform[] transformCamPostioneven;
    public Transform[] transformCamPostionoddNum;

    public Toggle toggleCamOnOff;
    public Toggle toggleMicOnOff;
    // Start is called before the first frame update
    void Start()
    {
        toggleCamOnOff.onValueChanged.AddListener(delegate { BtnCamOnOFfToggleValueChanged(toggleCamOnOff); });
        toggleMicOnOff.onValueChanged.AddListener(delegate { BtnCamOnOFfToggleValueChanged(toggleMicOnOff); });
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPermissions();
    }

    private void CheckPermissions()
    {
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }

    }
    private void SetupVideoSDKEngine()
    {
        // Create an instance of the video SDK.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // Specify the context configuration to initialize the created instance.
        RtcEngineContext context = new RtcEngineContext(_appID, 0,
        CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION,
        AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        // Initialize the instance.
        RtcEngine.Initialize(context);
    }

    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }
    internal class UserEventHandler : IRtcEngineEventHandler
    {

        private readonly OnlineMeeting _videoSample;
        internal UserEventHandler(OnlineMeeting videoSample)
        {
            _videoSample = videoSample;
        }
        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("You joined channel: " + connection.channelId);
        }
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            // Setup remote view.
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // Save the remote user ID in a variable.
            _videoSample.remoteUid = uid;
        }
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.RemoteView.SetEnable(false);
        }
    }

    private void SetupUI()
    {
        GameObject go = GameObject.Find("MyView");
        LocalView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 90.0f);
        go = GameObject.Find("RemoteView");
        RemoteView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 90.0f);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            go = GameObject.Find("RemoteView2");
            go.SetActive(true);
            RemoteViewOne = go.AddComponent<VideoSurface>();
            go.transform.position = transformCamPostionoddNum[0].position;
            go.transform.Rotate(0.0f, 0.0f, 90.0f);
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            go = GameObject.Find("RemoteView2");
            go.SetActive(true);
            RemoteViewOne = go.AddComponent<VideoSurface>();
            go.transform.position = transformCamPostioneven[0].position;
            go.transform.Rotate(0.0f, 0.0f, 90.0f);
            go = GameObject.Find("RemoteView3");
            go.SetActive(true);
            RemoteViewTwo = go.AddComponent<VideoSurface>();
            go.transform.position = transformCamPostioneven[1].position;
            go.transform.Rotate(0.0f, 0.0f, 90.0f);
        }
        go = GameObject.Find("ButtonLeave");
        go.GetComponent<Button>().onClick.AddListener(Leave);
        go = GameObject.Find("ButtonJoin");
        go.GetComponent<Button>().onClick.AddListener(Join);
    }
    public void Join()
    {
        // Enable the video module.
        RtcEngine.EnableVideo();
        // Set the user role as broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Set the local video view.
        LocalView.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
        // Start rendering local video.
        LocalView.SetEnable(true);
        // Join a channel.
        RtcEngine.JoinChannel(_token, _channelName);
    }


    

    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        PhotonNetwork.LeaveRoom();
        // Disable the video modules.
      //  RtcEngine.DisableVideo();
        // Stops rendering the remote video.
      //  RemoteView.SetEnable(false);
        // Stops rendering the local video.
      //  LocalView.SetEnable(false);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel(1);
    }
    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }
    public void BtnCamOnOFfToggleValueChanged(Toggle change)
    {
        if(!toggleCamOnOff.isOn)
        {
            RtcEngine.DisableVideo();
            LocalView.SetEnable(false);
        }
        else
        {
            RtcEngine.EnableVideo();
            LocalView.SetEnable(true);
        }
    }
    public Recorder recorder;
    public void BtnMicOnOFfToggleValueChanged(Toggle change)
    {
        if(photonView.IsMine)
        {            
            if (!toggleCamOnOff.isOn)
            {
                recorder.TransmitEnabled = true;
            }
            else
            {
                recorder.TransmitEnabled = false;

            }
        }
        
    }
}
