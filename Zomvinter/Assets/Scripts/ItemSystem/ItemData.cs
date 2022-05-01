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

    public void WatchItemInfo()
    {
        Debug.Log("아이템 이름 :: " + myProperties._itemName);
    }
}