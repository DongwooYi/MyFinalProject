using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class BuildingSystem_YDW : MonoBehaviour
{

    public ChallengeWorldManager challengeWorldManager;
    PlaceableObject objectToPlace;
    public Transform parentsPrefabs;
    public GameObject ObjPlacedList;
    public GameObject buttonforPlaced;
    public GameObject buttonforDestroy;
    public GameObject buttonforRotate;
    public GameObject preViewObj;
    GameObject obj;
    int idx;
    private void Start()
    {
        idx = ButtonManager.GetComponent<ButtonManager>().itemIndex;
        ObjPlacedList.SetActive(false);
    }
    private void Update()
    {
        if (!objectToPlace)
        {
            return;
        }
        if (objectToPlace.isOkaytoBuild)
        {
            ObjPlacedList.SetActive(true);
            buttonforPlaced.GetComponent<Button>().interactable =true;

        }
        else
        {
            ObjPlacedList.SetActive(false);
            buttonforPlaced.GetComponent<Button>().interactable = false;

        }
    }
    #region 버튼 선택
    public void OnclickPlaced()
    {
        if (!objectToPlace)
        {
            return;
        }

        objectToPlace.Place();
        StartCoroutine(BeginBuilding());


    }
    public void OnClickDestory()
    {
        if (!objectToPlace)
        {
            return;
        }
        Destroy(objectToPlace.gameObject);

    }
    public void OnclickRotate()
    {
        if (!objectToPlace)
        {
            return;
        }
        objectToPlace.Rotate();
    }

    public void OnclickCreate2ndPrefabs()
    {
        if (!objectToPlace)
        {
            return;
        }
        GetReward();
    }

    // ==== 3D 아이템 생성 =====
    // 인덱스 받아오기
    // 공장에서 꺼내기
    public GameObject ButtonManager;
    public GameObject[] factory;

    public void GetReward()
    {
        InstantiatewithObject(factory[idx]);

    }

    #endregion
    #region 마우스 월드 좌표용
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Ignore Raycast"));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            Debug.Log($"이름: {hitInfo.transform.name}");
            Debug.Log("hitInfo: "+hitInfo.point.y);
            return new Vector3((int)hitInfo.point.x, hitInfo.point.y, (int)hitInfo.point.z);
        }
        else
        {
            return Vector3.zero;
        }

    }

    IEnumerator BeginBuilding()
    {
        
        buttonforDestroy.GetComponent<Button>().interactable = false;
        buttonforRotate.GetComponent<Button>().interactable = false;
        obj.SetActive(false);
        GameObject go = Instantiate(preViewObj);
        go.transform.position = obj.transform.position;
        yield return new WaitForSeconds(4.0f);
        Destroy(go);
        obj.SetActive(true);
        buttonforDestroy.GetComponent<Button>().interactable = true;
        buttonforRotate.GetComponent<Button>().interactable = true;
    }
    #endregion
    #region 건물 건축

    public void InstantiatewithObject(GameObject prefab)
    {
        obj = Instantiate(prefab);
        obj.transform.SetParent(parentsPrefabs);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();

    }

    #endregion

}