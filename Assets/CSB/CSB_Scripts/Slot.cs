using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image image;    // itemImage

    private Item _item;

    public Item item
    {
        get { return _item; }   // slot 의 item 정보를 넘겨줌
        set
        {
            _item = value;  // item 에 들어오는 정보값 _item 에 저장
            if(_item != null)   // 만약 item 이 있다면
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else    // 없다면, 투명하게
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
