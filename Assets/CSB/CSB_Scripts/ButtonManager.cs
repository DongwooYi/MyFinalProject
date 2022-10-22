using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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

    public List<Item> itmeList = new List<Item>();   // 2D 리워드 리스트
    public List<Item> rewardList = new List<Item>();   // 받은 리워드 인벤토리에 넣는 아이템 리스트

    InventoryManager inventoryManager;  // InventoryManager 가져오기 

    /* 리워드 꺼내기 관련 */
    public float minClickTime = 2f; // 최소 클릭 시간

    private float clickTime;    // 클릭 중 시간
    public bool isClick;   // 클릭 중 판단


    void Start()
    {
        inventoryPanel.SetActive(false);
        isInventory = false;

        isCertificate = false;
        certificatePanel.SetActive(false);

        rewardPanel.SetActive(false);

        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        // 리워드 3D 공장
/*        for(int i = 0; i < reward3DFactory.Length; i++)
        {
            //reward3DFactory[i] = 
        }*/

        if (isClick)
        {
            clickTime += Time.deltaTime;
        }
        else
        {
            clickTime = 0;
        }
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

    // ==========================================================================================================
    // <<<<인벤토리에서 오브젝트 꺼내기 관련>>>>
    // (일정 시간 동안 누르면 인벤토리에서 오브젝트 제거 & 인벤토리 창 끄기)
    // 만약 배치가 되지 않으면 인벤토리 창에 다시 넣음 & 인벤토리 창 열기
    // 만약 배치가 되었다면 인벤토리 창 열기

    public GameObject[] reward3DFactory = new GameObject[4];   // 3D 리워드 펙토리
    public void HoldReward()
    {
        // 제거하는 리워드 이름
        int rewardIndex = itmeList.IndexOf(rewardList[btnIndex]);   // 2D 리워드 의 인덱스 찾기
        print(rewardName);

        // 인벤토리에서 리워드 제거(slot 의 index 받아서)
        rewardList.RemoveAt(btnIndex);
        // 인벤토리 창 끄기
        inventoryPanel.SetActive(false);
        isInventory = false;

        // 오브젝트(리워드) 3D 생성
        GameObject reward = Instantiate(reward3DFactory[rewardIndex]);

        // 만약 Ground 를 터치했다면

        

    }
    string rewardName;
    // 눌린 버튼 인덱스
    private int btnIndex;
    public void ButtonDown()
    {
        isClick = true;
        print(EventSystem.current.currentSelectedGameObject.name);  // 현재 눌린 버튼의 이름
        string btnName = EventSystem.current.currentSelectedGameObject.name;

        // slot 의 인덱스 받기
        btnIndex = GameObject.Find("Content").transform.FindChild(btnName).GetSiblingIndex();
        print(btnIndex);
        rewardName = rewardList[btnIndex].name;
    }

    public void ButtonUp()
    {
        isClick = false;

        if(clickTime >= minClickTime)
        {
            HoldReward();
        }
    }
}
