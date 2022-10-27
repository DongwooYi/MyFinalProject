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
using Photon.Voice;
using System.Drawing;
using Unity.VisualScripting;

public class PhoneCamera : MonoBehaviour
{
    
    bool cameraAvailable;
    WebCamTexture backCam;

    Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    challenges challenges;

    public GameObject Spinner;

    private void Start()
    {
        challenges = GetComponent<challenges>();
        defaultBackground = background.texture;
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            challenges.ChallengesList.SetActive(true);

        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            challenges.ChallengesList.SetActive(false);

        }
        if (!cameraAvailable)
        {
            return;
        }
        //AspectRatioFitter �̿����
        float ratio = (float)backCam.width / (float)backCam.height;

        fit.aspectRatio = ratio;
        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
       
        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void OnClickCameraON()
    {

    }

    public void CameraOn()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("ī�޶� ����");
            cameraAvailable = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            //if (devices[i].isFrontFacing) // ���� ī�޶�
            if (!devices[i].isFrontFacing) // �ĸ� ī�޶�
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("�ĸ� ī�޶� ã�� ������");
            return;
        }
        backCam.Play();
        background.texture = backCam;
        cameraAvailable = true;
    }

    public void TakeaShot()
    {
        StartCoroutine(TakeSnap());
        StopAllCoroutines();
        OnCompleteGetPost("���� �ٿ�ε� URL");
    }
    public void CameraOff()
    {
        backCam.Stop();
        cameraAvailable = false;
    }
    IEnumerator TakeSnap()
    {
        
        yield return new WaitForEndOfFrame();
        Spinner.SetActive(true);
        
        int width = backCam.width;
        int height = backCam.height;
        Texture2D snap = new Texture2D(width, height, TextureFormat.RGB24,false);
        if (snap == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Select Texture", "You Must Select a Texture first!", "Ok");
            yield break;
        }
        // ��ũ�� �� ������ �ؽ��� �������� 
        //snap.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        snap.SetPixels(backCam.GetPixels());
        snap.Apply();

        //byte[] bytes = GetComponent<Renderer>().material.mainTexture.
         byte[] bytes = snap.EncodeToPNG();
         UnityEngine.Object.Destroy(snap);

        WWWForm form = new WWWForm();
        form.AddBinaryData("image", bytes);
        foreach (KeyValuePair<string,int> keyValuePair in challenges.dictionary)
        {
            form.AddField(keyValuePair.Key, keyValuePair.Value);
            Debug.Log(keyValuePair.Key + ":" + keyValuePair.Value);
        }

        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.15:5005/detection", form);

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
        }
        
        // �׽�Ʈ��
        File.WriteAllBytes(Application.dataPath + "/Data/photo.png", bytes);
        
        
        
    }

    public bool isOkay;
   public void OnCompleteGetPost(string URL)
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
        
        request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            isOkay = false;  
            Debug.Log(request.error);
        }
        else
        {
            isOkay = true;
            File.WriteAllBytes(Application.dataPath + "/Data/photo.png", request.downloadHandler.data);
        }
        
    }
    
        
        public void LoadScene()
    {
        SceneManager.LoadScene("�̸� �־��ּ���");
    }

   
}

