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
    public GameObject confirm;
    public GameObject ChallengeList;
    public GameObject btnSanp;
    public bool isConfirm;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        isConfirm = false;
        challenges = GetComponent<challenges>();
    }
    private void Update()
    {
        if (!cameraAvailable)
        {
            return;
        }
    }
    public void CameraOn()
    {
        ChallengeList.SetActive(false);
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
        // backCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        backCam.Play();
        background.texture = backCam;
        cameraAvailable = true;


    }
    public void BtnTakeSnap()
    {
        StopAllCoroutines();
        StartCoroutine(TakeSnap());
    }
    public void CameraOff()
    {
        StopAllCoroutines();
        confirm.SetActive(false);
        SceneManager.LoadScene("ChallengeWorld");
        backCam.Stop();
        cameraAvailable = false;
    }
    IEnumerator TakeSnap()
    {
        yield return new WaitForEndOfFrame();
        Spinner.SetActive(true);
        btnSanp.GetComponent<Button>().interactable = false;
        int width = backCam.requestedWidth;
        int height = backCam.requestedHeight;
        Texture2D snap = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // 스크린 샷 정보를 텍스쳐 형식으로 
        RenderTexture saveTex = RenderTexture.GetTemporary(width, height, 32);

        Graphics.Blit(background.texture, saveTex);

        RenderTexture.active = saveTex;
        snap.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        //snap.SetPixels(backCam.GetPixels());
        snap.Apply();
        
        HttpRequester httpRequester = new HttpRequester();
        httpRequester.url = "http://172.17.80.236:8080/v1/target";
        
        ImageData data = new ImageData();       
        data.imageDatas = snap.EncodeToPNG();
        
        File.WriteAllBytes(Application.dataPath + "/Data/photo.png", data.imageDatas);

        httpRequester.body = JsonUtility.ToJson(data, true);
        print(httpRequester.body);
        httpRequester.requestType = RequestType.POST;
        httpRequester.onComplete = OnCompletedPostImageDate;

        HttpManager.instance.SendRequest(httpRequester, "application/json");
       /* WWWForm form = new WWWForm();
        form.AddBinaryData("file", bytes);
        //form.AddBinaryData("image", bytes, "/Data/photo.png");
        
        UnityWebRequest www = UnityWebRequest.Post("http://172.17.80.236:8080/v1/target", form);
        //byte[] data = System.Text.Encoding.UTF8.GetBytes(formData);
       // www.uploadHandler = new UploadHandlerRaw(data);
        //www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Spinner.SetActive(false);
            Debug.Log(www.error);
        }
        else
        {
            Spinner.SetActive(false);
            Debug.Log("Form upload complete!");

        }*/
        saveTex.Release();
        yield return new WaitForSeconds(3.0f);
        Spinner.SetActive(false);

        //confirm.SetActive(true);
        //confirm.GetComponentInChildren<Text>().text = "다시 인증해주세요";
        btnSanp.GetComponent<Button>().interactable = true;
        isConfirm = true;
        backCam.Stop();
        cameraAvailable = false;
        StopAllCoroutines();
        
    }

    public void OnCompletedPostImageDate(DownloadHandler downloadHandler)
    {
        Debug.Log("Recieved");
       JObject jObject = JObject.Parse(downloadHandler.text);
       bool type = (bool)jObject["results"]["type"];
        if(type)
        {
            confirm.GetComponentInChildren<Text>().text = "인증완료되었습니다.";
            confirm.SetActive(false);
        }
       else
        {
            Debug.Log(downloadHandler.error);
            confirm.SetActive(true);
            confirm.GetComponentInChildren<Text>().text = "다시인증해주세요.";
        }
    }
    /*IEnumerator Getrequest(string URL)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error" + webRequest.error);
                confirm.SetActive(true);
                confirm.GetComponentInChildren<Text>().text = "Error\r\n" + webRequest.error;

            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                confirm.SetActive(true);
                confirm.GetComponentInChildren<Text>().text = "인증되었습니다!";
            }


        }
    }*/

    public void goToChallengeList()
    {
        isConfirm = true;
        ChallengeList.SetActive(true);
        confirm.SetActive(false);
        StopAllCoroutines();
    }
    public void SnapAgain()
    {
        ChallengeList.SetActive(false);
        confirm.SetActive(false);
        CameraOn();
        StopAllCoroutines();
    }
}

