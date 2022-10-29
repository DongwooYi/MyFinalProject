using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChallengeWorldManager : MonoBehaviour
{
    GameObject player;
    public GameObject chunk;
    public List<GameObject> myGround = new List<GameObject>(); // 내 땅 리스트(chunk들 넣어줄거임)

    public int myIndex; // 내가 입장한 차례, 0~3 의 int 중 하나 (or 1~4)
    public int m;   // 가로 개수
    public int n;   // 세로 개수


    List<GameObject> chunks = new List<GameObject>();   // 전체 땅 리스트
    Vector3 spawnPos;   // 스폰 지점


    void Start()
    {
        // 내가 들어온 인덱스 랜덤으로 ( 포톤 붙이면 아이디/1000 )
        myIndex = Random.Range(0, 4);

        m = 10;
        n = 10;

        player = GameObject.FindGameObjectWithTag("Player");

        // 쿼드(1칸)들 Chunk(4칸) 단위로 다 뿌리기
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //Instantiate(chunk, new Vector3(2 * i - 9, 0.3f, 9 - 2 * j), Quaternion.identity);
                chunks.Add(Instantiate(chunk, new Vector3(2 * i - 9, 0.1f, 9 - 2 * j), Quaternion.identity));   // 전체 땅 리스트에 1씩
            }
        }

        /* 이게 내 최선.......... */
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
                        chunks[i + j].layer = 9;
                    }
                }
                spawnPos = myGround[24].transform.position;
                player.transform.position = spawnPos;
                break;
            case 1:
                // 섹션 구분 B
                for (int i = n * m / 2; i <= (m - 1) * n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                        chunks[i + j].layer = 9;
                    }
                }
                spawnPos = myGround[4].transform.position;
                player.transform.position = spawnPos;
                break;
            case 2:
                // 섹션 구분 C
                for (int i = (int)(n - 1) / 2 + 1; i <= (m - 1) * n / 2; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                        chunks[i + j].layer = 9;
                    }
                }
                spawnPos = myGround[20].transform.position;
                player.transform.position = spawnPos;
                break;
            case 3:
                // 섹션 구분 D
                for (int i = (n * m / 2) + n / 2; i <= (m - 1) * n + n; i += n)
                {
                    for (int j = 0; j <= (int)(n - 1) / 2; j++)
                    {
                        myGround.Add(chunks[i + j]);
                        chunks[i + j].layer = 9;
                    }
                }
                spawnPos = myGround[0].transform.position;
                player.transform.position = spawnPos;
                break;
        }

        for(int i = 0; i < myGround.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                myGround[i].transform.GetChild(j).gameObject.layer = 9;
                myGround[i].transform.GetChild(j).gameObject.GetComponent<MeshRenderer>().material.color = Color.black; // 내 땅은 black 으로
            }
        }
        
    }
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        if (isPass)
        {
            SelectGround();
        }

    }

    public bool isPass = false;
    public float delayTime = 1f;
    float currTime = 0f;
    public void SelectGround()  // Unlock 관련 (Pass)
    {
        panel.SetActive(false);
        isPass = true;
        currTime += Time.deltaTime;

        if(currTime >= delayTime)
        {
            if (isPass)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Chunk"))
                    {
                        DestroyImmediate(hit.transform.gameObject);
                        isPass = false;
                    }
                    else
                    {
                        isPass = true;   
                    }

                }

            }

        }
    }
    public GameObject panel;

    public void Pass()
    {
        //panel = GameObject.Find("GetRewardPanel");
        panel.SetActive(true);
    }
}
