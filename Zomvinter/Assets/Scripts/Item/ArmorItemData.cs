using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Armor_", menuName = "Inventory System/Item Data/Armor", order = 1)]
public class ArmorItemData : EquipmentItemData
{
    /// <summary> ��(������ ��) </summary>
    public float Armor => _armor;
    [SerializeField] private float _armor; // ����
    public float Durability => _durability;
    [SerializeField] private float _durability; // ������
}
