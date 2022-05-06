using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment/Weapon", order = 2)]
public class WeaponItemData : EquipmentItemData
{
    /// <summary> °ø°Ý·Â </summary>
    public int Damage => _damage;

    [SerializeField] private int _damage = 1;

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}
