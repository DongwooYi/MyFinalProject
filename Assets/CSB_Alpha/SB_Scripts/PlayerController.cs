using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPun
{
    public Transform characterBody;
    public Transform camPos;


    public GameObject Player;
    public float speed = 5f;
   
    string sceneName;

    public Animator animator;
    public void Start()
    {
        
        sceneName  = SceneManager.GetActiveScene().name;
        animator = characterBody.GetComponent<Animator>();
    }
    private void Update()
    {
        //LookAround();
        //Move();
        if (sceneName != "LobbyScene")
        {
            Player.SetActive(false);
        }
        else
        {
            Player.SetActive(true);
        }
    }
    // �÷��̾� �̵�
    public void Move(Vector2 InputDirection)
    {

        // �̵� ���� ���ϱ� 1
        //Debug.DrawRay(cameraArm.position, cameraArm.forward, Color.red);

        // �̵� ���� ���ϱ� 2
        //Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // �̵� ����Ű �Է� �� ��������
        //Vector2 moveInput = InputDirection;
        // �̵� ����Ű �Է� ���� : �̵� ���� ���Ͱ� 0���� ũ�� �Է��� �߻��ϰ� �ִ� ��
        bool isMove = moveInput.magnitude != 0;
        // �Է��� �߻��ϴ� ���̶�� �̵� �ִϸ��̼� ���
        animator.SetBool("isMove", isMove);
        print(isMove);
        if (isMove)
        {
            // ī�޶� �ٶ󺸴� ����
            Vector3 lookForward = new Vector3(camPos.forward.x, 0f, camPos.forward.z).normalized;
            // ī�޶��� ������ ����
            Vector3 lookRight = new Vector3(camPos.right.x, 0f, camPos.right.z).normalized;
            // �̵� ����
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // �̵��� �� ī�޶� ���� ���� �ٶ󺸱�
            //characterBody.forward = lookForward;
            // �̵��� �� �̵� ���� �ٶ󺸱�
            characterBody.forward = moveDir;
            // �̵�
            transform.position += moveDir * Time.deltaTime;
        }
        
    }

    public void LookAround(Vector3 inputDirection)
    {
        // ���콺 �̵� �� ����
        Vector2 mouseDelta = inputDirection;
        // ī�޶��� ���� ������ ���Ϸ� ������ ����
        Vector3 camAngle = camPos.rotation.eulerAngles;
        // ī�޶��� ��ġ �� ���
        float x = camAngle.x - mouseDelta.y;

        // ī�޶� ��ġ ���� �������� 70�� �Ʒ������� 25�� �̻� �������� ���ϰ� ����
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        // ī�޶� �� ȸ�� ��Ű��
        camPos.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }


}

