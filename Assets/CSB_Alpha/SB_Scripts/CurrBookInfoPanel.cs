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

    public InputField inputFieldReview; // ���� �Է� ĭ
    public Button btnEnter; // ����ϱ� ��ư


    void Start()
    {

    }

    void Update()
    {
        
    }

    #region �ؽ�Ʈ ���� ����
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
