using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

// å�忡 �پ��ִ� �ڵ�
public class MyBestBook : MonoBehaviour
{
    public List<string> link = new List<string>();
    public string[] lonkArray;

    public List<Texture> linkTex = new List<Texture>();

    public Texture[] texArray;
    private void Start()
    {
        link.Add("https://thehabit.s3.ap-northeast-2.amazonaws.com/record/cba2334f9a504ca595325b2df6fefa7e");
        link.Add("https://thehabit.s3.ap-northeast-2.amazonaws.com/record/2188885e1b1141a6a67d3c3194840efe");
        link.Add("https://thehabit.s3.ap-northeast-2.amazonaws.com/record/e843b5d709fa47ac84a264966e51f084");
        link.Add("https://thehabit.s3.ap-northeast-2.amazonaws.com/record/102d7865d7ac4a4cb08543d858268283");

    }
    public void OnClick()
    {
        GETThumbnailTexture();

    }

    void GETThumbnailTexture()
    {
        StartCoroutine(GetThumbnailImg(link.ToArray()));
    }

    IEnumerator GetThumbnailImg(string[] url)
    {
        for (int i = 0; i < url.Length; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url[i]);
            yield return www.SendWebRequest();


            if (www.result != UnityWebRequest.Result.Success)
            {
                print("����");
                break;
            }
            else
            {

                Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                linkTex.Add(myTexture);
                /*for (int i = 0; i < link.Count; i++)
                {
                    texArray[i] = myTexture;
                }*/
                print("������ ���� " + www.downloadHandler.text);
                //rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                //rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                //texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                //print(rawImage);
            }
            yield return null;
        }
         


    }

    /*// Toggle ��� ������ List
    public Dictionary<int, bool> toggles = new Dictionary<int, bool>();
    public List<bool> toggleList = new List<bool>();
    
    public GameObject player;   // �÷��̾�
    public GameObject myPastBookPanel;  // ������ å ��� UI

    public float distance = 1.5f;   // �÷��̾�� ��ü�� �Ÿ�

    public int idx; // ������ BestBook �������� �ε���

    void Start()
    {
        player = GameObject.Find("Character");     
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < distance)
        {
            ShowClickHereBestBook();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    GameObject me;
    Texture texture;
    public void ShowClickHereBestBook()
    {
        // �հ��� ���带 ����ش�
        transform.GetChild(0).gameObject.SetActive(true);
        // �հ��� ���� �׻� ī�޶� ����
        transform.GetChild(0).forward = Camera.main.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            //if (Physics.Raycast(ray, out hitInfo))
            //{
                //if (hitInfo.transform.gameObject.tag == "ClickHere")
                //{
                    myPastBookPanel.SetActive(true);
                    // Ŭ���� ģ���� �ε���(Ű ��)�� �ش��ϴ� ����� isOn ������ value �� change
                    me = EventSystem.current.currentSelectedGameObject;
            // ���� me == BestBook(Clone) �̸� toggles �ε��� ���� ó��
            print("���� �̸���: " + me.name);
            if (me.name.Contains("BestBook"))
            {
                texture = me.transform.GetChild(1).gameObject.GetComponent<RawImage>().texture;

                idx = me.transform.GetSiblingIndex();
                print("���� �ε��� " + idx);

                //toggles[idx] = me.GetComponent<Toggle>().isOn;
                toggleList[idx] = me.GetComponent<Toggle>().isOn;
               
                //Debug.Log("toggles[idx] = me.GetComponent<Toggle>().isOn;" + toggles[idx] + ":" + me.GetComponent<Toggle>().isOn);
            }

                    // ��� �����ϸ� 
            print("���� ��� ���");

                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                //}
            //}
        }
    }

    // Ȯ�� ��ư
    public void OnClickSetBestBook()
    {
        print("1111111");
        for(int i = 0; i < toggles.Count; i++)
        {
            // ���� value ���� true ��
            if (toggles.Values.ToList()[i])
            {
                // �ش��ϴ� 
                GameObject bestBook = transform.GetChild(i).gameObject;
                bestBook.SetActive(true);
                bestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
            }
        }
    }*/
}
