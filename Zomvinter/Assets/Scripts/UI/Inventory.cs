using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Transform myPlayerPos;

    public List<Item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    public Slot[] slots;

    /// <summary> Editor �󿡼� Slot �ڽ� ������Ʈ�� �ҷ��ͼ� slots �迭�� ���� </summary>
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        SetIndexOnSlot();
    }

    private void Awake()
    {
        RefreshSlot();
    }
    void SetIndexOnSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SlotIndex = i;
        }
    }

    /// <summary> ������ ��� ���ΰ�ħ </summary>
    public void RefreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i< slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _itme)
    {
        if(items.Count < slots.Length)
        {
            items.Add(_itme);
            RefreshSlot();
        }
        else
        {
            //�κ��丮 �� �� ���
            Debug.Log("������ ���� �� �ֽ��ϴ�.");
        }
    }

}
