using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem
{
    /// <summary> ConsumableData�κ��� ������ ������ Data�� ���� </summary>
    public WeaponItemData WeaponData;


    public WeaponItem(WeaponItemData data) : base(data) 
    {
        WeaponData = data;
    }
}