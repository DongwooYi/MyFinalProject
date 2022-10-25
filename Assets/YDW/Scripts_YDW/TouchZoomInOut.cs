using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchZoomInOut : MonoBehaviour
{
    public float foldTouchDistance = 0f; // 터치 이전 거리를 저장
    public float fiedlofView = 60f; // 카메라의 FieldofView의 기본 값을 60


    // Update is called once per frame
    void Update()
    {
        CheckTouch();
    }
    public void CheckTouch()
    {
        int nTouch = Input.touchCount;
        float ftouchDis = 0f;
        float fDistance = 0f;
        // 터치가 두개 이고 구 터치중 하나라도 이동한다면 카메라의 fieldofView를 조정
        if(Input.touchCount ==2 &&(Input.touches[0]).phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
                {
            ftouchDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
            fDistance = (ftouchDis - foldTouchDistance) * 0.01f;
            // 이전 두터치의 거리와 지금 두 터치의 거리의 차이를 FieldofView 에서 차감
            fiedlofView -= fDistance;
            // 최대는 100, 최소는 20으로 더이상 증가 혹은 감소 가 되지 않도록
            fiedlofView = Mathf.Clamp(fiedlofView, 20.0f, 100.0f);
            //확대 / 축소가 갑자기 되지않도록 보간
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fiedlofView, Time.deltaTime);
            foldTouchDistance = ftouchDis;

        }
    }
}
