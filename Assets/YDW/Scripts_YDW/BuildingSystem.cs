using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BuildingSystem : MonoBehaviour
{
    public static Dictionary<TileType, TileBase> tileBase = new Dictionary<TileType, TileBase>();
    public enum TileType
    {
        Empty,
        White,
        Green,
        Red
    }

    public static BuildingSystem instance;
    public GridLayout gridLayout;
    Grid grid;
    [SerializeField]
    Tilemap mainTileMap;
    [SerializeField]
    TileBase whiteTile;
    public GameObject prefab1;
    public GameObject prefab2;

    PlaceableObject objectToPlace;
    public GameObject parentsPrefabs;
    Vector3 prevPos;

    private void Awake()
    {
        instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    private void Start()
    {

    }
    void Update()
    {
        if (!objectToPlace)
        {
            return;
        }
        #region PC 테스트용
        if (Input.GetKeyDown(KeyCode.Space))

        {
            if (canBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.getStartPosition());
                TakeArea(start, objectToPlace.Size);
            }
            else
            {
                Destroy(objectToPlace.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Destroy(objectToPlace.gameObject);
        }
        #endregion
    }
    #region 버튼 선택


        

    public void OnclickPlaced()
    {       
        
        objectToPlace.Place();
        Vector3Int start = gridLayout.WorldToCell(objectToPlace.getStartPosition());
        TakeArea(start, objectToPlace.Size);
        
    }
    public void OnClickDestory()
    {
      Destroy(objectToPlace.gameObject);
    }
    public void OnclickRotate()
    {
        objectToPlace.Rotate();
    }
    public void OnclickCreate1stPrefabs()
    {
        InstantiatewithObject(prefab1);

    }
    public void OnclickCreate2ndPrefabs()
    {
        InstantiatewithObject(prefab2);

    }
    #endregion
    #region 마우스 월드 좌표용
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            return hitInfo.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 SnapCoordinatetoGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;

    }
    static TileBase[] GetTileBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        Filltiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }
    static void Filltiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i <arr.Length; i++)
        {
            arr[i] = tileBase[type];
        }
    }
    #endregion 
    #region 건물 건축
    public void InstantiatewithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinatetoGrid(Vector3.zero);
        //objectToPlace = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.parent = parentsPrefabs.transform;
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
        
    }

    bool canBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.getStartPosition());
        area.size = placeableObject.Size;
        TileBase[] baseArray = GetTileBlock(area, mainTileMap);
        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];
        for (int i = 0; i <baseArray.Length; i++)
        { if (baseArray[i] == tileBase[TileType.White])
            {
                tileArray[i] = tileBase[TileType.Green];
            }
        else
            {
                
            }
        }
        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }
        return true;
    }
    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        mainTileMap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.y, start.y + size.y);
    }
    #endregion

}
