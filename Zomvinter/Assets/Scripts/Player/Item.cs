using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Item(string name) // 아이템 형식 
    {
        this.name = name;
        Debug.Log(name);
    }
}
