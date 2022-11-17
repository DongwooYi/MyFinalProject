using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkayButton : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickOk()
    {
        Destroy(gameObject);
        Destroy(GameObject.Find("MyReviewPanel(Clone)"));
        Destroy(GameObject.Find("CurrBookInfoPanel(Clone)"));
    }
}
