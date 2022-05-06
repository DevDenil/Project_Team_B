using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKM : MonoBehaviour
{
    [SerializeField]
    private ItemData _itemData;
    public ItemData ItemData
    {
        get { return _itemData; }
        set { _itemData = value; }
    }

    ItemData _WeaponItemInfo;
    // Start is called before the first frame update
    void Start()
    {
        _WeaponItemInfo = ItemData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemData SendItemInfo()
    {
        return _WeaponItemInfo;
    }
}
