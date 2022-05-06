using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Item
{
    /// <summary> ConsumableData로부터 가져온 정보를 Data에 저장 </summary>
    public EquipmentData Data { get; private set; }

    protected Equipment(EquipmentData data) : base(data)
    {
        Data = data;
    }

    public Equipment(EquipmentData data, int amount = 1) : base(data)
    {
        Data = data;;
        Durability = data.MaxDurability;
    }

    /// <summary> 현재 내구도 </summary>
    public float Durability
    {
        get => _durability;
        set
        {
            if (value < 0) value = 0;
            if (value > Data.MaxDurability)
                value = Data.MaxDurability;

            _durability = value;
        }
    }
    private float _durability;
}
