using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public List<Item> itmeList = new List<Item>();   // ������ ����Ʈ
    public List<Item> rewardList = new List<Item>();   // ������ ����Ʈ

    InventoryManager inventoryManager;  // InventoryManager �������� 

    void Start()
    {
        inventoryPanel.SetActive(false);
        isInventory = false;

        isCertificate = false;
        certificatePanel.SetActive(false);

        rewardPanel.SetActive(false);

        inventoryManager = GetComponent<InventoryManager>();
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
}
