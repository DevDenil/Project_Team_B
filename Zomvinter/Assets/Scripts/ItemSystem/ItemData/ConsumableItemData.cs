using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �Һ� ������ ���� </summary>
[CreateAssetMenu(fileName = "Item_Consumable_", menuName = "Inventory System/Item Data/Countable/Consumabler", order = 1)]
public class ConsumableItemData : CountableItemData
{
    /// <summary> ȿ����(ȸ���� ��) </summary>
    public float Value => _value;
    [SerializeField] private float _value;
    public override Item CreateItem()
    {
        return new ConsumableItem(this);
    }
}
