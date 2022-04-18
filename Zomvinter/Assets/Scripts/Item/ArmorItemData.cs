using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Armor_", menuName = "Inventory System/Item Data/Armor", order = 1)]
public class ArmorItemData : EquipmentItemData
{
    /// <summary> 방어도(내구도 등) </summary>
    public float Armor => _armor;
    [SerializeField] private float _armor; // 방어력
    public float Durability => _durability;
    [SerializeField] private float _durability; // 내구도
}
