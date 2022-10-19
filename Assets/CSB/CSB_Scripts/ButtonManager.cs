using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 상의 버튼들 관리
public class ButtonManager : MonoBehaviour
{
    /* 인벤토리 관련 */
    public GameObject inventoryPanel;

    private bool isInventory;

    /* 인증 관련 */
    public GameObject certificatePanel;

    public bool isCertificate;

    /* 인증 확인 & 리워드 관련 */
    public GameObject rewardPanel;
    public bool isPass = false;

    public List<Item> itmeList = new List<Item>();   // 리워드 리스트
    public List<Item> rewardList = new List<Item>();   // 리워드 리스트

    InventoryManager inventoryManager;  // InventoryManager 가져오기 

    void Start()
    {
        inventoryPanel.SetActive(false);
        isInventory = false;

        isCertificate = false;
        certificatePanel.SetActive(false);

        rewardPanel.SetActive(false);

        inventoryManager = GetComponent<InventoryManager>();
    }


    // 인벤토리 버튼
    // 만약 인벤토리 버튼을 클릭하면 isInventory = true
    public void InventoryButtonManager()
    {
        if (isInventory)    // 만약 인벤토리 창이 열려 있다면
        {
            inventoryPanel.SetActive(false);
            isInventory = false;
        }
        else
        {
            inventoryPanel.SetActive(true);
            isInventory = true;
        }
    }


    // 인증 버튼
    public void CertificateButton()
    {
        if (isCertificate)    // 만약 인증 창이 열려 있다면
        {
            certificatePanel.SetActive(false);
            isCertificate = false;
        }
        else
        {
            certificatePanel.SetActive(true);
            isCertificate = true;
        }
    }

    // 인증 확인 Test (Pass 버튼 누르면 pass, NonPass 버튼 누르면 non pass)
    public void CheckPass()
    {
        isPass = true;

        if (isPass)
        {
            // 리워드 획득 팝업 true 로 & 인벤토리에 넣기
            rewardPanel.SetActive(true);

            // 랜덤으로 리워드 제공
            int selection = Random.Range(0, itmeList.Count);  // 난수
            Item reward = itmeList[selection];    // 리워드 제공


            // 인벤토리에 리워드 추가
            // InventoryManager.cs 의 items 리스트에 추가
            rewardList.Add(reward); // 이걸 InventoryManager 에서 관리
            //inventoryManager.items.Add(reward);

            // 인벤토리 slot 에 리워드 이미지로 추가


            print("리워드 받음");
            isPass = false;
        }

        // isPass 를 false 로

    }

    public void CheckNonPass()
    {
        isPass = false;
    }
}
