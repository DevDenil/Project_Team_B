using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment", order = 2)]
public class EquipmentData : ItemData
{

    public float Durability => _durability; // ������
    public int Ammunition => _ammunition; // ��ź ��

    [SerializeField]
    private float _durability;
    [SerializeField]
    private int _ammunition;

    public override Item CreateItem()
    {
        return new Equipment(this);
    }
}
