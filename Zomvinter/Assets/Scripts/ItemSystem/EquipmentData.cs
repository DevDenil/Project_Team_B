using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment", order = 2)]
public class EquipmentData : ItemData
{

    public float Durability => _durability; // 내구도
    public int Ammunition => _ammunition; // 장탄 수

    [SerializeField]
    private float _durability;
    [SerializeField]
    private int _ammunition;

    public override Item CreateItem()
    {
        return new Equipment(this);
    }
}
