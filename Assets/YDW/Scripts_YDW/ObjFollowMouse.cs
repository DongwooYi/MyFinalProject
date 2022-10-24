using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFollowMouse : MonoBehaviour
{
    PlaceObjectOnGrid PlaceObjectOnGrid;
    // Start is called before the first frame update
    void Start()
    {
        PlaceObjectOnGrid = FindObjectOfType<PlaceObjectOnGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
