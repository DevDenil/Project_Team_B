using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary> �� �÷��̾��� ��ġ </summary>
    public Transform myPlayerPos;
    /// <summary> �κ��丮�� ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> items;
    public int _itemsCapacity = 28;
    private int _isStockable = 28;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform slotParent;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] slots;

    /// <summary> �� ���⿡ ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> PrimaryItems;
    private int _PrimaryCapacity = 2;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform PrimaryParent;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] PrimarySlots;

    /// <summary> ���� ���⿡ ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> SecondaryItems;
    private int _SecondaryCapacity = 1;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform SecondaryParent;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] SecondarySlots;

    /// <summary> �Ҹ�ǰ�� ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> ConsumableItems;
    private int _ConsumableCapacity = 2;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform ConsumableParent;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] ConsumableSlots;

    /// <summary> ��� ������ ���� </summary>
    public Item HelmetItem = null;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] HelmetSlot;

    /// <summary> �� ������ ���� </summary>
    public Item BodyArmorItem = null;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] BodyArmorSlot;

    /// <summary> ���� ������ ���� </summary>
    public Item BackpackItem = null;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] BackpackSlot;


    /// <summary> ������ �󿡼� ���� �Ǵ� �Լ� </summary>
    private void OnValidate()
    {        
        slots = slotParent.GetComponentsInChildren<Slot>(); // Slot�� �ڽ� ������Ʈ�� �ҷ��ͼ� slots �迭�� ����
        PrimarySlots = PrimaryParent.GetComponentsInChildren<Slot>();
        SecondarySlots = SecondaryParent.GetComponentsInChildren<Slot>();
        ConsumableSlots = ConsumableParent.GetComponentsInChildren<Slot>();

        Refresh();
    }

    /// <summary> ���μ����� ���۵Ǳ� ���� ����Ǵ� �Լ� </summary>
    private void Awake()
    {
        GenerateInventory();
        Refresh();
    }
    public void Refresh()
    {
        RefreshSlotIndex(slots);
        RefreshSlot(slots, items);
        RefreshSlotIndex(PrimarySlots);
        RefreshSlot(PrimarySlots, PrimaryItems);
        RefreshSlotIndex(SecondarySlots);
        RefreshSlot(SecondarySlots, SecondaryItems);
        RefreshSlotIndex(ConsumableSlots);
        RefreshSlot(ConsumableSlots, ConsumableItems);
    }

    /// <summary> ���� �ε��� ���ΰ�ħ </summary>
    void RefreshSlotIndex(Slot[] Bag)
    {
        for (int i = 0; i < Bag.Length; i++)
        {
            Bag[i].SlotIndex = i;
        }
    }

    /// <summary> ������ ��� ���ΰ�ħ </summary>
    public void RefreshSlot(Slot[] Bag, List<Item> items)
    {
        int i = 0;
        _isStockable = 0;
        for (; i < items.Count && i < Bag.Length; i++)
        {
            Bag[i].GetComponentInChildren<SlotItem>().ItemProperty = items[i];

            _isStockable--;
        }
        for (; i< Bag.Length; i++)
        {
            Bag[i].GetComponentInChildren<SlotItem>().ItemProperty = null;

            _isStockable++;
        }
    }
    public void GenerateInventory()
    {
        SlotGenrate(PrimaryItems, _PrimaryCapacity);
        SlotGenrate(SecondaryItems, _SecondaryCapacity);
        SlotGenrate(ConsumableItems, _ConsumableCapacity);
        SlotGenrate(items, _itemsCapacity);
    }
    public void SlotGenrate(List<Item> Inventory, int Capacity)
    {
        int i = 0;

        for (; i < Inventory.Count && i < Capacity; i++)
        {

        }
        for (; i < Capacity; i++)
        {
            Inventory.Add(default);
        }
    }

    //public void ItemTypeChecker(Item _item)
    //{
    //    if (_item._type == Item.ItemType.Main)
    //    {
    //        if (MainItems[0] == null)
    //        {
    //            MainItems[0] = _item;
    //        }
    //        else
    //        {
    //            if(MainItems[1] == null)
    //            {
    //                MainItems[1] = _item;
    //            }
    //            else
    //            {
    //                items.Add(_item); 
    //            }
    //        }
    //    }
    //    else if (_item._type == Item.ItemType.Second)
    //    {
    //        if (SecondaryItems[0] == null)
    //        {
    //            SecondaryItems[0] = _item;
    //        }
    //        else
    //        {
    //            if (SecondaryItems[1] == null)
    //            {
    //                SecondaryItems[1] = _item;
    //            }
    //            else
    //            {
    //                items.Add(_item);
    //            }
    //        }
    //    }
    //    else if (_item._type == Item.ItemType.Consumable)
    //    {
    //        if (ConsumableItems[0] == null)
    //        {
    //            ConsumableItems[0] = _item;
    //        }
    //        else
    //        {
    //            if (ConsumableItems[1] == null)
    //            {
    //                ConsumableItems[1] = _item;
    //            }
    //            else
    //            {
    //                items.Add(_item);
    //            }
    //        }
    //    }
    //    else if (_item._type == Item.ItemType.Helmet)
    //    {
    //        HelmetItem = _item;
    //    }
    //    else if (_item._type == Item.ItemType.BodyArmor)
    //    {
    //        BodyArmorItem = _item;
    //    }
    //    else if (_item._type == Item.ItemType.Backpack)
    //    {
    //        BackpackItem = _item;
    //    }
    //    else
    //    {
    //        items.Add(_item);
    //    }
    //}

    /// <summary> ������ �߰� �Լ� </summary>
    /// <param name="_item">�߰� �� ������ ����</param>
    public void AddItem(ItemData _item)
    {
        if(_isStockable < _itemsCapacity)
        {
            switch (ItemClassifier(_item))
            {
                case 1:
                    if (SlotFillCheck(PrimaryItems))
                    {
                        
                        PrimaryItems[GetIndex(PrimaryItems)] = _item.myProperties;
                    }
                    else
                    {
                        items[GetIndex(items)] = _item.myProperties;
                    }
                    break;
                case 2:
                    Debug.Log("CHeck");
                    if (SlotFillCheck(SecondaryItems))
                    {
                        SecondaryItems[GetIndex(SecondaryItems)] = _item.myProperties;
                    }
                    else
                    {
                        items[GetIndex(items)] = _item.myProperties;
                    }
                    break;
                case 3:
                    if (SlotFillCheck(ConsumableItems))
                    {
                        ConsumableItems[GetIndex(ConsumableItems)] = _item.myProperties;
                    }
                    else
                    {
                        items[GetIndex(items)] = _item.myProperties;
                    }
                    break;
                case 4:
                    if (SlotFillCheck(HelmetItem))
                    {
                        HelmetItem = _item.myProperties;
                    }
                    else
                    {
                        items.Add(_item.myProperties);
                    }
                    break;
                case 5:
                    if (SlotFillCheck(BodyArmorItem))
                    {
                        BodyArmorItem = _item.myProperties;
                    }
                    else
                    {
                        items.Add(_item.myProperties);
                    }
                    break;
                case 6:
                    if (SlotFillCheck(BackpackItem))
                    {
                        BackpackItem = _item.myProperties;
                    }
                    else
                    {
                        items.Add(_item.myProperties);
                    }
                    break;
                case 7:
                    if (SlotFillCheck(items))
                    {
                        items[GetIndex(items)] = _item.myProperties;
                    }
                    else
                    {
                        Vector3 DropPos = myPlayerPos.position;
                        DropPos.y += 2.0f;
                        GameObject obj = Instantiate(_item.myProperties._itemPrefab,
                                DropPos, Quaternion.identity);
                        obj.GetComponent<Rigidbody>().AddForce(transform.up * 10.0f);
                        Debug.Log("������ ���� �� �ֽ��ϴ�.");
                    }
                    break;
                default:
                    break;
            }
            Refresh();
        }
        else
        {
            //�κ��丮 �� �� ���
        }
    }

    public int ItemClassifier(ItemData _item)
    {
        if (_item.myProperties._type == Item.ItemType.Main)
        {
            return 1;
        }
        else if (_item.myProperties._type == Item.ItemType.Second)
        {
            return 2;
        }
        else if (_item.myProperties._type == Item.ItemType.Consumable)
        {
            return 3;
        }
        else if (_item.myProperties._type == Item.ItemType.Helmet)
        {
            return 4;
        }
        else if (_item.myProperties._type == Item.ItemType.BodyArmor)
        {
            return 5;
        }
        else if (_item.myProperties._type == Item.ItemType.Backpack)
        {
            return 6;
        }
        else
        {
            return 7;
        }
    }

    public bool SlotFillCheck (List<Item> Slot)
    {
        bool isStockable = false;
        for(int i = 0; i < Slot.Count; i++)
        {
            if(Slot[i] == null)
            {
                isStockable = true;
                break;
            }
        }
        if(isStockable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SlotFillCheck(Item Slot)
    {
        bool isStockable = false;
        if (Slot == null)
        {
            isStockable = true;
        }
        return isStockable;
    }

    public int GetIndex (List<Item> Slot)
    {
        int Index = 0;
        for (int i = 0; i < Slot.Count; i++)
        {
            if (Slot[i] == null)
            {
                Index = i;
                break;
            }
        }
        return Index;
    }
}
