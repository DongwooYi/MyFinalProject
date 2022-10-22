using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �� �հ������� ȭ�� ����, �ܾƿ� (ī�޶��� �Ÿ�..?)
// �� �հ������� ī�޶� �̵�
// UI �ǵ�� ����

public class CameraManager : MonoBehaviour
{
    /* ī�޶� �̵� ���� */
    public float camMoveSpeed = 0.5f;   // ī�޶� �̵� �ӷ�

    private Vector2 currentPos, prePos; // �հ��� ������
    private Vector2 movePos;    // ī�޶� �̵� ������

    /* ī�޶� ���� �ƿ� ���� */
    public float zoomSpeed = 0.1f;  // ���� �ƿ� �ӷ�


    void Start()
    {
   
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))  // ���� UI �� �ƴ϶��
            {
                GetTouchDragValue();
                GetTouchZoomInOut();
            }
        }

    }

    // ȭ�� �巡��(ī�޶� �̵�) �Լ�
    // �� �հ���
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

    // ī�޶� ���� �ܾƿ� ����
    // �� �հ���
    private void GetTouchZoomInOut()
    {
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //��ġ�� ���� ���� ��ġ���� ���� ������
            //ó�� ��ġ�� ��ġ(touchZero.position)���� ���� �����ӿ����� ��ġ ��ġ�� �̹� �����ӿ��� ��ġ ��ġ�� ���̸� ��
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition�� �̵����� ������ �� ���
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // �� �����ӿ��� ��ġ ������ ���� �Ÿ� ����
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude�� �� ������ �Ÿ� ��(����)
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // �Ÿ� ���� ����(�Ÿ��� �������� ũ��(���̳ʽ��� ������)�հ����� ���� ����_���� ����)
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Camera.main.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 100f);
        }
    }
}
