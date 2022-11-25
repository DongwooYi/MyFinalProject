using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 리뷰 배너 관련
// 월드 입장하면 바로 생성
public class ReviewManager : MonoBehaviour
{
    public TextMeshPro title;
    public TextMeshPro nickname;
    public TextMeshPro oneLineReview;
    public GameObject thumbnail;    // 이 친구 쿼드임 -> 얘의 MeshRenderer 의 material의 texture 를 바꾸면 됨

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
