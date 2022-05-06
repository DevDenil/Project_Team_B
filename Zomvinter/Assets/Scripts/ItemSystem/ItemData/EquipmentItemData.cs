using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItemData : ItemData
{

    public float MaxDurability => _maxDurability; // ������
    public int Ammunition => _ammunition; // ��ź ��

    [SerializeField]
    private float _maxDurability;
    [SerializeField]
    private int _ammunition;
}
