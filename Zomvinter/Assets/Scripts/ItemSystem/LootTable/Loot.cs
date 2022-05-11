using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Loot : MonoBehaviour
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private float dropChance;//랜덤 몇번 돌릴 것 인지 

    public Item MyItem
    {
        get
        {
            return item;
        }
    }
    public float MyDropChance
    {
        get
        {
            return dropChance;
        }
    }
}
