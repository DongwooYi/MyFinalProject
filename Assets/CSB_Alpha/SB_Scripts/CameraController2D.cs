using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float delay = 0.2f;    // 스무스하게
    [SerializeField] private Vector2 minCamBoundary;
    [SerializeField] private Vector2 maxCamBoundary;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCamBoundary.x, maxCamBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCamBoundary.y, maxCamBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, delay);
    }
}
