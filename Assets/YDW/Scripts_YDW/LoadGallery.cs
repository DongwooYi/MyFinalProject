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
            //�̹��� ����
            FileInfo selected = new FileInfo(file);

            //�̹��� �뷮 ����(������ ���� ������ֱ⿡ ����)
            if(selected.Length>5000000)
            {
                return;
            }

            if(!string.IsNullOrEmpty(file))
            {
                // �ҷ�����
                StartCoroutine(LoadImage(file));
            }

        });
    }
IEnumerator LoadImage(string path)
    {
        yield return null;
        byte[] fileData = File.ReadAllBytes(path);
        // Ȯ������ �̸� �� �ʿ����
        string fileName = Path.GetFileName(path).Split('.')[0];
        //������ �̹���
        string savePath = Application.persistentDataPath + "/Image";
        // ���� ���� ������ ���� ��ΰ� ���ٸ� ���� ��θ� ������
        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        // ���� ���ϴ� ����� PNG ������ ���� �̸����� ����
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
