using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

// 책장에 붙어있는 코드
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
                print("실패");
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
                print("데이터 형식 " + www.downloadHandler.text);
                //rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                //rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                //texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                //print(rawImage);
            }
            yield return null;
        }
         


    }

    /*// Toggle 들로 구성된 List
    public Dictionary<int, bool> toggles = new Dictionary<int, bool>();
    public List<bool> toggleList = new List<bool>();
    
    public GameObject player;   // 플레이어
    public GameObject myPastBookPanel;  // 다읽은 책 목록 UI

    public float distance = 1.5f;   // 플레이어와 물체의 거리

    public int idx; // 생성된 BestBook 프리펩의 인덱스

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
        // 손가락 쿼드를 띄워준다
        transform.GetChild(0).gameObject.SetActive(true);
        // 손가락 쿼드 항상 카메라 방향
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
                    // 클릭된 친구의 인덱스(키 값)에 해당하는 토글의 isOn 값으로 value 값 change
                    me = EventSystem.current.currentSelectedGameObject;
            // 만약 me == BestBook(Clone) 이면 toggles 인덱스 관련 처리
            print("나의 이름은: " + me.name);
            if (me.name.Contains("BestBook"))
            {
                texture = me.transform.GetChild(1).gameObject.GetComponent<RawImage>().texture;

                idx = me.transform.GetSiblingIndex();
                print("나의 인덱스 " + idx);

                //toggles[idx] = me.GetComponent<Toggle>().isOn;
                toggleList[idx] = me.GetComponent<Toggle>().isOn;
               
                //Debug.Log("toggles[idx] = me.GetComponent<Toggle>().isOn;" + toggles[idx] + ":" + me.GetComponent<Toggle>().isOn);
            }

                    // 토글 선택하면 
            print("도서 목록 출력");

                    transform.GetChild(0).gameObject.SetActive(false);
                    return;
                //}
            //}
        }
    }

    // 확인 버튼
    public void OnClickSetBestBook()
    {
        print("1111111");
        for(int i = 0; i < toggles.Count; i++)
        {
            // 만약 value 값이 true 면
            if (toggles.Values.ToList()[i])
            {
                // 해당하는 
                GameObject bestBook = transform.GetChild(i).gameObject;
                bestBook.SetActive(true);
                bestBook.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
            }
        }
    }*/
}
