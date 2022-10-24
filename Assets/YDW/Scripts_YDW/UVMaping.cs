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
        // �ʼ� ������Ʈ�� ���̰� �����Ѵ�.
        mr = gameObject.GetComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        bc = gameObject.AddComponent<BoxCollider>();
        bc.size = new Vector3(1, 0.2f, 1);

        // ������� �ϴ� ������ ��ġ����ǥ �迭�� �����
        myVertices = new Vector3[4];
        myVertices[0] = new Vector3(-0.5f, 0, 0.5f);
        myVertices[1] = new Vector3(0.5f, 0, 0.5f);
        myVertices[2] = new Vector3(-0.5f, 0, -0.5f);
        myVertices[3] = new Vector3(0.5f, 0, -0.5f);

        mf.mesh.vertices = myVertices;

        //�ؽ�ó�� UV �����Ѵ�
        myUv = new Vector2[myVertices.Length];
        myUv[0] = new Vector2(0, 0.5f); // �������� ���� ����
        myUv[1] = new Vector2(0.5f, 0.5f); // ������ ��
        myUv[2] = new Vector2(0, 0); // ���� �Ʒ�
        myUv[3] = new Vector2(0.5f, 0); //������ �Ʒ�

        //������ �ִ� ������ �迭�� �����
        int[] mytraingle = new int[6];
        mytraingle = new int[] { 0, 1, 2, 2, 1, 3 };
        mf.mesh.triangles = mytraingle;

       // ��Ƽ������ �����ϰ�, �ؽ����� UV �� �����Ѵ�
        myMat = new Material(Shader.Find("Standard"));
            Object mat = Resources.Load<Material>("MAT_Cube");
            myMat = (Material)Instantiate(mat);
            mr.materials[0] = myMat;
        
    }
    private void Update()
    {
        mf.mesh.uv = myUv;
        mf.mesh.RecalculateNormals(); // �յ�: ���� �ʼ� 
        mf.mesh.RecalculateBounds(); //����
        mf.mesh.RecalculateTangents(); //����
    }
}
