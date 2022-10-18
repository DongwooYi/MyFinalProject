using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPool_YDW : MonoBehaviour
{
    public GameObject objFactory;
    public int objPoolSize = 10;
    public static List<GameObject> objPool = new List<GameObject>();
    // ���� �ð�
    public float createTime = 0.1f;
    // ��� �ð�
    float currentTime = 0;
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

    // Update is called once per frame
    void Update()
    {

        // ���� �ð����� ������ ����� �ʹ�.
        // 1. ��� �ð��� �帥��.
        currentTime += Time.deltaTime;

        // 2. ��� �ð��� ���� �ð��� �ʰ��ߴٸ�
        if (Input.GetButtonDown("Fire1") && currentTime > createTime)
        {
            // 2) ���콺�� ���ϴ� �������� �ü� �����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();

            // 2. ���콺�� ��ġ�� �ٴ� ���� ��ġ�� �ִٸ�
            if (Physics.Raycast(ray, out hitInfo))
            {
                // ������Ʈ Ǯ �̿��ϱ�
                // 1. ���� ������Ʈ Ǯ�� ���ӿ�����Ʈ�� �ִٸ�
                if (objPool.Count > 0)
                {
                    // �������� ���� ��� �ð��� �ʱ�ȭ���ش�.
                    currentTime = 0;
                    // 2. ������Ʈ Ǯ���� ���ӿ�����Ʈ �ϳ� �����´�.
                    GameObject obbj = objPool[0];
                    // 3. ���ӿ�����Ʈ�� Ȱ��ȭ�Ѵ�.
                    obbj.SetActive(true);
                    /* // 4. ������ ��ġ�ϰ� �ʹ�.
                     obbj.transform.position = hitInfo.point;*/
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        obbj.transform.position = new Vector3((int)hitInfo.point.x, hitInfo.point.y + 0.5f, (int)hitInfo.point.z);
                    }
                    // 5. ������Ʈ Ǯ���� ���ӿ�����Ʈ�� �����Ѵ�.
                    objPool.RemoveAt(0);
                    
                }
            }
        }
    }

}

