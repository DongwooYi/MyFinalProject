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
        get { return _item; }   // slot �� item ������ �Ѱ���
        set
        {
            _item = value;  // item �� ������ ������ _item �� ����
            if(_item != null)   // ���� item �� �ִٸ�
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else    // ���ٸ�, �����ϰ�
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
