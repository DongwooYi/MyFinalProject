using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchZoomInOut : MonoBehaviour
{
    public float foldTouchDistance = 0f; // ��ġ ���� �Ÿ��� ����
    public float fiedlofView = 60f; // ī�޶��� FieldofView�� �⺻ ���� 60


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
        // ��ġ�� �ΰ� �̰� �� ��ġ�� �ϳ��� �̵��Ѵٸ� ī�޶��� fieldofView�� ����
        if(Input.touchCount ==2 &&(Input.touches[0]).phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
                {
            ftouchDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
            fDistance = (ftouchDis - foldTouchDistance) * 0.01f;
            // ���� ����ġ�� �Ÿ��� ���� �� ��ġ�� �Ÿ��� ���̸� FieldofView ���� ����
            fiedlofView -= fDistance;
            // �ִ�� 100, �ּҴ� 20���� ���̻� ���� Ȥ�� ���� �� ���� �ʵ���
            fiedlofView = Mathf.Clamp(fiedlofView, 20.0f, 100.0f);
            //Ȯ�� / ��Ұ� ���ڱ� �����ʵ��� ����
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fiedlofView, Time.deltaTime);
            foldTouchDistance = ftouchDis;

        }
    }
}
