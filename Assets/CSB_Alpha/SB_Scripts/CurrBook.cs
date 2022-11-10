using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrBook : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(0.25f, 0.75f, 1);
        
    }

    void Update()
    {
        
    }

    // 텍스쳐 변경
    public void ChangeTexture(Texture texture)
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat.SetTexture("_MainTex", texture);
    }
}
