using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastBookInfoPanel : MonoBehaviour
{
    public int myIndex;

    public RawImage thumbnail;
    public Texture texture;

    public Text bookTitle;
    public Text bookAuthor;
    public Text bookIsbn;
    public Text bookInfo;
    public Text bookRating;
    public Text bookReview;

    public bool isBest; // ���� �λ�å�ΰ�
    bool temp;

    Transform contentDoneBook;

    private void Start()
    {
        contentDoneBook = GameObject.Find("MyPastBookPanel/Scroll View_Done/Viewport/Content").transform;
    }
    public void OnCheck(bool checkBool)
    {
        temp = isBest;  // ���� �� ����
        isBest = checkBool; // ���� ��
    }

    // ��� ��ư
    List<_MyBookInfo> bookChanged = new List<_MyBookInfo>();
    public void OnClickSetBestBook()
    {
        // ��ư Ŭ���ϸ� �λ�å ����
        // ���� �ε����� �ش��ϴ� å�� isBest �ٲٰ�
        contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBest = isBest;
        // ����
        for (int i = 0; i < contentDoneBook.childCount; i++)
        {
            int count = 0;
            if (contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isBest)
            {
                count++;
            }
        }
        // MyPastBookPanel ���� ����Ǿ� �ִ� �λ�å �� �޾ƿͼ� 
        // ���� 3���� 0�� �ε��� �����
        // �߰�
        // MyPastBookPanel ���� �λ�å ���� �Ѱ���
    }

    public void OnClickExit()
    {
        Destroy(gameObject);
    }

    public void SetMyIndex(int num)
    {
        myIndex = num;  // �ٽ� ���� �Ѱ��ֱ� ����
    }

    public void SetTitle(string s)
    {
        bookTitle.text = s;
    }

    public void SetAuthor(string s)
    {
        bookAuthor.text = s;
    }

    public void SetIsbn(string s)
    {
        bookIsbn.text = s;
    }

    public void SetInfo(string s)
    {
        bookInfo.text = s;
    }

    public void SetRating(string s)
    {
        bookRating.text = s;
    }

    public void SetReview(string s)
    {
        bookReview.text = s;
    }

    public void SetThumbnail(Texture texture)
    {
        thumbnail.texture = texture;
    }

    public void SetBestBook(bool best)
    {
        isBest = best;
    }
}
