using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Item(string name) // ������ ���� 
    {
        this.name = name;
        Debug.Log(name);
    }
}
