using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectOnGrid : MonoBehaviour
{
    public Transform gridCellPrefabs;
    public Transform cube;
    public int height, width;
    public Transform onMousePrefabs;

    Vector3 smoothMousePosition;
    Vector3 mousePosition;
    Node[,] nodes;
    Plane plane;
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        plane = new Plane(Vector3.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePositionOnGrid();
    }
    void GetMousePositionOnGrid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out var enter))
        {
            mousePosition = ray.GetPoint(enter);
            smoothMousePosition = mousePosition;
            mousePosition.y = 0;
            mousePosition = Vector3Int.RoundToInt(mousePosition);
        }
    }
    public void OnMouseClickOnUi()
    {
        if(onMousePrefabs ==null)
        {
            onMousePrefabs = Instantiate(cube, mousePosition, Quaternion.identity);
        }
    }
    void CreateGrid()
    {
        nodes = new Node[width, height];
        var name=0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 worldPosition = new Vector3(i, 0, j);
                Transform obj = Instantiate(gridCellPrefabs, worldPosition, Quaternion.identity);
                
                obj.name = "Å¥ºê" + name;
                
                nodes[i, j] = new Node(true, worldPosition, obj);
                name++;
            }
        }
    }
}
public class Node
{
    public bool isPlaceable;
    public Vector3 cellPosition;
    public Transform obj;
    public Node(bool isPlaceble, Vector3 cellPosition, Transform obj)
    {
        this.isPlaceable = isPlaceble;
        this.cellPosition = cellPosition;
        this.obj = obj;
    }
}
