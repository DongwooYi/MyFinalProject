/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class HttpManager : MonoBehaviour
{
    public static HttpManager instance; //하나만 존재하게 하기 위해서 싱글톤으로 만드는 것임.

    public byte image;
    private void Awake()
    {
        //만약에 instance가 null이라면
        if (instance == null)
        {
            //instance 나를 넣겠다
            instance = this;
            //씬이 전환이 되어도 나를 파괴되지 않게 하겠다
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //그렇지 않으면
            //나를 파괴하겠다
            print("destroy");
            Destroy(gameObject);
        }

    }

    //서버에게 요청
    //url(/posts/1).Get
    public void SendRequest(HttpRequester requester)
    {
        print("주소로 접근!!");
        StartCoroutine(Send(requester));
    }

    IEnumerator Send(HttpRequester requester)
    {
        UnityWebRequest webRequest = null;

        switch (requester.requestType)
        {
            case RequestType.POST:
                using (webRequest = UnityWebRequest.Post(requester.url, requester.data))
                {
                    print("post 접근");
                    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requester.data);
                    webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                    webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                    webRequest.SetRequestHeader("x-access-token", PlayerPrefs.GetString("jwt"));
                    yield return webRequest.SendWebRequest();

                    //만약에 응답이 성공했다면
                    if (webRequest.result == UnityWebRequest.Result.Success)
                    {
                        // print(webRequest.downloadHandler.text);
                        requester.onComplete(webRequest.downloadHandler);
                    }
                    else
                    {
                        print("통신 실패");
                    }
                }
                break;
            case RequestType.GET:
                webRequest = UnityWebRequest.Get(requester.url);
                yield return webRequest.SendWebRequest();

                //만약에 응답이 성공했다면
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // print(webRequest.downloadHandler.text);
                    requester.onComplete(webRequest.downloadHandler);
                }
                else
                {
                    print("통신 실패");
                }
                break;
        }

        yield return null;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
*/