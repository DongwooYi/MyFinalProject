using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableObject : MonoBehaviour
{
    public BuildingSystem_YDW YDW_BuildingSystem;
    public bool isOkaytoBuild;
    private void Start()
    {

        YDW_BuildingSystem = GameObject.FindObjectOfType<BuildingSystem_YDW>();
    }

    private void Update()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (Physics.Raycast(pos, -Vector3.up, out RaycastHit hit))
        {
            Debug.DrawLine(pos, hit.point, Color.blue);
            Debug.Log("HIt: " + hit.collider.name);
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isOkaytoBuild = true;
            }
            else
            {
                isOkaytoBuild = false;
            }
            Debug.Log(isOkaytoBuild);
        }

    }
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));

    }
    public virtual void Place()
    {

        ObjectDrag objectDrag = gameObject.GetComponent<ObjectDrag>();
        Destroy(objectDrag);

    }
}