using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyProfileManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Scene_YDW");
        }
    }
}
