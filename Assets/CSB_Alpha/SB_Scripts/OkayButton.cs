using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkayButton : MonoBehaviour
{

    public void OnClickOk()
    {
        Destroy(gameObject);
        Destroy(GameObject.Find("ConfirmMyBook(Clone)"));
        Destroy(GameObject.Find("Already(Clone)"));
    }

}
