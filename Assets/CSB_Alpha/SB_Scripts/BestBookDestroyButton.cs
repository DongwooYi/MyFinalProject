using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestBookDestroyButton : MonoBehaviour
{
    GameObject go; 
    void Start()
    {
        go = GameObject.Find("PastBookInfoPanel(Clone)");
    }

    void Update()
    {
        
    }

    public void OnClickButton()
    {
        Destroy(go);
        Destroy(gameObject);
    }
}
