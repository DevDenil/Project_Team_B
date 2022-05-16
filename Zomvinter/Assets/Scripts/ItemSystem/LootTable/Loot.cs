using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Loot : MonoBehaviour
{
    [SerializeField]
    private LootItem item;

    [SerializeField]
    private float dropChance;//���� ��� ���� �� ���� 

    public LootItem MyItem
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
