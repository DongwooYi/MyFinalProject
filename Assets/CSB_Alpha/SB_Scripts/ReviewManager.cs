using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ���� ��� ����
// ���� �����ϸ� �ٷ� ����
public class ReviewManager : MonoBehaviour
{
    public TextMeshPro title;
    public TextMeshPro nickname;
    public TextMeshPro oneLineReview;
    public GameObject thumbnail;    // �� ģ�� ������ -> ���� MeshRenderer �� material�� texture �� �ٲٸ� ��

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetNickname(string s)
    {
        nickname.text = s;
    }

    public void SetReview(string s)
    {
        oneLineReview.text = s;
    }

    public void SetThumbnail(Texture texture)
    {
        Material mat = thumbnail.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_MainTex", texture);
    }
}
