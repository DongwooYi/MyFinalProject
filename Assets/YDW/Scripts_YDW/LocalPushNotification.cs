using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.SimpleAndroidNotifications;

public class LocalPushNotification : MonoBehaviour
{
    string tile_Test = "�˶� ����";
    string content_Test = "�˶� ����";

    string tileforThechallenge = "�˶� ����";
    string contentforThechallenge = "�˶� ����";

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnApplicationPause(bool pause)
    {
        // ��ϵ� �˶� ��� ����
        NotificationManager.CancelAll();
    if(pause)
        {
            Debug.Log("call NotificationManager");
            // ���� ��� �� �� �����ð� ���Ŀ� �˸� == �����ð� ���Ŀ� �˸��� ���Բ�
            DateTime timeToNotify = DateTime.Now.AddMinutes(1);
            TimeSpan time = timeToNotify - DateTime.Now;
            NotificationManager.SendWithAppIcon(time, tile_Test, content_Test, Color.blue, NotificationIcon.Bell);


            // ���� ��� �� �� ������ �ð��� �˸� ==  ������ �ð��� �˸��� ���Բ�
            DateTime specifiedTime1 = Convert.ToDateTime("8:00:00 AM");
            TimeSpan sTime1 = specifiedTime1 - DateTime.Now;
            if (sTime1.Ticks > 0) NotificationManager.SendWithAppIcon(sTime1, tileforThechallenge, contentforThechallenge, Color.red, NotificationIcon.Heart);
        }
    }
}
