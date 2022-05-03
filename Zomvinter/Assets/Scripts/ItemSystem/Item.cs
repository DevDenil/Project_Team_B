using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType // ������ Ÿ��
{
    Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
}

public class Item : ScriptableObject
{
    
    [SerializeField]
    public ItemType _type;
    [SerializeField]
    public int _id; // �ε���
    [SerializeField]
    public string _itemName; // ������ �̸�
    [SerializeField]
    public Sprite _itemImage; // ������ ��ǥ �̹���
    [SerializeField]
    public GameObject _itemPrefab; // �ٴڿ� ������ �� ������ ������
    [SerializeField]
    public string _itemTooltip; // ������ ����
    [SerializeField]
    public int _maxAmount = 1;
}
