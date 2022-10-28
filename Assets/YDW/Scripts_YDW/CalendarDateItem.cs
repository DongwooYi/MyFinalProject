using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour {

    public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }
    
    public void OnDateItemClick2()
    {
        CalendarController._calendarInstance.OnDateItemClick2(gameObject.GetComponentInChildren<Text>().text);
    }

    public void OnDateItemClick3()
    {
        CalendarController._calendarInstance.OnDateItemClick3(gameObject.GetComponentInChildren<Text>().text);
    }

    public void OnDateItemClick4()
    {
        CalendarController._calendarInstance.OnDateItemClick4(gameObject.GetComponentInChildren<Text>().text);
    }
}
