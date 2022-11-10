using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���⼭ �ǽð� ��ʿ� �ø� ģ�� ���� �޾ƿ���
// ��� ����
// ���� �ð� �� ���� & ���� ����

public class BannerManager : MonoBehaviour
{
    // �� ģ�� ������ ��� ����
    // �ϴ��� myPastBookInfo ����Ʈ�� �ε��� �� ������ ģ���� ������
    // �̱������� �ø��� �ȵǳ�..
    GameObject worldManager;
    List<_MyPastBookInfo> myPastBookInfoList = new List<_MyPastBookInfo>();

    public GameObject bannerFactory;
    GameObject banneritem;

    // ���� ������ ���صѱ� -> �̰� �� ������


    public float bannerTime = 5f;
    float currTime = 0;

    public int idx;

    void Start()
    {
        // AI ���� �޾ƿ��� �� �ӽ÷�
        // ���� �� ���� å list �޾ƿ� -> ���⼭ (��������) ���� �ð����� ����
        // ���� ����Ʈ�� ��������� �ȵ�
        worldManager = GameObject.Find("WorldManager");
        myPastBookInfoList = worldManager.GetComponent<WorldManager2D>().myPastBookList;

        //banneritem = GameObject.FindGameObjectWithTag("BannerItem");
    }

    void Update()
    {
        if(myPastBookInfoList.Count <= 0)
        {
            return;
        }

        currTime += Time.deltaTime;
        if(currTime > bannerTime)
        {
            // ���� �ε��� �ϳ� �̱�
            idx = Random.Range(0, myPastBookInfoList.Count);

            if (banneritem != null)
            {
                // ������ �ִ°� ����
                Destroy(banneritem);

                // ���ο�� ����
                banneritem = Instantiate(bannerFactory);
            }
            else
            {
                banneritem = Instantiate(bannerFactory);
            }
            MakeBanner(banneritem);
            currTime = 0;
        }
    }

    public void MakeBanner(GameObject bannerItem)
    {
        ReviewManager reviewManager = bannerItem.GetComponent<ReviewManager>();

        reviewManager.SetTitle(myPastBookInfoList[idx].title);
        reviewManager.SetReview(myPastBookInfoList[idx].review);
        reviewManager.SetNickname("User Nickname");
        reviewManager.SetThumbnail(myPastBookInfoList[idx].thumbnail.texture);
    }
}
