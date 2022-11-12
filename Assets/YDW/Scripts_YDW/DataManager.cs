using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
   public NPC nPC;
    public bool SetActiveMakingRoom;
    public bool ShowRoomlist;
    // Start is called before the first frame update
    void Start()
    {
        SetActiveMakingRoom = false;
        ShowRoomlist = false;
        DontDestroyOnLoad(this);
        if(nPC== null|| nPC.isActiveAndEnabled == false)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(nPC.isTiggerEnter)
        {
            SetActiveMakingRoom = true;
        }
        else if(nPC.isTriggershowRoomList)
        {
            ShowRoomlist = true;
        }
    }
}
