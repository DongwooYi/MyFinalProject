using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector 창에서 Drag 해서 넣어줌
    private RectTransform outerCircle;  // 나 자신


    // 조이스틱 범위
    [SerializeField, Range(5f, 50f)]
    private float joystickRange;

    public PlayerController playerController;
    public bool isInput;

    private Vector2 inputVector;

    private void Awake()
    {
        outerCircle = GetComponent<RectTransform>();
    }

    private void Start()
    {
        //Screen.orientation = ScreenOrientation.LandscapeRight;  // 씬 화면 고정
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (isInput)
        {
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
        //isInput = false;
        //print(isInput);

    }

    // Drag 를 끝
    // 원점으로
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");

        innerCircle.anchoredPosition = Vector2.zero;    // 원점으로 돌아옴
        isInput = false;    // 입력 끝
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
        if (playerController)
        {
            playerController.Move(inputVector);

        }
    }

}
