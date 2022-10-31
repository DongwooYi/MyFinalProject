using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreviewObject : MonoBehaviour
{
   
    public TextMeshPro text;
    public float startTime = 0;
   public float buildTime =4.0f;
    void Start()
    {

        startTime = 0;
        buildTime = 4.0f;
    }
    private void OnMouseDrag()
    {
      transform.position = BuildingSystem_YDW.GetMouseWorldPosition();
    }
    void Update()
    {
        startTime += Time.deltaTime;
        text.text = ((int)buildTime - (int)startTime).ToString();
        text.transform.position = new Vector3(transform.position.x, transform.position.y + 2f,transform.position.z);
    }
}
