using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
   public bool Placed
    {
        get; set;
    }
    public Vector3Int Size
    {
        get;
        set;
    }
    public BoundsInt area;
    Vector3[] vertices;

    void GetColliderVertexPOsitionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        vertices = new Vector3[4];
        vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }
    void CalculateSizeInCells()
    {
        Vector3Int[] Vertices = new Vector3Int[vertices.Length];
        for (int i = 0; i <Vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            vertices[i] = BuildingSystem.instance.gridLayout.WorldToCell(worldPos);

        }
        Size = new Vector3Int(Math.Abs((Vertices[0] - Vertices[1]).x), Math.Abs((Vertices[0] - Vertices[3]).y), 1);
    }

  public  Vector3 getStartPosition()
    {
        return transform.TransformPoint(vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPOsitionsLocal();
        CalculateSizeInCells();
    }
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        Size = new Vector3Int(Size.y, Size.x, 1);
        Vector3[] Vertices = new Vector3[vertices.Length];
        for (int i = 0; i < Vertices.Length; i++)
        {
            Vertices[i] = vertices[(i + 1) % vertices.Length];
        }
        vertices = Vertices;
    }
    public virtual void Place()
    {
        ObjectDrag objectDrag = gameObject.GetComponent<ObjectDrag>();

        Destroy(objectDrag);

        Placed = true;
        //건축 할떄 인보크 사용하고싶으면 해도됨
    }
}
