using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [Header("Ÿ�̸� �ؽ�Ʈ")]
    public Text hourTime;
    // Ÿ�̸� �׽�Ʈ��
    public Text textTimerTest;
    public Text startdate;

    [Header("Ÿ�̸�")]
    public float totalHourTime;

    [Header("�ð� Bool ��")]
    public bool isTimercheckTrue;
    public bool isCheckStartTimer;

    [Header("���� �ð�")]
    DateTime dateTime;
     DateTime dateTime1;
    private void Start()
    {
        totalHourTime = 0;
    }
    
    private void Update()
    {
        dateTime = DateTime.Now;               
        if (isTimercheckTrue)
        { 
        var time = (float.Parse(hourTime.text));
        totalHourTime = time * 60 *60;
        isCheckStartTimer = true;
        }

        if(isCheckStartTimer)
        {
        isTimercheckTrue = false;
        textTimerTest.text = CountDownTimer();
        }
    }
   public void HandleInputData(int val)
    {
        if(val == 0)
        {
            hourTime.text = "24";
            dateTime1 = dateTime.AddHours(24);
            startdate.text = $"{dateTime1.Year}:{dateTime1.Month}:{dateTime1.Day}";
            print("24");
            isTimercheckTrue = true;
        }
        if(val == 1)
        {
            hourTime.text = "72";
            dateTime1 = dateTime.AddHours(72);
            startdate.text = $"{dateTime1.Year}:{dateTime1.Month}:{dateTime1.Day}";
            print("72");
            isTimercheckTrue = true;
        }
        if (val == 2)
        {
            hourTime.text = "168";
            dateTime1 = dateTime.AddHours(168);
            startdate.text = $"{dateTime1.Year}:{dateTime1.Month}:{dateTime1.Day}";
            print("168");
            isTimercheckTrue = true;
        }
    }
      
        
    string CountDownTimer()
    {
        totalHourTime -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalHourTime);
        string timer = string.Format("{0:00}:{1:00}{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return timer;
    }
}
