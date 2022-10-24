using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class WebcamManager : MonoBehaviour
{
    WebCamTexture camTexture;
    public RawImage cameraViewImage; // 카메라에 보여질 화면
   
    public void CameraOn()
    {
        // 카메라 권한
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        // 카메라가 없으면
        if(WebCamTexture.devices.Length ==0)
        {
            Debug.Log("카메라 없음");
            return;
        }
        WebCamDevice[] devices = WebCamTexture.devices; // 스마트폰의 카메라 정보 모두 가져오기
        int selectedCameraIndex = 0;

        //후면 카메라 찾기
        for (int i = 0; i < devices.Length; i++)
        {if(devices[i].isFrontFacing == false)
            {
                selectedCameraIndex = i;
                break;
            }

        }
        print(selectedCameraIndex + "==selectedCameraIndex");
        // 카메라 켜기
        if(selectedCameraIndex>=0)
        {

            // 선택괸 후면 카메라 가져옴
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);
            camTexture.requestedFPS = 30; // 카메라 프레임
            cameraViewImage.texture = camTexture; // 영상을 Raw Image형식에 할당
            camTexture.Play(); // 카메라 시작
        }
    }
    public void CameraOff()
    {
        if(camTexture != null)
        {
            camTexture.Stop(); //카메라 정지
            WebCamTexture.Destroy(camTexture); // 카메라 객체 반납
            camTexture = null; // 변수 초기화
        }
    }
}
