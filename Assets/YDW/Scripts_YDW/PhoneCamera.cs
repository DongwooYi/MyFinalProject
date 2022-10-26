using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class PhoneCamera : MonoBehaviour
{
    bool cameraAvailable;
    WebCamTexture backCam;

    Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    private void Start()
    {
        defaultBackground = background.texture;
        
    }
    private void Update()
    {
        if(!cameraAvailable)
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
            if (!devices[i].isFrontFacing)
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

    }
    public void CameraOff()
    {
        backCam.Stop();
        cameraAvailable = false;
    }
    public GameObject Loading;
    IEnumerator TakeSnap()
    {
        
        yield return new WaitForEndOfFrame();
        
        GetComponent<Renderer>().material.mainTexture = backCam;
        int width = backCam.width;
        int height = backCam.height;
        Texture2D snap = new Texture2D(width, height, TextureFormat.RGB24,false);

        // ��ũ�� �� ������ �ؽ��� �������� 
        //snap.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        snap.SetPixels(backCam.GetPixels());
        snap.Apply();

        //byte[] bytes = GetComponent<Renderer>().material.mainTexture.
         byte[] bytes = snap.EncodeToPNG();

        // �׽�Ʈ��
        File.WriteAllBytes(Application.dataPath + "/Data/photo.png", bytes);
        
        
        
        //Object.Destroy(snap);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("�̸� �־��ּ���");
    }
}
