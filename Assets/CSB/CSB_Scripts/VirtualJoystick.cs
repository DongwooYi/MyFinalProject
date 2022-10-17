using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ ���� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ������

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform innerCircle;  // Inspector â���� Drag �ؼ� �־���
    private RectTransform outerCircle;  // �� �ڽ�

    public GameObject player;   // Player
    public float moveSpeed;
    public float radius;

    private void Start()
    {
        outerCircle = GetComponent<RectTransform>();
    }


    // Drag �� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");

        var inputDir = eventData.position - outerCircle.anchoredPosition;
        innerCircle.anchoredPosition = inputDir;
    }

    // Drag �� (���콺 ���߸� �̺�Ʈ�� ������ ����)
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        var inputDir = eventData.position - outerCircle.anchoredPosition;
        innerCircle.anchoredPosition = inputDir;
    }

    // Drag �� ��
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");

        innerCircle.anchoredPosition = Vector2.zero;
    }
}
