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
    private string _token = "007eJxTYNjmNCtlxaWLjW2/JX0v2gZmJcxWyLlVun3n74/rzH12qPxVYLAwNUxLNTcwMTEwNDZJtEixTEu2SDK3NDAys7SwMLCwOODSmtwQyMigUPqImZEBAkF8FoaQ1OISBgYAyb4giw==";
    // A variable to save the remote user uid.
    private uint remoteUid;
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal VideoSurface RemoteViewOne;
    internal VideoSurface RemoteViewTwo;
    internal IRtcEngine RtcEngine;
    public GameObject btnJoin;
    [Header(" ȭ��ķ ��ġ Ȧ��&¦�� ���")]
    public Transform[] transformCamPostioneven;
    public Transform[] transformCamPostionoddNum;

    public Text textRoomName;
    // Start is called before the first frame update
    void Start()
    {
        textRoomName.text = $"{PhotonNetwork.CurrentRoom.Name}";
        btnJoin.SetActive(true);
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
        buttonCamON.onClick.AddListener(BtnCamOn);
        buttonCamOff.onClick.AddListener(BtnCamOff);
        buttonMicON.onClick.AddListener(BtnMicOn);
        buttonMicOff.onClick.AddListener(BtnMicOff);
        buttonCamOff.gameObject.SetActive(true);
        buttonMicOff.gameObject.SetActive(true);
        buttonMicON.gameObject.SetActive(false);
        buttonCamON.gameObject.SetActive(false);
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
    public GameObject myView; //= GameObject.Find("MyView");
    public GameObject remoteView; //= GameObject.Find("RemoteView");
    public GameObject remoteView1st;
    public GameObject remoteView2nd;
    private void SetupUI()
    {
        LocalView = myView.AddComponent<VideoSurface>();

        myView.transform.Rotate(0.0f, 0.0f, -180);
        RemoteView = remoteView.AddComponent<VideoSurface>();

        RemoteView.transform.Rotate(0.0f, 0.0f, -180);
        RemoteViewOne = remoteView1st.AddComponent<VideoSurface>();
        RemoteViewOne.transform.Rotate(0.0f, 0.0f, -180);

        RemoteViewTwo = remoteView2nd.AddComponent<VideoSurface>();
        RemoteViewTwo.transform.Rotate(0.0f, 0.0f, -180);

        btnJoin.GetComponent<Button>().onClick.AddListener(Join);
    }



    public void Join()
    {
        btnJoin.SetActive(false);
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
        myView.SetActive(true);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {

            remoteView.SetActive(true);
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            remoteView1st.SetActive(true);

        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {

            remoteView2nd.SetActive(true);
        }

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


    public Button buttonCamON;
    public Button buttonCamOff;
    public Button buttonMicON;
    public Button buttonMicOff;
    public void BtnCamOn()
    {
        RtcEngine.EnableVideo();
        LocalView.SetEnable(true);
        myView.SetActive(true);
        buttonCamON.gameObject.SetActive(false);
        buttonCamOff.gameObject.SetActive(true);
    }
    public void BtnCamOff()
    {
        RtcEngine.DisableVideo();
        LocalView.SetEnable(false);
        myView.SetActive(false);
        buttonCamON.gameObject.SetActive(true);
        buttonCamOff.gameObject.SetActive(false);

    }
    public Recorder recorder;
    public void BtnMicOn()
    {
        
            recorder.TransmitEnabled = true;
            buttonMicON.gameObject.SetActive(false);
            buttonMicOff.gameObject.SetActive(true);
        

    }
    public void BtnMicOff()
    {
       
            recorder.TransmitEnabled = false;
            buttonMicON.gameObject.SetActive(true);
            buttonMicOff .gameObject.SetActive(false);        
        
    }
}
