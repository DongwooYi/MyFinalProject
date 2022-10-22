using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 두 손가락으로 화면 줌인, 줌아웃 (카메라의 거리..?)
// 한 손가락으로 카메라 이동
// UI 건들면 리턴

public class CameraManager : MonoBehaviour
{
    /* 카메라 이동 관련 */
    public float camMoveSpeed = 0.5f;   // 카메라 이동 속력

    private Vector2 currentPos, prePos; // 손가락 포지션
    private Vector2 movePos;    // 카메라 이동 포지션

    /* 카메라 줌인 아웃 관련 */
    public float zoomSpeed = 0.1f;  // 줌인 아웃 속력


    void Start()
    {
   
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))  // 만약 UI 위 아니라면
            {
                GetTouchDragValue();
                GetTouchZoomInOut();
            }
        }

    }

    // 화면 드래그(카메라 이동) 함수
    // 한 손가락
    private void GetTouchDragValue()
    {
        movePos = Vector3.zero;

        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                currentPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - currentPos) * Time.deltaTime * camMoveSpeed;
                Camera.main.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;
            }
        }
    }

    // 카메라 줌인 줌아웃 관련
    // 두 손가락
    private void GetTouchZoomInOut()
    {
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //터치에 대한 이전 위치값을 각각 저장함
            //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // 각 프레임에서 터치 사이의 벡터 거리 구함
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Camera.main.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 100f);
        }
    }
}
