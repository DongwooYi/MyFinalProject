using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치 등을 이벤트로 오브젝트에 보낼 수 있는 기능을 지원함

// 가상 조이스틱
// 조이스틱의 방향으로 플레이어를 일정한 속력으로 움직이게 한다.
public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector 창에서 Drag 해서 넣어줌
    private RectTransform outerCircle;  // 나 자신


    // 조이스틱 범위
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

    // Drag 를 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");

        ControlJoystickInnerCircle(eventData);
        isInput = true;
        print(isInput);
    }

    // Drag 중 (마우스 멈추면 이벤트가 들어오지 않음)
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        ControlJoystickInnerCircle(eventData);
        isInput = false;
        print(isInput);

    }

    // Drag 를 끝
    // 원점으로
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

    // Player 에 조이스틱 입력 전달
    private void InputControl()
    {
        if (characterController)
        {
            print("들어옴");
            characterController.Move(inputVector);

        }
    }

}
