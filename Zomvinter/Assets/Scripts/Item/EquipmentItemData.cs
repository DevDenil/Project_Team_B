using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ ������ </summary>
public class EquipmentItemData : ItemData
{
    public int EquipmentType => _equipmentType;
    [SerializeField] private int _equipmentType = 0;
}
