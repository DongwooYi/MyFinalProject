using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �κ��丮
public class InventoryManager : MonoBehaviour
{
    // ButtonManager�� itemList ��������
    public List<Item> itemList = new List<Item>();

    public List<Item> items = new List<Item>();    // ButtonManager �� itemList �����ؾ��ҵ�..?

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
        itemList = GameObject.Find("Canvas").GetComponent<ButtonManager>().itemList;    // ButtonManager �� itemList ������
    }

    private void Update()
    {
        items = itemList;
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
