using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType // 아이템 타입
{
    Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
}

public class Item : ScriptableObject
{
    
    [SerializeField]
    public ItemType _type;
    [SerializeField]
    public int _id; // 인덱스
    [SerializeField]
    public string _itemName; // 아이템 이름
    [SerializeField]
    public Sprite _itemImage; // 아이템 대표 이미지
    [SerializeField]
    public GameObject _itemPrefab; // 바닥에 떨어질 때 생성할 프리팹
    [SerializeField]
    public string _itemTooltip; // 아이템 설명
    [SerializeField]
    public int _maxAmount = 1;
}
