using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItemData : ItemData
{

    public float MaxDurability => _maxDurability; // 내구도
    public int Ammunition => _ammunition; // 장탄 수

    [SerializeField]
    private float _maxDurability;
    [SerializeField]
    private int _ammunition;
}
