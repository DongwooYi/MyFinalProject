using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_YDW : MonoBehaviour
{
    [Header("���� ������Ʈ ����")]
    public Transform objectTofollow;
    [Header("���� ���ǵ�")]
    public float followSpeed = 10f;
    [Header("���콺 ���� / ȸ�� �ӵ�")]
    public float sensitivity = 100f;
    [Header("���콺 ���� ���� ��")]
    public float clampAngleMin = 70f;
    public float clampAngleMax = 70f;

    //���콺 ��ǲ
    private float rotX;
    private float rotY;

    [Header("ī�޶� ����")]
    public Transform realCamera;
    [Header("����")]
    public Vector3 dirNormalized;
    [Header("���� ������ ����")]
    public Vector3 finalDir;

    // ���ع��� ���� �� �ʿ��� ����
    [Header("�ּ� �Ÿ�")]
    public float minDistance;
    [Header("�ִ� �Ÿ�")]
    public float maxDistance;
    [Header("���� �Ÿ�")]
    public float finalDistance;
    [Header("ī�޶� �ε巴�� �����̱�")]
    public float smoothness = 10;

    [Header("���콺 ��")]
    //�� �ӵ� ����
    public float zoomSpeed = 0f;
    //�� �Ѱ� ��ġ��
    public float zoomMax = 0;
    public float zoomMin = 0f;

    [Header("ĳ���� ȸ�� �ӵ�")]
    public float rotSpeed = 100f;

    //realCamera ó�� ��ġ ����
    Vector3 savedPos;
    //ĳ���� ī�޶� �Ÿ��� ����
    float char2cam = 0;
    //���� �� ī�޶� ������ ������ ���� ����
    CharacterController cc;

    void Start()
    {
        // ���콺 ��ǲ �ʱ�ȭ
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        // �ʿ� ���� ?
        // ���� �� �ʱ�ȭ (normalized : ũ�Ⱑ 0 -> ���⸸ ���� / magnitude : ũ��)
        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        //ó�� ī�޶� ��ġ ����
        savedPos = realCamera.position;
        //ó�� �ִ�Ÿ� ����
        char2cam = maxDistance;

        //FollowCam �������� ������Ʈ��������
        objectTofollow = GameObject.FindObjectOfType<CharacterController>().GetComponent<Transform>();//GameObject.Find("FollowCam").GetComponent<Transform>(); 
        cc = objectTofollow.parent.GetComponent<CharacterController>();
    }

    void Update()
    {
        // ���콺 Ŀ���� Ȱ��ȭ �Ǿ��ִٸ� ������!
        if (Cursor.visible == true) return;

        // �⺻ ī�޶� ������
        CameraMove();

        // ī�޶� �ڵ� ������
        CameraAutoRotation();

        // ī�޶� ��ũ�� �� �ƿ�
        CameraZoom();

        //ĳ���� ��ġ���� �չ������� -(�ִ�Ÿ�)��ŭ (Ray�� �������)
        savedPos = objectTofollow.position + transform.forward * -char2cam;
    }

    // �� ��� ���� 
    // ī�޶� ������ (Update�� ���� ������ ����)
    private void LateUpdate()
    {
        //������Ʈ�� ���󰡾� ��
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);

        // ���ع� ������Ʈ�� ������ ���� ����
        RaycastHit hit;

        // ���ع� ���� ��
        // ī�޶� ��ġ, FollowCam��ġ����, Player�� ����
        if (Physics.Linecast(savedPos, objectTofollow.position, out hit, ~(1<<6)))
        {
            //�����Ÿ��� ������ �д�
            //finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            finalDistance = Mathf.Clamp(hit.distance, minDistance, zoomMax);
        }

        // ���ع� ���簡 �ʴ´ٸ�
        else
        {
            //�����Ÿ��� 0
            finalDistance = 0;
        }
        
        //������ ��ġ = �� ��ġ���� + �չ������� * �����Ÿ���ŭ
        Vector3 finalPosition = savedPos + realCamera.forward * finalDistance;

        realCamera.position = Vector3.Lerp(realCamera.position, finalPosition, Time.deltaTime * smoothness);

        #region ���ع� ���� �� ī�޶� ������ (�ʱⰪ)
        // ���ع� ���� �� ī�޶� ������ (�ʱⰪ)
        //realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
        //realCamera.position = finalPosition;

        //���̶�� Lerp�� �̵�
        //if (cc.isGrounded)
        //{
        //    realCamera.position = Vector3.Lerp(realCamera.position, finalPosition, Time.deltaTime * smoothness);
        //}

        ////���� �ƴ϶��
        //else
        //{
        //    realCamera.position = finalPosition;
        //}
        #endregion
    }
    
    // �⺻ ī�޶� ������
    void CameraMove()
    {
        // ���콺 ���ʹ�ư�� �����ٸ�
        if (Input.GetMouseButton(0))
        {
            // �������Ӹ��� ��ǲ�� �ޱ�
            rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            // ���� ����
            rotX = Mathf.Clamp(rotX, -clampAngleMin, clampAngleMax);
            // ȸ����Ű��
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            transform.rotation = rot;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //rotY = 0;
        }
    }

    // ī�޶� ��ũ�� ��/�ƿ�
    void CameraZoom()
    {
        //������ ������ -1, �ڷ� ������ 1�� ����
        float zoomDirection = Input.GetAxis("Mouse ScrollWheel");
      
        //ī�޶� ��ġ += ī�޶� ���麤�� * ���� * ���ǵ�
        Vector3 futurePos = realCamera.position + realCamera.transform.forward * zoomDirection * zoomSpeed;
        float dist = Vector3.Distance(futurePos, objectTofollow.position);

        if (dist >= zoomMin && dist <= zoomMax)
        {
            realCamera.position = futurePos;
            // 0�� �ƴ϶�� �ٽ� ī�޶� ���ư��� �ڵ�
            if(zoomDirection != 0)
            {
                char2cam = dist;
            }
        }
    }

    // ī�޶� �ڵ� ȸ��
    void CameraAutoRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, Vector3.down, Time.deltaTime * rotSpeed);
        }

        else if (Input.GetKeyUp(KeyCode.Q))
        {
            rotX = transform.eulerAngles.x;
            rotY = transform.eulerAngles.y;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotSpeed);
        }

        else if (Input.GetKeyUp(KeyCode.E))
        {
            rotX = transform.eulerAngles.x;
            rotY = transform.eulerAngles.y;
        }
    }
}
