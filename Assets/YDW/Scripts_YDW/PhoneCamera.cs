using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using System.Drawing;
using Unity.VisualScripting;
using System.Net;
public class PhoneCamera : MonoBehaviour
{

    bool cameraAvailable;
    WebCamTexture backCam;

    public RawImage background;


    challenges challenges;

    public GameObject Spinner;

    private void Start()
    {
        challenges = GetComponent<challenges>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            challenges.ChallengesList.SetActive(true);

        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            challenges.ChallengesList.SetActive(false);

        }
        if (!cameraAvailable)
        {
            return;
        }
       
     //   int orient = -backCam.videoRotationAngle;
       //     background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        
    }
    public void Get()
    {
        StartCoroutine(Getrequest("http://192.168.0.15:5005/detection"));
    }

    public void CameraOn()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("카메라 없음");
            cameraAvailable = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            //if (devices[i].isFrontFacing) // 정면 카메라
            if (!devices[i].isFrontFacing) // 후면 카메라
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                
                print($"캠 너비: {backCam.requestedWidth}, 캠 높이: {backCam.requestedHeight}");
            }
        }

        if (backCam == null)
        {
            Debug.Log("후면 카메라 찾을 수없음");
            return;
        }
        //backCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        backCam.Play();
        background.texture = backCam;
        cameraAvailable = true;
    }
         
    public void CameraOff()
    {
        backCam.Stop();
        cameraAvailable = false;
    }
    public Text obj;
    public void TakeSnap()
    {

        //yield return new WaitForEndOfFrame();
        Spinner.SetActive(true);

        int width = backCam.requestedWidth;
        int height = backCam.requestedHeight;
        Texture2D snap = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // 스크린 샷 정보를 텍스쳐 형식으로 
        RenderTexture saveTex = RenderTexture.GetTemporary(width, height, 32);
        
        Graphics.Blit(background.texture, saveTex);

        RenderTexture.active = saveTex;
        snap.ReadPixels(new Rect(0, 0, height, width), 0, 0);
        //snap.SetPixels(backCam.GetPixels());
        snap.Apply();

        byte[] bytes = snap.EncodeToPNG();
        //print(bytes.Length);

        File.WriteAllBytes(Application.dataPath + "/Data/photo.png", bytes);

        WWWForm form = new WWWForm();
        //form.AddBinaryData("image", bytes);
        form.AddBinaryData("image", bytes, "/Data/photo.png");



        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.15:5005/detection", form);
       
        // yield return www.SendWebRequest();
         www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Spinner.SetActive(false);

            Debug.Log(www.error);
        }
        else
        {
           
            Spinner.SetActive(false);
            Debug.Log("Form upload complete!");
        }
        //rt.Release();
        saveTex.Release();
    }

   IEnumerator Getrequest(string URL)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
        {
            yield return webRequest.SendWebRequest();
        if ( webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error" + webRequest.error);
            }else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        { 
        }

        }
    }

}

