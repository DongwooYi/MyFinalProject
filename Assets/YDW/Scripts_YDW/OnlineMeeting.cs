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
    private string _token = "007eJxTYNj1OkEg723spaWSE+V9/Tya+SvzFp5YtOroHL5fNTal3csUGCxMDdNSzQ1MTAwMjU0SLVIs05ItkswtDYzMLC0sDCwsrr5pS24IZGRwuSrOysgAgSA+C0NIanEJAwMASmsfOw==";
    private uint remoteUid;
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal VideoSurface RemoteView2nd;
    internal VideoSurface RemoteView3rd;
    internal IRtcEngine RtcEngine;

    public Button btnJoin;
    public Button btnCamOff;
    // Start is called before the first frame update
    void Start()
    {
        
        btnJoin.onClick.AddListener(Join);
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
    }


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
            print(uid);
            if(uid == 4122959556)
            {
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);

            }
            else
            {
                _videoSample.RemoteView2nd.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);

            }
            // Setup remote view.

            // Save the remote user ID in a variable.
            _videoSample.remoteUid = uid;
        }
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.RemoteView.SetEnable(false);
        }
    }
   
    public void SetupUI()
    {
        
         GameObject go = GameObject.Find("MyView");
        LocalView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);

          go = GameObject.Find("RemoteView");
        RemoteView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);


        go = GameObject.Find("RemoteViewTwo");
        RemoteView2nd = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);


       /* go = GameObject.Find("RemoteView3");
        RemoteView2nd = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
*/

        go = GameObject.Find("ButtonLeave");
        go.GetComponent<Button>().onClick.AddListener(Leave);

        
    }

    public List<GameObject> viewImg;
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
        btnJoin.gameObject.SetActive(false);
        btnCamOff.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            viewImg[i].SetActive(false);
        }
    }
    public void CamOff()
    {
        btnJoin.gameObject.SetActive(true);
        btnCamOff.gameObject.SetActive(false);
    }




    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the video modules.
        RtcEngine.DisableVideo();
        // Stops rendering the remote video.
        RemoteView.SetEnable(false);
        // Stops rendering the local video.
        LocalView.SetEnable(false);
          PhotonNetwork.LeaveRoom();
    }
    /* public override void OnLeftRoom()
     {
         base.OnLeftRoom();
         PhotonNetwork.LoadLevel(1);
     }*/
    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

}
