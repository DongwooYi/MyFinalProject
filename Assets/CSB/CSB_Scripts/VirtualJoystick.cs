using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치 등을 이벤트로 오브젝트에 보낼 수 있는 기능을 지원함

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector 창에서 Drag 해서 넣어줌
    private RectTransform outerCircle;  // 나 자신

    public GameObject player;   // Player
    public float moveSpeed;
    public float radius;

    private void Start()
    {
        outerCircle = GetComponent<RectTransform>();
    }


    // Drag 를 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");

        var inputDir = eventData.position - outerCircle.anchoredPosition;
        innerCircle.anchoredPosition = inputDir;
    }

    // Drag 중 (마우스 멈추면 이벤트가 들어오지 않음)
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        var inputDir = eventData.position - outerCircle.anchoredPosition;
        innerCircle.anchoredPosition = inputDir;
    }

    // Drag 를 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");

        innerCircle.anchoredPosition = Vector2.zero;
    }
}
