using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment", order = 2)]
public class Equipment : Item
{

    [SerializeField]
    public float _durability; // 내구도
    [SerializeField]
    public int _ammunition; // 장탄 수
}
