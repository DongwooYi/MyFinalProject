using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeWorldManager : MonoBehaviour
{
    public GameObject chunk;
    public int myIndex; // ���� ������ ����, 0~4 �� int �� �ϳ�

    List<GameObject> chunks = new List<GameObject>();   // ��ü �� ����Ʈ
    public List<GameObject> myGround = new List<GameObject>(); // �� �� ����Ʈ(chunk�� �־��ٰ���)

    void Start()
    {
        // ���� ���� �ε��� �������� 
        myIndex = Random.Range(0, 4);

        // ����(1ĭ)�� Chunk(4ĭ) ������ �� �Ѹ���
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                //Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity);
                chunks.Add(Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity));   // ��ü �� ����Ʈ�� 1��

            }
        }

        // A, B, C, D ���� ����
/*        switch (myIndex)
        {
            case 0:
                for(int i = chunks.Count / 4 * myIndex; i < chunks.Count / 4 * (myIndex+1); i++)
                {
                    myGround.Add(chunks[i]);
                }

                break;
            case 1:
                for (int i = chunks.Count / 4 * myIndex; i < chunks.Count / 4 * (myIndex + 1); i++)
                {
                    myGround.Add(chunks[i]);
                }
                break;
            case 2:
                for (int i = chunks.Count / 4 * myIndex; i < chunks.Count / 4 * (myIndex + 1); i++)
                {
                    myGround.Add(chunks[i]);
                }
                break;
            case 3:
                for (int i = chunks.Count / 4 * myIndex; i < chunks.Count / 4 * (myIndex + 1); i++)
                {
                    myGround.Add(chunks[i]);
                }
                break;
        }*/

    }

    void Update()
    {
        
    }
}
