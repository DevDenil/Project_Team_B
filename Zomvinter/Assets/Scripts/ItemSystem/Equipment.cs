using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public Equipment(EquipmentData data, int amount = 1) : base(data)
    {
        Data = data;;
    }

    /// <summary> ConsumableData로부터 가져온 정보를 Data에 저장 </summary>
    public EquipmentData Data { get; private set; }
}
