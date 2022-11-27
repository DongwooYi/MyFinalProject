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
        //만약에 instance가 null이라면
        if (instance == null)
        {

            //instance에 나를 넣겠다.
            instance = this;
            //씬이 전환이 되어도 나를 파괴되지 않게 하겠다.
            DontDestroyOnLoad(gameObject);
        }
        //그렇지 않으면
        else
        {
            //나를 파괴하겠다.
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
