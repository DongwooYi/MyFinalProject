using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Photon.Pun;

//, IBeginDragHandler, IDragHandler, IEndDragHandler
public class Joystick2DPhoton : MonoBehaviourPun , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector â���� Drag �ؼ� �־���
    private RectTransform outerCircle;  // �� �ڽ�

    // ���̽�ƽ ����
    [SerializeField, Range(5f, 50f)]
    private float joystickRange;

    public YDW_CharacterControllerPhoton playerController;
    public bool isInput;


    public float moveSpeed = 10;
    Vector2 touchOrigin;

    private Vector2 inputVector;
    #region �̵��� ���̽�ƽ ���� �κ�
    public enum JoystickType { Move, Rotate }
    public JoystickType joystickType;
    string sceneName;
    #endregion
    private void Awake()
    {
        outerCircle = GetComponent<RectTransform>();
    }

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<YDW_CharacterControllerPhoton>();

    }
    private void Update()
    {
        if (isInput)
        {
            InputControl();
        }

        //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
        // if (EventSystem.current.IsPointerOverGameObject() == false)
#if !PC
        Touch touch = Input.GetTouch(0);

        bool isIn = Vector3.Distance(outerCircle.position, touch.position) <= 600;

        if (isIn&&photonView.IsMine)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touchOrigin = touch.position;

            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                ControlJoystickInnerCircle((touch.position - touchOrigin).normalized);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
         playerController.Move(Vector2.zero);

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
                ControlJoystickInnerCircle(eventData);

        isInput = true;

    }

    // Drag �� (���콺 ���߸� �̺�Ʈ�� ������ ����)
    public void OnDrag(PointerEventData eventData)
    {
        
      ControlJoystickInnerCircle(eventData);
    }

    // Drag �� ��
    // ��������
    public void OnEndDrag(PointerEventData eventData)
    {
        
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
