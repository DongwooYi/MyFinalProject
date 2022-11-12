using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadGallery : MonoBehaviour
{
    public GameObject lobbyManager;
    public RawImage image;
    public void OnClickImageLoad()
    {
        
        NativeGallery.GetImageFromGallery((file)=> 
        { 
            //이미지 정보
            FileInfo selected = new FileInfo(file);

            //이미지 용량 제한(나중의 문제 생길수있기에 예방)
            if(selected.Length>5000000)
            {
                return;
            }

            if(!string.IsNullOrEmpty(file))
            {
                // 불러오기
                StartCoroutine(LoadImage(file));
            }

        });
    }
IEnumerator LoadImage(string path)
    {
        yield return null;
        byte[] fileData = File.ReadAllBytes(path);
        // 확장자의 이름 은 필요없음
        string fileName = Path.GetFileName(path).Split('.')[0];
        //설정된 이미지
        string savePath = Application.persistentDataPath + "/Image";
        // 만약 내가 지정한 저장 경로가 없다면 지정 경로를 만들어라
        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        // 내가 원하는 장소의 PNG 형식의 파일 이름으로 저장
        File.WriteAllBytes(savePath + fileName + ".png", fileData);

       var temp = File.ReadAllBytes(savePath + fileName + ".png");
        Texture2D tex = new Texture2D(0,0);
        tex.LoadImage(temp);

        image.texture = tex;
       
    }
    // Start is called before the first frame update
    void Start()
    { 
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
