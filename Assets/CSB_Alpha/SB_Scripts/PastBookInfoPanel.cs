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

    public bool isBest; // 내가 인생책인가
    bool temp;

    Transform contentDoneBook;

    private void Start()
    {
        contentDoneBook = GameObject.Find("MyPastBookPanel/Scroll View_Done/Viewport/Content").transform;
    }
    public void OnCheck(bool checkBool)
    {
        temp = isBest;  // 이전 값 저장
        isBest = checkBool; // 변한 값
    }

    // 등록 버튼
    List<_MyBookInfo> bookChanged = new List<_MyBookInfo>();
    public void OnClickSetBestBook()
    {
        // 버튼 클릭하면 인생책 여부
        // 받은 인덱스에 해당하는 책의 isBest 바꾸고
        contentDoneBook.transform.GetChild(myIndex).gameObject.GetComponent<MyBook>().isBest = isBest;
        // 만약
        for (int i = 0; i < contentDoneBook.childCount; i++)
        {
            int count = 0;
            if (contentDoneBook.GetChild(i).gameObject.GetComponent<MyBook>().isBest)
            {
                count++;
            }
        }
        // MyPastBookPanel 에서 저장되어 있는 인생책 수 받아와서 
        // 만약 3개면 0번 인덱스 지우고
        // 추가
        // MyPastBookPanel 에서 인생책 개수 넘겨줌
    }

    public void OnClickExit()
    {
        Destroy(gameObject);
    }

    public void SetMyIndex(int num)
    {
        myIndex = num;  // 다시 값을 넘겨주기 위함
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
