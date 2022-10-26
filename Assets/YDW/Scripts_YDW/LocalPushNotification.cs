using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.SimpleAndroidNotifications;

public class LocalPushNotification : MonoBehaviour
{
    string tile_Test = "알람 제목";
    string content_Test = "알람 내용";

    string tileforThechallenge = "알람 제목";
    string contentforThechallenge = "알람 제목";

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnApplicationPause(bool pause)
    {
        // 들록된 알람 모두 제거
        NotificationManager.CancelAll();
    if(pause)
        {
            Debug.Log("call NotificationManager");
            // 앱을 잠시 쉴 때 일정시간 이후에 알림 == 일정시간 이후에 알림이 오게끔
            DateTime timeToNotify = DateTime.Now.AddMinutes(1);
            TimeSpan time = timeToNotify - DateTime.Now;
            NotificationManager.SendWithAppIcon(time, tile_Test, content_Test, Color.blue, NotificationIcon.Bell);


            // 앱을 잠시 쉴 때 지정된 시간에 알림 ==  지정된 시간에 알림이 오게끔
            DateTime specifiedTime1 = Convert.ToDateTime("8:00:00 AM");
            TimeSpan sTime1 = specifiedTime1 - DateTime.Now;
            if (sTime1.Ticks > 0) NotificationManager.SendWithAppIcon(sTime1, tileforThechallenge, contentforThechallenge, Color.red, NotificationIcon.Heart);
        }
    }
}
