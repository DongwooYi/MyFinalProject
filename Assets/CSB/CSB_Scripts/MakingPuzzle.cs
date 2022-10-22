using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingPuzzle : MonoBehaviour
{
    public List<Material> puzzleMats = new List<Material>();    // 퍼즐 쪼개기
    public List<GameObject> puzzlePieces = new List<GameObject>();   // 퍼즐 조각들

    public int people = 4;  // 참가 인원
    public int date = 4;    // 기간

    void Start()
    {
        for(int i = 0; i < people; i++) // 가로
        {
            for(int j = 0; j < date; j++)
            {
                //puzzleMats[i*(j+1)]
            }
        }

    }

    void Update()
    {
        
    }
}
