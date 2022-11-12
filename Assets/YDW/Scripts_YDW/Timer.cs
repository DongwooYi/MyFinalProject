using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
   
    public Text textTimer;
    public InputField hourTime;
    
    public float totalHourTime;

    private void Start()
    {
        totalHourTime = 0;
        hourTime.onEndEdit.AddListener(delegate { LockInput(hourTime); });
       
    }
    public bool isStopRenew = false;
    private void Update()
    {
        if(isStopRenew)
        {
        var time = (float.Parse(hourTime.text));
        totalHourTime = time * 60 *60;
        }

        if(okay)
        {
        textTimer.text = CountDownTimer();
            isStopRenew = false;
        }
        
    }
    public bool okay;
    void LockInput(InputField input)
    {
        if (input.text.Length > 0)
        {
            Debug.Log("Text has been entered");
            okay = true;
            isStopRenew = true;
        }
        else if (input.text.Length == 0)
        {
            Debug.Log("Main Input Empty");
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
