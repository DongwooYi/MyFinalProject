using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager : MonoBehaviour
{
    public static HttpManager instance;

    private void Awake()
    {
        //���࿡ instance�� null�̶��
        if (instance == null)
        {
            //instance ���� �ְڴ�
            instance = this;
            //���� ��ȯ�� �Ǿ ���� �ı����� �ʰ� �ϰڴ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //�׷��� ������
            //���� �ı��ϰڴ�
            print("destroy");
            Destroy(gameObject);
        }

    }

    //�������� ��û
    //url(/posts/1).Get
    public void SendRequest(HttpRequester requester)
    {
        print("�ּҷ� ����!!");
        StartCoroutine(Send(requester));
    }

    IEnumerator Send(HttpRequester requester)
    {
        UnityWebRequest webRequest = null;

        switch (requester.requestType)
        {
            case RequestType.POST:
                using (webRequest = UnityWebRequest.PostWwwForm(requester.url, requester.data))
                {
                    print("post ����");
                    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requester.data);
                    webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                    webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                    webRequest.SetRequestHeader("x-access-token", PlayerPrefs.GetString("jwt"));
                    yield return webRequest.SendWebRequest();

                    //���࿡ ������ �����ߴٸ�
                    if (webRequest.result == UnityWebRequest.Result.Success)
                    {
                        // print(webRequest.downloadHandler.text);
                        requester.onComplete(webRequest.downloadHandler);
                    }
                    else
                    {
                        print("��� ����");
                    }
                }
                break;
            case RequestType.GET:
                webRequest = UnityWebRequest.Get(requester.url);
                yield return webRequest.SendWebRequest();

                //���࿡ ������ �����ߴٸ�
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // print(webRequest.downloadHandler.text);
                    requester.onComplete(webRequest.downloadHandler);
                }
                else
                {
                    print("��� ����");
                }
                break;
        }

        yield return null;
    }
}
