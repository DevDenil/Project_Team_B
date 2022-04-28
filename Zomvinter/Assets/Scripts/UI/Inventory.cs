using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary> �� �÷��̾��� ��ġ </summary>
    public Transform myPlayerPos;
    /// <summary> �κ��丮�� ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> items;
    /// <summary> �� ���⿡ ���� �� ������ ��� ����Ʈ </summary>
    public Item[] MainItems = new Item[2];
    /// <summary> ���� ���⿡ ���� �� ������ ��� ����Ʈ </summary>
    public Item[] SecondaryItems = new Item[2];
    /// <summary> �Ҹ�ǰ�� ���� �� ������ ��� ����Ʈ </summary>
    public Item[] ConsumableItems = new Item[3];
    /// <summary> ��� ������ ���� </summary>
    public Item HelmetItem = null;
    /// <summary> �� ������ ���� </summary>
    public Item BodyArmorItem = null;
    /// <summary> ���� ������ ���� </summary>
    public Item BackpackItem = null;

    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform slotParent;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    [SerializeField]
    public Slot[] slots;

    /// <summary> ������ �󿡼� ���� �Ǵ� �Լ� </summary>
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>(); // Slot�� �ڽ� ������Ʈ�� �ҷ��ͼ� slots �迭�� ����
        SetIndexOnSlot();
    }

    /// <summary> ���μ����� ���۵Ǳ� ���� ����Ǵ� �Լ� </summary>
    private void Awake()
    {
        RefreshSlot();
    }

    /// <summary> ���Կ� �ε��� ��ȣ�� �ο��ϴ� �Լ� </summary>
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
            slots[i].GetComponentInChildren<SlotItem>().item = items[i];
        }
        for (; i< slots.Length; i++)
        {
            slots[i].GetComponentInChildren<SlotItem>().item = null;
        }
    }
    public void ItemTypeChecker(Item _item)
    {
        if (_item._type == Item.ItemType.Main)
        {
            if (MainItems[0] == null)
            {
                MainItems[0] = _item;
            }
            else
            {
                if(MainItems[1] == null)
                {
                    MainItems[1] = _item;
                }
                else
                {
                    items.Add(_item); 
                }
            }
        }
        else if (_item._type == Item.ItemType.Second)
        {
            if (SecondaryItems[0] == null)
            {
                SecondaryItems[0] = _item;
            }
            else
            {
                if (SecondaryItems[1] == null)
                {
                    SecondaryItems[1] = _item;
                }
                else
                {
                    items.Add(_item);
                }
            }
        }
        else if (_item._type == Item.ItemType.Consumable)
        {
            if (ConsumableItems[0] == null)
            {
                ConsumableItems[0] = _item;
            }
            else
            {
                if (ConsumableItems[1] == null)
                {
                    ConsumableItems[1] = _item;
                }
                else
                {
                    items.Add(_item);
                }
            }
        }
        else if (_item._type == Item.ItemType.Helmet)
        {
            HelmetItem = _item;
        }
        else if (_item._type == Item.ItemType.BodyArmor)
        {
            BodyArmorItem = _item;
        }
        else if (_item._type == Item.ItemType.Backpack)
        {
            BackpackItem = _item;
        }
        else
        {
            items.Add(_item);
        }
    }
    /// <summary> ������ �߰� �Լ� </summary>
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
