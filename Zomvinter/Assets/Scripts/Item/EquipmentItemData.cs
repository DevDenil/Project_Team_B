using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 장착 아이템 데이터 </summary>
public class EquipmentItemData : ItemData
{
    public int EquipmentType => _equipmentType;
    [SerializeField] private int _equipmentType = 0;
}
