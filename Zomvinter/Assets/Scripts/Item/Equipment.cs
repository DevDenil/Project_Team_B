using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment", order = 2)]
public class Equipment : Item
{

    [SerializeField]
    public float _durability; // ������
    [SerializeField]
    public int _ammunition; // ��ź ��
}
