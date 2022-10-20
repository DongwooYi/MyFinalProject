using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPool_YDW : MonoBehaviour
{



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

    Touch touch;
    void Start()
    {

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
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetButton("Fire1"))
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
        else if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Object"))
                {
                    // ������Ʈ ��Ȱ��ȭ
                    DestroyImmediate(hitInfo.transform.gameObject);

                }

            }
        }

        // ��� �ð��� ���� �ð��� �ʰ��ߴٸ�
        if (Input.GetButtonUp("Fire1"))
        {
            // ������Ʈ Ǯ �̿��ϱ�
            // 1. ���� ������Ʈ Ǯ�� ���ӿ�����Ʈ�� �ִٸ�
            if (objPool.Count > 0)
            {

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && isunabletomake == false)
                {
                // 2. ������Ʈ Ǯ���� ���ӿ�����Ʈ �ϳ� �����´�.
                GameObject obj = objPool[0];
                    
                    // 3. ���ӿ�����Ʈ�� Ȱ��ȭ�Ѵ�.
                    obj.SetActive(true);
                    // 4.��ġ�ϰ� �ʹ�.
                    obj.transform.position = prewviewItem.transform.position;

                }
                prewviewItem.SetActive(false);
                // 5. ������Ʈ Ǯ���� ���ӿ�����Ʈ�� �����Ѵ�.
                objPool.RemoveAt(0);

            }


        }
    }


    public void OnClickRemove()
    {

    }

}

