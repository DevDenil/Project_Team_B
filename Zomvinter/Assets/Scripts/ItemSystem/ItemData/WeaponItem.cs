using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Equipment
{
    [SerializeField]
    public WeaponItem(WeaponItemData data) : base(data) { }

    public WeaponItemData ItemData
    {
        get { return ItemData; }
        set { ItemData = value; }
    }
}
