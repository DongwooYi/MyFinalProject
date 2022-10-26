using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeWorldManager : MonoBehaviour
{
    public GameObject chunk;
    public int myIndex; // 내가 입장한 차례, 0~3 의 int 중 하나 (or 1~4)

    List<GameObject> chunks = new List<GameObject>();   // 전체 땅 리스트
    GameObject[,] test; // 전체 땅 2차원 배열로
    public int people;
    public int day;

    public int m;   // 가로 개수
    public int n;   // 세로 개수

    public List<GameObject> myGround = new List<GameObject>(); // 내 땅 리스트(chunk들 넣어줄거임)

    void Start()
    {
        // 내가 들어온 인덱스 랜덤으로 
        myIndex = Random.Range(0, 4);

        m = 10;
        n = 10;

        // 쿼드(1칸)들 Chunk(4칸) 단위로 다 뿌리기
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity);
                chunks.Add(Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity));   // 전체 땅 리스트에 1씩
            }
        }


        // A, B, C, D 섹션 구분
        switch (myIndex)
        {
            case 0:
                // 섹션 구분 A
                for (int i = 0; i <= n * (int)(m - 1) / 2; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                break;
            case 1:
                // 섹션 구분 B
                for (int i = n * m / 2; i <= (m - 1) * n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                break;
            case 2:
                // 섹션 구분 C
                for (int i = n * (int)(m - 1) / 2; i <= (m - 1) * n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                break;
            case 3:
                // 섹션 구분 D
                for (int i = (n * m / 2) + n / 2; i <= (m - 1) * n + n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                    }
                }
                break;
        }

    }

    void Update()
    {

    }
}
