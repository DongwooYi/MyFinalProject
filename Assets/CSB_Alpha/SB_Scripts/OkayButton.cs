using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkayButton : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        panel = GameObject.Find("MyBookPanel");
    }
    public void OnClickOk()
    {
        Destroy(gameObject);
        Destroy(GameObject.Find("ConfirmMyBook(Clone)"));
        Destroy(GameObject.Find("Already(Clone)"));
    }

    public void OnClickDoneButton()
    {
        Destroy(gameObject);
        panel.SetActive(true);

    }

}
