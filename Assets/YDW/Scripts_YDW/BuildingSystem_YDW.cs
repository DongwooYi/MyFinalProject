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
    public GameObject buttonforPlaced;
    public GameObject preViewObj;
    GameObject obj;
    int idx;
    private void Start()
    {
        idx = ButtonManager.GetComponent<ButtonManager>().itemIndex;
        buttonforPlaced.SetActive(false);

    }
    private void Update()
    {
        if(!objectToPlace)
        {
            return;
        }
        if (objectToPlace.isOkaytoBuild)
        {
            buttonforPlaced.SetActive(true);
        }
        else
        {
            buttonforPlaced.SetActive(false);
        }
    }
    #region ��ư ����
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

    // ==== 3D ������ ���� =====
    // �ε��� �޾ƿ���
    // ���忡�� ������
    public GameObject ButtonManager;
    public GameObject[] factory;

    public void GetReward()
    {
        InstantiatewithObject(factory[idx]);

    }

    #endregion
    #region ���콺 ���� ��ǥ��
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Ignore Raycast"));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            Debug.Log($"�̸�: {hitInfo.transform.name}");
            return new Vector3((int)hitInfo.point.x, (int)hitInfo.point.y, (int)hitInfo.point.z);
        }
        else
        {
            return Vector3.zero;
        }

    }

    IEnumerator BeginBuilding()
    {
        obj.SetActive(false);
        GameObject go = Instantiate(preViewObj);
        go.transform.position = obj.transform.position;

        yield return new WaitForSeconds(4.0f);
        Destroy(go);
        obj.SetActive(true);
    }
    #endregion
    #region �ǹ� ����

    public void InstantiatewithObject(GameObject prefab)
    {
        obj = Instantiate(prefab);
        obj.transform.SetParent(parentsPrefabs);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();

    }

    #endregion

}