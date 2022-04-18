using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Weapon_", menuName = "Inventory System/Item Data/Weapon", order = 1)]
public class WeaponItemData : EquipmentItemData
{

    /// <summary> ���ݷ�(������ ��) </summary>
    public float Damage => _Damage;
    [SerializeField] private float _Damage; // ���ݷ�
    public float Durability => _durability;
    [SerializeField] private float _durability; // ������
    public int Ammunition => _ammunition;
    [SerializeField] private int _ammunition; // ��ź ��
}
