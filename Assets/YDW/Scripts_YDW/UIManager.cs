using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[System.Serializable]
public class ObjectInfo
{
    public enum Type
    {
        Prefab1,
        Prefab2,
    }
    public Type type;
    //위치
    public Vector3 position;
    //스케일
    public Vector3 scale;
    //각도
    public Vector3 angle;
}

[System.Serializable]
public class ArrayObjectInfo
{
    public List<ObjectInfo> data;
}
public class UIManager : MonoBehaviour
{
    #region Json
    //불러오기에서 사용할 타입 오브젝트 배열
    public GameObject[] loadObjs;

    GameObject obj;

    //오브젝트 정보들을 담을 수 있는 리스트
    public List<ObjectInfo> objInfoList = new List<ObjectInfo>();

    void CreateObject(ObjectInfo info)
    {
        //게임오브젝트 타입대로 생성 
        int a = (int)info.type;
        GameObject loadObj = Instantiate(loadObjs[a]);
        loadObj.transform.position = info.position;
        loadObj.transform.localScale = info.scale;
        loadObj.transform.eulerAngles = info.angle;

    }
   
    //저장 버튼
    public void OnClickSave()
    {
        obj = GameObject.Find("parentsPrefabs");
       for(int i = 0; i < obj.transform.childCount; i++) 
        {
            ObjectInfo objectInfo = new ObjectInfo();
            GameObject child = obj.transform.GetChild(i).gameObject;
            if (child.name.Contains("PrefabsSample"))
            {
                objectInfo.type = ObjectInfo.Type.Prefab1;
            }
            else if(child.name.Contains("PrefabSample2"))
            {
                objectInfo.type = ObjectInfo.Type.Prefab2;
            }
            objectInfo.position = child.transform.position;
            objectInfo.scale = child.transform.localScale;
            objectInfo.angle = child.transform.eulerAngles;
            
            objInfoList.Add(objectInfo);

        }
        //3. 정보를 리스트에 추가
        //위치, 크기, 회전, 오브젝트 종류
        ArrayObjectInfo arrayData = new ArrayObjectInfo();
        arrayData.data = objInfoList;
        //objInfo를 Json으로 변환
        string jsonData = JsonUtility.ToJson(arrayData, true);
        print(jsonData);

        //저장경로 가져오기
        string path = UnityEngine.Application.dataPath + "/Data";

        //해당경로에 Data폴더가 있는가?
        if(Directory.Exists(path)== false)
        {
            //해당경로를 만들기
            Directory.CreateDirectory(path);
        }

        //Text 파일로 저장
        File.WriteAllText(path + "/data.txt", jsonData);

    }
    
    //불러오기 버튼
    public void OnClickLoad()
    {
        //파일경로
        string path = UnityEngine.Application.dataPath + "/Data/data.txt";
        //데이터 불러오기
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        //jsonData -> Objectinfo
        ArrayObjectInfo arrayData = JsonUtility.FromJson<ArrayObjectInfo>(jsonData);
        //오브젝트 생성
        for(int i = 0; i < arrayData.data.Count; i++)
        {
            ObjectInfo info = arrayData.data[i];
            objInfoList.Add(info);
            CreateObject(info);
        }
    }
#endregion
}
