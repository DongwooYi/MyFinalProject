using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MyReviewPanel : MonoBehaviour
{
    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 확인 버튼 (누르면 <다읽은 책목록>에 추가




    // 나가기 버튼 (누르면 저장되지 않음)
    public void OnClickExit()
    {
        // 작성한 리뷰와 평점 초기화... 할 필요가 없네
        // 나 자신 초기화
        Destroy(gameObject);
    }

    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetAuthor(string s)
    {
        author.text = s;
    }
    
    public void SetPublishInfo(string s)
    {
        publishInfo.text = s;
    }

    public void SetIsbn(string s)
    {
        isbn.text = s;
    }


}
