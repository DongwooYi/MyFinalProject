using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeWorldManager : MonoBehaviour
{
    GameObject player;
    public GameObject chunk;
    public List<GameObject> myGround = new List<GameObject>(); // �� �� ����Ʈ(chunk�� �־��ٰ���)

    public int myIndex; // ���� ������ ����, 0~3 �� int �� �ϳ� (or 1~4)
    public int m;   // ���� ����
    public int n;   // ���� ����


    List<GameObject> chunks = new List<GameObject>();   // ��ü �� ����Ʈ
    Vector3 spawnPos;   // ���� ����


    void Start()
    {
        // ���� ���� �ε��� �������� ( ���� ���̸� ���̵�/1000 )
        myIndex = Random.Range(0, 4);

        m = 10;
        n = 10;

        player = GameObject.FindGameObjectWithTag("Player");

        // ����(1ĭ)�� Chunk(4ĭ) ������ �� �Ѹ���
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity);
                chunks.Add(Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity));   // ��ü �� ����Ʈ�� 1��
            }
        }

        /* �̰� �� �ּ�.......... */
        // A, B, C, D ���� ����
        switch (myIndex)
        {
            case 0:
                // ���� ���� A
                for (int i = 0; i <= n * (int)(m - 1) / 2; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                spawnPos = myGround[24].transform.position;
                player.transform.position = spawnPos;
                break;
            case 1:
                // ���� ���� B
                for (int i = n * m / 2; i <= (m - 1) * n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                spawnPos = myGround[4].transform.position;
                player.transform.position = spawnPos;
                break;
            case 2:
                // ���� ���� C
                for (int i = n * (int)(m - 1) / 2; i <= (m - 1) * n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                spawnPos = myGround[20].transform.position;
                player.transform.position = spawnPos;
                break;
            case 3:
                // ���� ���� D
                for (int i = (n * m / 2) + n / 2; i <= (m - 1) * n + n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                spawnPos = myGround[0].transform.position;
                player.transform.position = spawnPos;
                break;
        }

    }

    void Update()
    {

    }
}
