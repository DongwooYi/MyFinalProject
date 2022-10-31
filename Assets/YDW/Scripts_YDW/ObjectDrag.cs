using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    
    private void OnMouseDrag()
    {
        transform.position = BuildingSystem_YDW.GetMouseWorldPosition();
    }
}
