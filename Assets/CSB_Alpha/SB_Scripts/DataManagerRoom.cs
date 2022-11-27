using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerRoom : MonoBehaviour
{
    public static DataManagerRoom instance;

    public GameObject showBook;
    public Texture tex;
    private void Awake()
    {
        //���࿡ instance�� null�̶��
        if (instance == null)
        {

            //instance�� ���� �ְڴ�.
            instance = this;
            //���� ��ȯ�� �Ǿ ���� �ı����� �ʰ� �ϰڴ�.
            DontDestroyOnLoad(gameObject);
        }
        //�׷��� ������
        else
        {
            //���� �ı��ϰڴ�.
            Destroy(gameObject);
        }
    }

    void Start()
    {
        showBook = GameObject.Find("ShowBook");
        tex = showBook.GetComponent<MeshRenderer>().material.mainTexture;
    }

    void Update()
    {
        
    }
}
