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
    //��ġ
    public Vector3 position;
    //������
    public Vector3 scale;
    //����
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
    //�ҷ����⿡�� ����� Ÿ�� ������Ʈ �迭
    public GameObject[] loadObjs;

    GameObject obj;

    //������Ʈ �������� ���� �� �ִ� ����Ʈ
    public List<ObjectInfo> objInfoList = new List<ObjectInfo>();

    void CreateObject(ObjectInfo info)
    {
        //���ӿ�����Ʈ Ÿ�Դ�� ���� 
        int a = (int)info.type;
        GameObject loadObj = Instantiate(loadObjs[a]);
        loadObj.transform.position = info.position;
        loadObj.transform.localScale = info.scale;
        loadObj.transform.eulerAngles = info.angle;

    }
   
    //���� ��ư
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
        //3. ������ ����Ʈ�� �߰�
        //��ġ, ũ��, ȸ��, ������Ʈ ����
        ArrayObjectInfo arrayData = new ArrayObjectInfo();
        arrayData.data = objInfoList;
        //objInfo�� Json���� ��ȯ
        string jsonData = JsonUtility.ToJson(arrayData, true);
        print(jsonData);

        //������ ��������
        string path = UnityEngine.Application.dataPath + "/Data";

        //�ش��ο� Data������ �ִ°�?
        if(Directory.Exists(path)== false)
        {
            //�ش��θ� �����
            Directory.CreateDirectory(path);
        }

        //Text ���Ϸ� ����
        File.WriteAllText(path + "/data.txt", jsonData);

    }
    
    //�ҷ����� ��ư
    public void OnClickLoad()
    {
        //���ϰ��
        string path = UnityEngine.Application.dataPath + "/Data/data.txt";
        //������ �ҷ�����
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        //jsonData -> Objectinfo
        ArrayObjectInfo arrayData = JsonUtility.FromJson<ArrayObjectInfo>(jsonData);
        //������Ʈ ����
        for(int i = 0; i < arrayData.data.Count; i++)
        {
            ObjectInfo info = arrayData.data[i];
            objInfoList.Add(info);
            CreateObject(info);
        }
    }
#endregion
}
