using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class Joystick2DLobby : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector â���� Drag �ؼ� �־���
    private RectTransform outerCircle;  // �� �ڽ�

    // ���̽�ƽ ����
    [SerializeField, Range(5f, 50f)]
    private float joystickRange;

    public YDW_CharacterController playerController;
    public bool isInput;

    public Text log;
    public float moveSpeed = 10;
    Vector2 touchOrigin;
    private Vector2 inputVector;

    #region �̵��� ���̽�ƽ ���� �κ�
    public enum JoystickType { Move, Rotate }
    public JoystickType joystickType;
    #endregion
    private void Awake()
    {
        outerCircle = GetComponent<RectTransform>();
    }

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<YDW_CharacterController>();
    }
    private void Update()
    {
       
            if (isInput)
        {
            InputControl();
        }


#if !PC
        Touch touch = Input.GetTouch(0);

        bool isIn = Vector3.Distance(outerCircle.position, touch.position) <= 600;
      
        if (isIn)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touchOrigin = touch.position;
                isInput = true;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                ControlJoystickInnerCircle((touch.position - touchOrigin).normalized);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                    playerController.Move(Vector.zero);
                
                innerCircle.anchoredPosition = Vector2.zero;    // �������� ���ƿ�
                isInput = false;    // �Է� ��
            }
       
        }
#endif

    }


#if PC
    // Drag �� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin");
        ControlJoystickInnerCircle(eventData);
        
        isInput = true;

    }

    // Drag �� (���콺 ���߸� �̺�Ʈ�� ������ ����)
    public void OnDrag(PointerEventData eventData)
    {
       // Debug.Log("Drag");
        ControlJoystickInnerCircle(eventData);
    }

    // Drag �� ��
    // ��������
    public void OnEndDrag(PointerEventData eventData)
    {
      //  Debug.Log("End");
        innerCircle.anchoredPosition = Vector2.zero;    // �������� ���ƿ�
        isInput = false;    // �Է� ��
        switch (joystickType)
        {
            case JoystickType.Move:
                playerController.Move(Vector2.zero);
                break;
            case JoystickType.Rotate:
                break;
        }
        playerController.Move(Vector2.zero);
    }
#endif

    public void ControlJoystickInnerCircle(PointerEventData eventData)
    {
        var inputDir = eventData.position - outerCircle.anchoredPosition;
        var clampedDir = inputDir.magnitude < joystickRange ? inputDir : inputDir.normalized * joystickRange;
        //innerCircle.anchoredPosition = inputDir;
        innerCircle.anchoredPosition = clampedDir;
        inputVector = clampedDir / joystickRange;
    }

    public void ControlJoystickInnerCircle(Vector2 eventData)
    {
        Vector3 moveRange = eventData * moveSpeed;

        var clampedDir = moveRange.magnitude < joystickRange ? moveRange : moveRange.normalized * joystickRange;
        
        innerCircle.anchoredPosition = clampedDir;
        inputVector = clampedDir / joystickRange;
    }

    // Player �� ���̽�ƽ �Է� ����
    private void InputControl()
    {
        //print(joystickType);
        switch (joystickType)
        {
            case JoystickType.Move:
                if (playerController)
                {
                    playerController.Move(inputVector);
                }
                break;
            case JoystickType.Rotate:
               // playerController.LookAround(inputVector);
                break;
        }

    }

}
