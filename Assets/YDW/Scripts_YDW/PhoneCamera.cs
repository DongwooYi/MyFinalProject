using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class PhoneCamera : MonoBehaviour
{
    bool cameraAvailable;
    WebCamTexture backCam;
    Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;
    private void Start()
    {
     
            /*  defaultBackground = background.texture;
              WebCamDevice[] devices = WebCamTexture.devices;
          if(devices.Length ==0)
              {
                  Debug.Log("카메라 없음");
                  cameraAvailable = false;
                  return;       
              }
              for (int i = 0; i < devices.Length; i++)
              {
                  if(!devices[i].isFrontFacing)
                  {
                      backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                  }
              }

              if(backCam== null)
              {
                  Debug.Log("후면 카메라 찾을 수없음");
                  return;
              }
              backCam.Play();
              background.texture = backCam;
              cameraAvailable = true; */
        }

    private void Update()
    {
        if(!cameraAvailable)
        {
            return;
        }
        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;
        float scaleY = backCam.videoVerticallyMirrored ? -10f : 10f;
        background.rectTransform.localScale = new Vector3(10f, scaleY, 10f);
        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void CameraOn()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("카메라 없음");
            cameraAvailable = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("후면 카메라 찾을 수없음");
            return;
        }
        backCam.Play();
        background.texture = backCam;
        cameraAvailable = true;
    }
    
    public void CameraOff()
    {
        StartCoroutine(TakeSnap());
        backCam.Stop();
        cameraAvailable = false;
        
    }
    IEnumerator TakeSnap()
    {
        //print("111111111111");
        yield return new WaitForEndOfFrame();
        int width = backCam.width;
        int height = backCam.height;
        Texture2D snap = new Texture2D(width, height, TextureFormat.RGB24,false);

        // 스크린 샷 정보를 텍스쳐 형식으로 
        snap.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        snap.Apply();
        //snap.SetPixels(backCam.GetPixels());
        
        byte[] bytes = snap.EncodeToPNG();
        Object.Destroy(snap);
        // 테스트용
         File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }
}
