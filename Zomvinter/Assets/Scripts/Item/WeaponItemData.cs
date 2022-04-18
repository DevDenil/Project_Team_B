using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Weapon_", menuName = "Inventory System/Item Data/Weapon", order = 1)]
public class WeaponItemData : EquipmentItemData
{

    /// <summary> 공격력(내구도 등) </summary>
    public float Damage => _Damage;
    [SerializeField] private float _Damage; // 공격력
    public float Durability => _durability;
    [SerializeField] private float _durability; // 내구도
    public int Ammunition => _ammunition;
    [SerializeField] private int _ammunition; // 장탄 수
}
