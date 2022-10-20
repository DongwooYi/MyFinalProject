using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �κ��丮
public class InventoryManager : MonoBehaviour
{
    // ButtonManager�� rewardList ��������
    public List<Item> rewardedItems = new List<Item>();

    public List<Item> items = new List<Item>();    // ButtonManager �� rewardList�� �����ؾ��ҵ�..?

    [SerializeField]
    private Transform content;

    [SerializeField]
    private Slot[] slots;

    // Run ���� �ʾƵ� ������ �󿡼� ������ �ȴٰ� ��..
    private void OnValidate()
    {
        slots = content.GetComponentsInChildren<Slot>();
    }

    private void Awake()
    {
        SetSlot();  // items �� �ִ� �����带 �κ��丮�� �־���
    }

    private void Start()
    {
        rewardedItems = GameObject.Find("Canvas").GetComponent<ButtonManager>().rewardList;
    }

    private void Update()
    {
        items = rewardedItems;
        SetSlot();
    }

    // Slot �����ؼ� ������ (setting)
    public void SetSlot()
    {
        int i = 0;  // �Ʒ� �� for ������ ���� i �� ���

        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }

        for(; i < slots.Length; i++)
        {
            slots[i].item = null;   // �� slot null ó��
        }
    }

    public void AddItem(Item _item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            SetSlot();
        }
        else
        {
            print("Slot Full");
        }
    }
}
