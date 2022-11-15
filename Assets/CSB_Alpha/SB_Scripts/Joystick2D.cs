using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    #region 이동우 조이스틱 수정 부분
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
        //Screen.orientation = ScreenOrientation.LandscapeRight;  // 씬 화면 고정
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



    // Drag 를 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");
        ControlJoystickInnerCircle(eventData);
        isInput = true;

    }

    // Drag 중 (마우스 멈추면 이벤트가 들어오지 않음)
    public void OnDrag(PointerEventData eventData)
    {

        Debug.Log("Drag");
        ControlJoystickInnerCircle(eventData);
    }

    // Drag 를 끝
    // 원점으로
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");
        innerCircle.anchoredPosition = Vector2.zero;
        playerController.Move(Vector2.zero);
        // 원점으로 돌아옴
        isInput = false;    // 입력 끝
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

    // Player 에 조이스틱 입력 전달
    private void InputControl()
    {
        Debug.Log(inputVector.x + "/" + inputVector.y);
        switch (joystickType)
        {
            case JoystickType.Move:
                if (playerController)
                {
                    playerController.Move(inputVector);
                }
                break;
            case JoystickType.Rotate:
                //playerController.LookAround(inputVector);
                break;
        }

    }

}
