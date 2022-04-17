using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data { get; private set; }

    public Item(ItemData data) => Data = data;

    public Item(string name) // 아이템 형식 
    {
        this.name = name;
        Debug.Log(name);
    }

}
