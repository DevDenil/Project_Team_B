using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Consumable_", menuName = "Inventory System/Item Data/Consumaber", order = 1)]
public class Consumable : Item
{
    [SerializeField]
    public int _Amount; // �ִ� ����
    [SerializeField]
    public float _value; // ȿ����
}
