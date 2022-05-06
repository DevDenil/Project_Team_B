using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    /// <summary> ������ �����͸� �ҷ��´� </summary>
    public ItemData Data { get; private set; }
    /// <summary> �ҷ��� �����͸� Data�� ���� </summary>
    /// <param name="data"></param>
    public Item(ItemData data) => Data = data;
}