using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MapPool_YDW : MonoBehaviour
{
    public enum State
    {
        Idle,
        Play,
        Remove,
    }
    public State state;

    public GameObject objFactory;
    //오브젝트 풀의 크기
    public int objPoolSize = 10;
    //오브젝트 풀
    public static List<GameObject> objPool = new List<GameObject>();
    //프리뷰 아이템
    public GameObject prewviewItem;
    // 2) 마우스가 향하는 방향으로 시선 만들기
    Ray ray;
    RaycastHit hitInfo;
    float currentTime=0;
    void Start()
    {
        state = State.Idle;
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

    bool isunabletomake;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            print (hitInfo.collider.name);
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //------------------------------------------------------------------------------------------------
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire2"))
        {
            state = State.Remove;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Object") && state == State.Remove)
                {
                    // 오브젝트 비활성화
                    DestroyImmediate(hitInfo.transform.gameObject);
                    state = State.Idle;
                }
            }
        }
        //------------------------------------------------------------------------------------------------
        if(Input.GetMouseButton(0) || Input.GetButton("Fire1"))
        {
            state = State.Play;
            //모바일 경우 
            //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false) //____1. 모바일 경우
            if (EventSystem.current.IsPointerOverGameObject() == false && state == State.Play)//____2. PC 경우
            {
                prewviewItem.SetActive(true);
                // 2. 마우스의 위치가 바닥 위에 위치해 있다면
                if (Physics.Raycast(ray, out hitInfo))
                {
                    prewviewItem.transform.position = new Vector3((int)hitInfo.point.x, (int)hitInfo.point.y, (int)hitInfo.point.z);
                    // 충돌 체크용 함수
                    if (hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("Ground"))
                    {
                        isunabletomake = true;
                        prewviewItem.GetComponentInChildren<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        isunabletomake = false;
                        prewviewItem.GetComponentInChildren<Renderer>().material.color = Color.green;
                    }
                }
            }
        }
        //------------------------------------------------------------------------------------------------
       currentTime += Time.deltaTime;
        if (state == State.Play)
        {
            if ((Input.GetMouseButtonUp(0) || Input.GetButtonDown("Fire2")))
            {
                // 오브젝트 풀 이용하기
                // 1. 만약 오브젝트 풀에 게임오브젝트가 있다면
                if (objPool.Count > 0)
                {

                    // 2. 오브젝트 풀에서 게임오브젝트 하나 가져온다.
                    GameObject obj = objPool[0];
                    //모바일 경우 
                    //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false) //____1. 모바일 경우
                    if (EventSystem.current.IsPointerOverGameObject() == false)//____2. PC 경우
                    {
                        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && isunabletomake == false)
                        {

                            // 3. 게임오브젝트을 활성화한다.
                            obj.SetActive(true);
                            // 4.배치하고 싶다.
                            obj.transform.position = prewviewItem.transform.position;

                        }
                        prewviewItem.SetActive(false);
                        // 5. 오브젝트 풀에서 게임오브젝트을 제거한다.
                        objPool.RemoveAt(0);
                        state = State.Idle;
                    }
                }

            }
        }
    }


    public void OnClickRemove()
    {

    }

}

