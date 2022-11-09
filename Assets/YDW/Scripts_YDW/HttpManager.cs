using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager : MonoBehaviour
{
    public static HttpManager instance;

    private void Awake()
    {
        //만약에 instance가 null이라면
        if (instance == null)
        {
            //instance에 나를 넣겠다.
            instance = this;
            //씬이 전환이 되어도 나를 파괴되지 않게 하겠다.
            DontDestroyOnLoad(gameObject);
        }
        //그렇지 않으면
        else
        {
            //나를 파괴하겠다.
            Destroy(gameObject);
        }
    }

    //서버에게 요청
    //url(posts/1), GET
    public void SendRequest(HttpRequester requester)
    {
        StartCoroutine(Send(requester));
    }

    IEnumerator Send(HttpRequester requester)
    {
        UnityWebRequest webRequest = null;
        //requestType 에 따라서 호출해줘야한다.
        switch (requester.requestType)
        {

            case RequestType.POST:
                webRequest = UnityWebRequest.Post(requester.url, requester.body);
                byte[] data = Encoding.UTF8.GetBytes(requester.body);
                webRequest.uploadHandler = new UploadHandlerRaw(data);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("x-access-token", PlayerPrefs.GetString("jwt"));
                yield return webRequest.SendWebRequest();
                //만약에 응답이 성공했다면
                if (webRequest.result == UnityWebRequest.Result.Success)
                {                    
                    requester.onComplete(webRequest.downloadHandler);
                }
                else
                {
                    requester.onFailed();
                    print("통신 실패");
                }
                break;
            case RequestType.GET:
                webRequest = UnityWebRequest.Get(requester.url);
                //서버에 요청을 보내고 응답이 올때까지 기다린다.
                yield return webRequest.SendWebRequest();

                //만약에 응답이 성공했다면
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    requester.onComplete(webRequest.downloadHandler);
                }
                else
                {
                    requester.onFailed();
                    print("통신 실패");
                }
                break;
          
        }
        yield return null;      
    }
}
