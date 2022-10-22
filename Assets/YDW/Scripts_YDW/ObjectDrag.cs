using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    Vector3 offset;
    private void OnMouseDown()
    {
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }
    private void OnMouseDrag()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition()+offset;
        transform.position = BuildingSystem.instance.SnapCoordinatetoGrid(pos);
    }
}
