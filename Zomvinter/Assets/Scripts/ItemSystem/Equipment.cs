using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public Equipment(EquipmentData data, int amount = 1) : base(data)
    {
        Data = data;;
    }

    /// <summary> ConsumableData�κ��� ������ ������ Data�� ���� </summary>
    public EquipmentData Data { get; private set; }
}
