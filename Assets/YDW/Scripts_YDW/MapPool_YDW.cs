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
    //������Ʈ Ǯ�� ũ��
    public int objPoolSize = 10;
    //������Ʈ Ǯ
    public static List<GameObject> objPool = new List<GameObject>();
    //������ ������
    public GameObject prewviewItem;
    // 2) ���콺�� ���ϴ� �������� �ü� �����
    Ray ray;
    RaycastHit hitInfo;
    float currentTime=0;
    void Start()
    {
        state = State.Idle;
        // ������Ʈ Ǯ�� ��Ȱ��ȭ�� ���ӿ�����Ʈ(������)�� ��� �ʹ�.
        for (int i = 0; i < objPoolSize; i++)
        {
            // 1. ���忡�� �����ϱ�
            GameObject obj = Instantiate(objFactory);
            // 2. ��Ȱ��ȭ�ϱ�
            obj.SetActive(false);
            // 3. ������Ʈ Ǯ�� ��� �ʹ�.
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
                    // ������Ʈ ��Ȱ��ȭ
                    DestroyImmediate(hitInfo.transform.gameObject);
                    state = State.Idle;
                }
            }
        }
        //------------------------------------------------------------------------------------------------
        if(Input.GetMouseButton(0) || Input.GetButton("Fire1"))
        {
            state = State.Play;
            //����� ��� 
            //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false) //____1. ����� ���
            if (EventSystem.current.IsPointerOverGameObject() == false && state == State.Play)//____2. PC ���
            {
                prewviewItem.SetActive(true);
                // 2. ���콺�� ��ġ�� �ٴ� ���� ��ġ�� �ִٸ�
                if (Physics.Raycast(ray, out hitInfo))
                {
                    prewviewItem.transform.position = new Vector3((int)hitInfo.point.x, (int)hitInfo.point.y, (int)hitInfo.point.z);
                    // �浹 üũ�� �Լ�
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
                // ������Ʈ Ǯ �̿��ϱ�
                // 1. ���� ������Ʈ Ǯ�� ���ӿ�����Ʈ�� �ִٸ�
                if (objPool.Count > 0)
                {

                    // 2. ������Ʈ Ǯ���� ���ӿ�����Ʈ �ϳ� �����´�.
                    GameObject obj = objPool[0];
                    //����� ��� 
                    //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false) //____1. ����� ���
                    if (EventSystem.current.IsPointerOverGameObject() == false)//____2. PC ���
                    {
                        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && isunabletomake == false)
                        {

                            // 3. ���ӿ�����Ʈ�� Ȱ��ȭ�Ѵ�.
                            obj.SetActive(true);
                            // 4.��ġ�ϰ� �ʹ�.
                            obj.transform.position = prewviewItem.transform.position;

                        }
                        prewviewItem.SetActive(false);
                        // 5. ������Ʈ Ǯ���� ���ӿ�����Ʈ�� �����Ѵ�.
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

