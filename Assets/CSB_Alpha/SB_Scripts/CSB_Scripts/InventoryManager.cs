using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리
public class InventoryManager : MonoBehaviour
{
    // ButtonManager의 itemList 가져오기
    public List<Item> itemList = new List<Item>();

    public List<Item> items = new List<Item>();    // ButtonManager 의 itemList 접근해야할듯..?

    [SerializeField]
    private Transform content;

    [SerializeField]
    private Slot[] slots;

    // Run 하지 않아도 에디터 상에서 실행이 된다고 함..
    private void OnValidate()
    {
        slots = content.GetComponentsInChildren<Slot>();
    }

    private void Awake()
    {
        SetSlot();  // items 에 있는 리워드를 인벤토리에 넣어줌
    }

    private void Start()
    {
        itemList = GameObject.Find("Canvas").GetComponent<ButtonManager>().itemList;    // ButtonManager 의 itemList 가져옴
    }

    private void Update()
    {
        items = itemList;
        SetSlot();
    }

    // Slot 정리해서 보여줌 (setting)
    public void SetSlot()
    {
        int i = 0;  // 아래 두 for 문에서 같은 i 값 사용

        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }

        for(; i < slots.Length; i++)
        {
            slots[i].item = null;   // 빈 slot null 처리
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
