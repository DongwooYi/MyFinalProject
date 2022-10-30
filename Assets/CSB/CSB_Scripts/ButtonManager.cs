using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


// UI ���� ��ư�� ����
public class ButtonManager : MonoBehaviour
{
    public ButtonManager instance;
    /* �κ��丮 ���� */
    public GameObject inventoryPanel;

    private bool isInventory;

    /* ���� ���� */
    public GameObject certificatePanel;

    public bool isCertificate;

    /* ���� Ȯ�� & ������ ���� */
    public GameObject rewardPanel;
    public bool isPass = false;

    public List<Item> itemList = new List<Item>();   // 2D ������ ����Ʈ
    //public List<Item> rewardList = new List<Item>();   // ���� ������ �κ��丮�� �ִ� ������ ����Ʈ

    InventoryManager inventoryManager;  // InventoryManager �������� 

    /* ������ ������ ���� */
    public float minClickTime = 2f; // �ּ� Ŭ�� �ð�

    public float clickTime;    // Ŭ�� �� �ð�
    public bool isClick;   // Ŭ�� �� �Ǵ�

    [Header("카메라_UI")]
    public GameObject Btncam;

    private void Awake()
    {
        instance = this;
      
    }

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
        if (isClick)
        {
            clickTime += Time.deltaTime;
        }
        else
        {
            clickTime = 0;
        }
        
    }
  
    // �κ��丮 ��ư
    // ���� �κ��丮 ��ư�� Ŭ���ϸ� isInventory = true
    public void InventoryButtonManager()
    {
        if (isInventory)    // ���� �κ��丮 â�� ���� �ִٸ�
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


    // 인증버튼(카메라 씬으로) 관련
    public void CertificateButton()
    {
        if (isCertificate)    // ���� ���� â�� ���� �ִٸ�
        {
            certificatePanel.SetActive(false);
            rewardPanel.SetActive(false);
            isCertificate = false;
        }
        else
        {
            certificatePanel.SetActive(true);
            isCertificate = true;
        }
    }

    // ==========================================================================================================
    // 아이템 배치 관련
    // 빌딩시스템
    PlaceableObject objectToPlace;
    public GameObject[] reward3DFactory = new GameObject[4];   // 3D 아이템 (배치할 물건) 공장
    public int itemIndex;
    public GameObject YDW_BuildingSystem;
    public void HoldReward()
    {
        // �����ϴ� ������ �̸�
        itemIndex = itemList.IndexOf(itemList[btnIndex]);   // 2D ������ �� �ε��� ã��

        // �κ��丮���� ������ ����(slot �� index �޾Ƽ�)
        //rewardList.RemoveAt(btnIndex);
        // �κ��丮 â ����
     
        // ������Ʈ(������) 3D ����
        inventoryPanel.SetActive(false);
        isInventory = false;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hitinfo;

        GameObject reward = Instantiate(reward3DFactory[itemIndex]);  // 3D 생성
        YDW_BuildingSystem.GetComponent<YDW_BuildingSystem>().GetReward();

    }
    string rewardName;
    // ���� ��ư �ε���
    public int btnIndex;
    public void ButtonDown()
    {
        isClick = true;
        print(EventSystem.current.currentSelectedGameObject.name);  // ���� ���� ��ư�� �̸�
        string btnName = EventSystem.current.currentSelectedGameObject.name;

        // slot �� �ε��� �ޱ�
        btnIndex = GameObject.Find("Content").transform.Find(btnName).GetSiblingIndex();
        print(btnIndex);
        //rewardName = rewardList[btnIndex].name;
    }

    public void ButtonUp()
    {
        isClick = false;

        if(clickTime >= minClickTime)
        {            
            HoldReward();
        }
    }

    public void ButtonCamOn()
    {
        Btncam.SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
        //Btncam.gameObject.GetComponent<Canvas>().renderingDisplaySize = new Vector2(1080, 1920);
    }

    // MyGoal 관련
    public void MyGoalUI()
    {
        // 버튼을 누르면 내 목표(MyGoal) 보여줌

    }

    public void GotoCameraScence()
    {
        SceneManager.LoadScene("CameraScene_YDW");
    }

    /*    // ���� Ȯ�� Test (Pass ��ư ������ pass, NonPass ��ư ������ non pass)
    Ray ray;
    RaycastHit hit;
    public void CheckPass()
    {
        isPass = true;
        print(isPass);

        if (isPass)
        {

            *//*            // Unlock 할 땅 선택
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            print("들어오니");
                            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Chunk"))
                            {
                                DestroyImmediate(hit.transform.gameObject);
                                print("땅 Unlock");
                                isPass = false;
                            }
                        }
                        else
                        {

                        }*/


    /*            // ������ ȹ�� �˾� true �� & �κ��丮�� �ֱ�
                rewardPanel.SetActive(true);

                // �������� ������ ����
                int selection = Random.Range(0, itmeList.Count);  // ����
                Item reward = itmeList[selection];    // ������ ����


                // �κ��丮�� ������ �߰�
                // InventoryManager.cs �� items ����Ʈ�� �߰�
                rewardList.Add(reward); // �̰� InventoryManager ���� ����
                //inventoryManager.items.Add(reward);

                // �κ��丮 slot �� ������ �̹����� �߰�


                print("������ ����");
                isPass = false;*//*
}

// isPass �� false ��

}*/


/*
    public void CheckNonPass()
    {
        isPass = false;
    }*/
}
