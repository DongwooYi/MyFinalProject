using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingPuzzle : MonoBehaviour
{
    public List<Material> puzzleMats = new List<Material>();    // ���� �ɰ���
    public List<GameObject> puzzlePieces = new List<GameObject>();   // ���� ������

    public int people = 4;  // ���� �ο�
    public int date = 4;    // �Ⱓ

    void Start()
    {
        for(int i = 0; i < people; i++) // ����
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
