using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
public class Joystick2D : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector â���� Drag �ؼ� �־���
    private RectTransform outerCircle;  // �� �ڽ�

    // ���̽�ƽ ����
    [SerializeField, Range(5f, 50f)]
    private float joystickRange;

    public PlayerController playerController;
    public bool isInput;

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
        //Screen.orientation = ScreenOrientation.LandscapeRight;  // �� ȭ�� ����
        playerController = GameObject.FindObjectOfType<PlayerController>();

    }
    private void Update()
    {
        if (isInput)
        {
            InputControl();
        }
        //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
        // if (EventSystem.current.IsPointerOverGameObject() == false)
        
    }



    // Drag �� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");
        ControlJoystickInnerCircle(eventData);
        isInput = true;

    }

    // Drag �� (���콺 ���߸� �̺�Ʈ�� ������ ����)
    public void OnDrag(PointerEventData eventData)
    {

        Debug.Log("Drag");
        ControlJoystickInnerCircle(eventData);
    }

    // Drag �� ��
    // ��������
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");
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
        //characterController.Move(Vector2.zero);
    }

    public void ControlJoystickInnerCircle(PointerEventData eventData)
    {
        var inputDir = eventData.position - outerCircle.anchoredPosition;
        var clampedDir = inputDir.magnitude < joystickRange ? inputDir : inputDir.normalized * joystickRange;
        //innerCircle.anchoredPosition = inputDir;
        innerCircle.anchoredPosition = clampedDir;
        inputVector = clampedDir / joystickRange;
    }

    // Player �� ���̽�ƽ �Է� ����
    private void InputControl()
    {
        print(joystickType);
        switch (joystickType)
        {
            case JoystickType.Move:
                if (playerController)
                {
                    playerController.Move(inputVector);
                }
                break;
            case JoystickType.Rotate:
                playerController.LookAround(inputVector);
                break;
        }

    }

}
