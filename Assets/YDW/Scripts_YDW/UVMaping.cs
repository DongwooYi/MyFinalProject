using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVMaping : MonoBehaviour
{
    public Vector3[] myVertices;
    public Material textureMat;
    public Vector2[] myUv;
    MeshRenderer mr;
    BoxCollider bc;
    MeshFilter mf;
    Material myMat;

    // private void OnValidate() 
    private void Start()
    {
        // 필수 컴포넌트를 붙이고 시작한다.
        mr = gameObject.GetComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        bc = gameObject.AddComponent<BoxCollider>();
        bc.size = new Vector3(1, 0.2f, 1);

        // 만들고자 하는 점점의 위치를좌표 배열로 만든다
        myVertices = new Vector3[4];
        myVertices[0] = new Vector3(-0.5f, 0, 0.5f);
        myVertices[1] = new Vector3(0.5f, 0, 0.5f);
        myVertices[2] = new Vector3(-0.5f, 0, -0.5f);
        myVertices[3] = new Vector3(0.5f, 0, -0.5f);

        mf.mesh.vertices = myVertices;

        //텍스처의 UV 설정한다
        myUv = new Vector2[myVertices.Length];
        myUv[0] = new Vector2(0, 0.5f); // 시작점은 왼쪽 부터
        myUv[1] = new Vector2(0.5f, 0.5f); // 오른쪽 위
        myUv[2] = new Vector2(0, 0); // 왼쪽 아래
        myUv[3] = new Vector2(0.5f, 0); //오른쪽 아래

        //정점을 있는 순서를 배열로 만든다
        int[] mytraingle = new int[6];
        mytraingle = new int[] { 0, 1, 2, 2, 1, 3 };
        mf.mesh.triangles = mytraingle;

       // 머티리얼을 생성하고, 텍스쳐의 UV 를 설정한다
        myMat = new Material(Shader.Find("Standard"));
            Object mat = Resources.Load<Material>("MAT_Cube");
            myMat = (Material)Instantiate(mat);
            mr.materials[0] = myMat;
        
    }
    private void Update()
    {
        mf.mesh.uv = myUv;
        mf.mesh.RecalculateNormals(); // 앞뒤: 외적 필수 
        mf.mesh.RecalculateBounds(); //범위
        mf.mesh.RecalculateTangents(); //기울기
    }
}
