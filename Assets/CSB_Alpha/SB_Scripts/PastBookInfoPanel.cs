using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastBookInfoPanel : MonoBehaviour
{
    public RawImage thumbnail;
    public Texture texture;

    public Text bookTitle;
    public Text bookAuthor;
    public Text bookIsbn;
    public Text bookInfo;
    public Text bookRating;
    public Text bookReview;

    public bool isBest;

    public void OnClickExit()
    {
        Destroy(gameObject);
    }

    public void SetTitle(string s)
    {
        print(s);
        bookTitle.text = s;
    }

    public void SetAuthor(string s)
    {
        print(s);
        bookAuthor.text = s;
    }

    public void SetIsbn(string s)
    {
        print(s);
        bookIsbn.text = s;
    }

    public void SetInfo(string s)
    {
        print(s);
        bookInfo.text = s;
    }

    public void SetRating(string s)
    {
        print(s);
        bookRating.text = s;
    }

    public void SetReview(string s)
    {
        print(s);
        bookReview.text = s;
    }

    public void SetThumbnail(Texture texture)
    {
        thumbnail.texture = texture;
    }

    public void SetBestBook(bool best)
    {
        print(best);
        isBest = best;
    }
}
