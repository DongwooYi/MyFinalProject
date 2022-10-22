using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// UI ���� ��ư�� ����
public class ButtonManager : MonoBehaviour
{
    /* �κ��丮 ���� */
    public GameObject inventoryPanel;

    private bool isInventory;

    /* ���� ���� */
    public GameObject certificatePanel;

    public bool isCertificate;

    /* ���� Ȯ�� & ������ ���� */
    public GameObject rewardPanel;
    public bool isPass = false;

    public List<Item> itmeList = new List<Item>();   // 2D ������ ����Ʈ
    public List<Item> rewardList = new List<Item>();   // ���� ������ �κ��丮�� �ִ� ������ ����Ʈ

    InventoryManager inventoryManager;  // InventoryManager �������� 

    /* ������ ������ ���� */
    public float minClickTime = 2f; // �ּ� Ŭ�� �ð�

    private float clickTime;    // Ŭ�� �� �ð�
    public bool isClick;   // Ŭ�� �� �Ǵ�


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
        // ������ 3D ����
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


    // ���� ��ư
    public void CertificateButton()
    {
        if (isCertificate)    // ���� ���� â�� ���� �ִٸ�
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

    // ���� Ȯ�� Test (Pass ��ư ������ pass, NonPass ��ư ������ non pass)
    public void CheckPass()
    {
        isPass = true;

        if (isPass)
        {
            // ������ ȹ�� �˾� true �� & �κ��丮�� �ֱ�
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
            isPass = false;
        }

        // isPass �� false ��

    }

    public void CheckNonPass()
    {
        isPass = false;
    }

    // ==========================================================================================================
    // <<<<�κ��丮���� ������Ʈ ������ ����>>>>
    // (���� �ð� ���� ������ �κ��丮���� ������Ʈ ���� & �κ��丮 â ����)
    // ���� ��ġ�� ���� ������ �κ��丮 â�� �ٽ� ���� & �κ��丮 â ����
    // ���� ��ġ�� �Ǿ��ٸ� �κ��丮 â ����

    public GameObject[] reward3DFactory = new GameObject[4];   // 3D ������ ���丮
    public void HoldReward()
    {
        // �����ϴ� ������ �̸�
        int rewardIndex = itmeList.IndexOf(rewardList[btnIndex]);   // 2D ������ �� �ε��� ã��
        print(rewardName);

        // �κ��丮���� ������ ����(slot �� index �޾Ƽ�)
        rewardList.RemoveAt(btnIndex);
        // �κ��丮 â ����
        inventoryPanel.SetActive(false);
        isInventory = false;

        // ������Ʈ(������) 3D ����
        GameObject reward = Instantiate(reward3DFactory[rewardIndex]);

        // ���� Ground �� ��ġ�ߴٸ�

        

    }
    string rewardName;
    // ���� ��ư �ε���
    private int btnIndex;
    public void ButtonDown()
    {
        isClick = true;
        print(EventSystem.current.currentSelectedGameObject.name);  // ���� ���� ��ư�� �̸�
        string btnName = EventSystem.current.currentSelectedGameObject.name;

        // slot �� �ε��� �ޱ�
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
