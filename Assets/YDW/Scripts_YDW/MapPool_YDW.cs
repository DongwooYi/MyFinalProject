using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPool_YDW : MonoBehaviour
{
    public GameObject objFactory;
    public int objPoolSize = 10;
    public static List<GameObject> objPool = new List<GameObject>();
    // 생성 시간
    public float createTime = 0.1f;
    // 경과 시간
    float currentTime = 0;
    void Start()
    {
        // 오브젝트 풀에 비활성화된 게임오브젝트(아이템)을 담고 싶다.
        for (int i = 0; i < objPoolSize; i++)
        {
            // 1. 공장에서 생성하기
            GameObject obj = Instantiate(objFactory);
            // 2. 비활성화하기
            obj.SetActive(false);
            // 3. 오브젝트 풀에 담고 싶다.
            objPool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // 일정 시간마다 복셀을 만들고 싶다.
        // 1. 경과 시간이 흐른다.
        currentTime += Time.deltaTime;

        // 2. 경과 시간이 생성 시간을 초과했다면
        if (Input.GetButtonDown("Fire1") && currentTime > createTime)
        {
            // 2) 마우스가 향하는 방향으로 시선 만들기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();

            // 2. 마우스의 위치가 바닥 위에 위치해 있다면
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 오브젝트 풀 이용하기
                // 1. 만약 오브젝트 풀에 게임오브젝트가 있다면
                if (objPool.Count > 0)
                {
                    // 생성했을 때만 경과 시간을 초기화해준다.
                    currentTime = 0;
                    // 2. 오브젝트 풀에서 게임오브젝트 하나 가져온다.
                    GameObject obbj = objPool[0];
                    // 3. 게임오브젝트을 활성화한다.
                    obbj.SetActive(true);
                    /* // 4. 복셀을 배치하고 싶다.
                     obbj.transform.position = hitInfo.point;*/
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        obbj.transform.position = new Vector3((int)hitInfo.point.x, hitInfo.point.y + 0.5f, (int)hitInfo.point.z);
                    }
                    // 5. 오브젝트 풀에서 게임오브젝트을 제거한다.
                    objPool.RemoveAt(0);
                    
                }
            }
        }
    }

}

