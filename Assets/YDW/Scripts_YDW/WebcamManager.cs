using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class WebcamManager : MonoBehaviour
{
    WebCamTexture camTexture;
    public RawImage cameraViewImage; // ī�޶� ������ ȭ��
   
    public void CameraOn()
    {
        // ī�޶� ����
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        // ī�޶� ������
        if(WebCamTexture.devices.Length ==0)
        {
            Debug.Log("ī�޶� ����");
            return;
        }
        WebCamDevice[] devices = WebCamTexture.devices; // ����Ʈ���� ī�޶� ���� ��� ��������
        int selectedCameraIndex = 0;

        //�ĸ� ī�޶� ã��
        for (int i = 0; i < devices.Length; i++)
        {if(devices[i].isFrontFacing == false)
            {
                selectedCameraIndex = i;
                break;
            }

        }
        print(selectedCameraIndex + "==selectedCameraIndex");
        // ī�޶� �ѱ�
        if(selectedCameraIndex>=0)
        {

            // ���ñ� �ĸ� ī�޶� ������
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);
            camTexture.requestedFPS = 30; // ī�޶� ������
            cameraViewImage.texture = camTexture; // ������ Raw Image���Ŀ� �Ҵ�
            camTexture.Play(); // ī�޶� ����
        }
    }
    public void CameraOff()
    {
        if(camTexture != null)
        {
            camTexture.Stop(); //ī�޶� ����
            WebCamTexture.Destroy(camTexture); // ī�޶� ��ü �ݳ�
            camTexture = null; // ���� �ʱ�ȭ
        }
    }
}
