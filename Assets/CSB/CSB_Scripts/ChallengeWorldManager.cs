using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeWorldManager : MonoBehaviour
{
    public GameObject chunk;
    public int myIndex; // 내가 입장한 차례, 0~4 의 int 중 하나

    List<GameObject> chunks = new List<GameObject>();   // 전체 땅 리스트
    public List<GameObject> myGround = new List<GameObject>(); // 내 땅 리스트(chunk들 넣어줄거임)

    void Start()
    {
        // 내가 들어온 인덱스 랜덤으로 
        myIndex = Random.Range(0, 4);

        // 쿼드(1칸)들 Chunk(4칸) 단위로 다 뿌리기
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                //Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity);
                chunks.Add(Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity));   // 전체 땅 리스트에 1씩

            }
        }

        // A, B, C, D 섹션 구분
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
