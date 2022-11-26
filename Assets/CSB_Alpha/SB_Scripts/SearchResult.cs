using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


public class SearchResult : MonoBehaviour
{
    GameObject worldManager;
    List<_MyBookInfo> myBookInfoList = new List<_MyBookInfo>();
    //dlrj
    public Text bookTitle;
    public Text author;
    public Text publishInfo;
    public Text isbn;
    public RawImage thumbnail;
    public Texture thumbnailTexture;
    //public Texture aa;

    public GameObject reviewPanelFactory;
    public GameObject alarmFactory;

    public Transform myDesk;

    public WorldManager2D wm;

    private void Start()
    {
        worldManager = GameObject.Find("WorldManager");
        wm = worldManager.GetComponent<WorldManager2D>();
        
        myBookInfoList = wm.myAllBookListNet;
    }

    /* 책 담기 관련 */
    public void OnClickAddBook()
    {
        //HttpPostMyBook();
        imageData = TexToTex2D(thumbnail.texture).EncodeToJPG();
        StartCoroutine(SendBookData());

/*        _MyBookInfo myBookInfo = new _MyBookInfo();

        myBookInfo.bookName = bookTitle.text;
        myBookInfo.bookAuthor = author.text;
        myBookInfo.bookPublishInfo = publishInfo.text;
        myBookInfo.bookISBN = isbn.text;
        myBookInfo.thumbnail = thumbnail;
        myBookInfo.isDone = false;

        wm.myAllBookListNet.Add(myBookInfo);   // 월드의 myAllBookList 에 추가*/

        // <등록 되었습니다>
        Transform canvas = GameObject.Find("Canvas").transform;
        GameObject go = Instantiate(alarmFactory, canvas);
    }

    #region Text Setting
    public void SetBookTitle(string s)
    {
        bookTitle.text = s;
    }

    public void SetBookAuthor(string s)
    {
        author.text = s;
    }

    public void SetBookPublishInfo(string s)
    {
        publishInfo.text = s;
    }

    public void SetIsbn(string s)
    {
        isbn.text = s;
    }

    public void SetImage(RawImage rawImage)
    {
        thumbnailTexture = rawImage.texture;
    }
    #endregion

    // (바뀐 버전) Http 통신 관련 -------------------
    // 3. 도서 담을 때 호출하는 API    
    IEnumerator SendBookData()
    {
        BookInfo bookInfo = new BookInfo();
        bookInfo.bookName = bookTitle.text;
        bookInfo.bookAuthor = author.text;
        bookInfo.bookISBN = isbn.text;
        bookInfo.bookPublishInfo = publishInfo.text;

        WWWForm www = new WWWForm();
        www.AddBinaryData("bookImg", imageData, "image/jpg");
        www.AddField("bookName", bookTitle.text);
        www.AddField("bookISBN", isbn.text);
        www.AddField("bookPublishInfo", publishInfo.text);
        www.AddField("bookAuthor", author.text);
        

        UnityWebRequest webRequest = UnityWebRequest.Post("http://15.165.28.206:80/v1/records/contain", www);
        webRequest.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("jwt"));
        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            print("성공");
            print(webRequest.result);
        }
        else
        {
            if(webRequest.error == "HTTP/1.1 409 Conflict")
            {
                // 여기서 에러 처리ㄴ
                print("에러");
            }
            Debug.Log(webRequest.error);

        }
    }
    public void Oncompeleted()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://15.165.28.206:80/v1/records/contain");
        
    }
    public byte[] imageData;
    Texture2D TexToTex2D(Texture img)
    {
        RenderTexture rt = new RenderTexture(img.width, img.height, 32);
        Texture2D convertImg = new Texture2D(img.width, img.height);

        Graphics.Blit(img, rt);

        convertImg.ReadPixels(new Rect(0, 0, img.width, img.height), 0, 0);
        return convertImg;
    }

    void OnCompletePostMyBook(DownloadHandler handler)
    {
        // 처리
        JObject jObject = JObject.Parse(handler.text);
        int type = (int)jObject["status"];

        if (type == 200)
        {
            print("통신 성공. 책 담기");
            //wm.myAllBookListNet.Add();
        }
        else if(type == 409)
        {
            // <담겨있는 책입니다>
        }
    }

}
