using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ ���� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ������

// ���� ���̽�ƽ
// ���̽�ƽ�� �������� �÷��̾ ������ �ӷ����� �����̰� �Ѵ�.
public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector â���� Drag �ؼ� �־���
    private RectTransform outerCircle;  // �� �ڽ�


    // ���̽�ƽ ����
    [SerializeField, Range(5f, 50f)]
    private float joystickRange;

    public CharacterController characterController;

    private Vector2 inputVector;
    public bool isInput;

    private void Awake()
    {
        outerCircle = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isInput)
        {
            Debug.Log("isInput");
            InputControl();
        }
    }

    // Drag �� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");

        ControlJoystickInnerCircle(eventData);
        isInput = true;
        print(isInput);
    }

    // Drag �� (���콺 ���߸� �̺�Ʈ�� ������ ����)
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        ControlJoystickInnerCircle(eventData);
        isInput = false;
        print(isInput);

    }

    // Drag �� ��
    // ��������
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");

        innerCircle.anchoredPosition = Vector2.zero;
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
        if (characterController)
        {
            print("����");
            characterController.Move(inputVector);

        }
    }

}
