using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Consumable_", menuName = "Inventory System/Item Data/Consumaber", order = 1)]
public class ConsumableData : ItemData
{
    public int MaxAmount => _maxAmount;

    public float Value => _value;

    [SerializeField]
    private int _maxAmount = 99; // �ִ� ����
    [SerializeField]
    private float _value; // ȿ����

    public override Item CreateItem()
    {
        return new Consumable(this);
    }
}
