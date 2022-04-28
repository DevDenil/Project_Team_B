using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{

    [SerializeField]
    private Item _myProperties;
    public Item myProperties
    {
        get
        {
            return _myProperties;
        }
        set
        {
            _myProperties = value;
        }
    }

    [SerializeField]
    private Equipment _myEquipmentData;
    public Equipment myEquipmentData
    {
        get
        {
            return _myEquipmentData;
        }
        set
        {
            _myEquipmentData = value;
        }
    }

    [SerializeField]
    private Consumable _myConsumableData;
    public Consumable myConsumableData
    {
        get
        {
            return _myConsumableData;
        }
        set
        {
            _myConsumableData = value;
        }
    }

    public void WatchItemInfo()
    {
        Debug.Log("아이템 이름 :: " + myProperties._itemName);
    }
}