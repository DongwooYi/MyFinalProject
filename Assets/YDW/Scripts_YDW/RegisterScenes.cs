using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RegisterScenes : MonoBehaviour
{
   
    public void onRegisterBtnClick()
    {
        SceneManager.LoadScene(1);

    }
}
