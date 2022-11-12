using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrBookInfoPanel : MonoBehaviour
{
    public Text title;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;

    public Dropdown dropdown;

    public InputField inputFieldReview; // 리뷰 입력 칸
    public Button btnEnter; // 등록하기 버튼


    void Start()
    {

    }

    void Update()
    {
        
    }

    #region 텍스트 세팅 관련
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

    public void OnClickExit()
    {
        Destroy(gameObject);
    }

    /*    public void SetImage(Texture texture)
        {
            thumbnail.texture = texture;
        }*/
    #endregion
}
