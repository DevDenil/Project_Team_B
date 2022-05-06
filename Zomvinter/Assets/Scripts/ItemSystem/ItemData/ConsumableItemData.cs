using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Consumable_", menuName = "Inventory System/Item Data/Countable/Consumabler", order = 1)]
public class ConsumableItemData : CountableItemData
{
    /// <summary> 효과량(회복량 등) </summary>
    public float Value => _value;
    [SerializeField] private float _value;
    public override Item CreateItem()
    {
        return new ConsumableItem(this);
    }
}
