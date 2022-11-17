using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 리워드(오브젝트) 관리

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
}
