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
        Vector3 pos = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z);
        if (Physics.Raycast(pos, -Vector3.up, out RaycastHit hit))
        {
            Debug.DrawLine(pos, hit.point, Color.blue);
            Debug.Log("Hit: " + hit.collider.name);
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
       this.transform.GetChild(0).transform.Rotate(new Vector3(0, 90, 0));

    }
    public virtual void Place()
    {

        ObjectDrag objectDrag = gameObject.GetComponent<ObjectDrag>();
        Destroy(objectDrag);

    }
}