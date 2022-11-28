using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class Joystick2DLobby : MonoBehaviourPun//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector 창에서 Drag 해서 넣어줌
    private RectTransform outerCircle;  // 나 자신

    // 조이스틱 범위
    [SerializeField, Range(5f, 50f)]
    private float joystickRange;

    public YDW_CharacterController playerController;
    public bool isInput;

    public Text log;
    public float moveSpeed = 10;
    Vector2 touchOrigin;
    private Vector2 inputVector;

    #region 이동우 조이스틱 수정 부분
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
                innerCircle.anchoredPosition = Vector2.zero;    // 원점으로 돌아옴
                isInput = false;    // 입력 끝
            }
       
        }
#endif

    }


#if PC
    // Drag 를 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin");
        ControlJoystickInnerCircle(eventData);
        
        isInput = true;

    }

    // Drag 중 (마우스 멈추면 이벤트가 들어오지 않음)
    public void OnDrag(PointerEventData eventData)
    {
       // Debug.Log("Drag");
        ControlJoystickInnerCircle(eventData);
    }

    // Drag 를 끝
    // 원점으로
    public void OnEndDrag(PointerEventData eventData)
    {
      //  Debug.Log("End");
        innerCircle.anchoredPosition = Vector2.zero;    // 원점으로 돌아옴
        isInput = false;    // 입력 끝
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

    // Player 에 조이스틱 입력 전달
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
