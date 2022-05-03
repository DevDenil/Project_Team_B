using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary> �� �÷��̾��� ��ġ </summary>
    public Transform myPlayerPos;
    [SerializeField]
    //public int _isStockable = 28;
    //public int _hasItems = 0;

    /// <summary> �κ��丮�� ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> items;
    public int _itemsCapacity = 28;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform ItemBag;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] ItemSlots;

    /// <summary> �� ���⿡ ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> PrimaryItems;
    private int _PrimaryCapacity = 2;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform PrimaryBag;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] PrimarySlots;

    /// <summary> ���� ���⿡ ���� �� ������ </summary>
    public List<Item> SecondaryItems;
    private int _SecondaryCapacity = 1;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform SecondaryBag;
    /// <summary> ���� ���Ⱑ ���� �� Slot </summary>
    public Slot[] SecondarySlots;

    /// <summary> �Ҹ�ǰ�� ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> ConsumableItems;
    private int _ConsumableCapacity = 2;
    /// <summary> �κ��丮 ���� ���Ե��� ��Ƴ��� Contents�� ��ġ �� </summary>
    [SerializeField]
    private Transform ConsumableBag;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    public Slot[] ConsumableSlots;

    /// <summary> ��信 ���� �� ������ </summary>
    public Item HelmetItem = null;
    /// <summary> Helmet�� ���� �� Slot </summary>
    public Slot HelmetSlot;

    /// <summary> ���� ���� �� ������ </summary>
    public Item BodyArmorItem = null;
    /// <summary> BodyArmor�� ���� �� Slot </summary>
    public Slot BodyArmorSlot;

    /// <summary> ���濡 ���� �� ������ </summary>
    public Item BackpackItem = null;
    /// <summary> Backpack�� ���� �� Slot </summary>
    public Slot BackpackSlot;

    /* ---------------------------------------------------------------------------------------------------------------------------------------------------------- */

    /// <summary> ������ �󿡼� ���� �Ǵ� �Լ� </summary>
    private void OnValidate()
    {
        // �� Bag�� ����ִ� Slot�� ������ �ҷ��´�.
        ItemSlots = ItemBag.GetComponentsInChildren<Slot>();
        PrimarySlots = PrimaryBag.GetComponentsInChildren<Slot>();
        SecondarySlots = SecondaryBag.GetComponentsInChildren<Slot>();
        ConsumableSlots = ConsumableBag.GetComponentsInChildren<Slot>();

        RefreshList();
    }

    /// <summary> ���μ����� ���۵Ǳ� ���� ����Ǵ� �Լ� </summary>
    private void Awake()
    {
        //����Ʈ �ȿ� �� ������Ʈ�� ����־ �ִ� ũ�� ����
        GenerateInventory();
        //����Ʈ �ε���, ������ ���� ���ΰ�ħ
        RefreshList();
    }

    /* ---------------------------------------------------------------------------------------------------------------------------------------------------------- */

    /// <summary> ��� Bag�� ������ ����, �ε��� ���ΰ�ħ </summary>
    public void RefreshList()
    {
        RefreshSlot(ItemSlots, items);
        RefreshSlot(PrimarySlots, PrimaryItems);
        RefreshSlot(ConsumableSlots, ConsumableItems);
        RefreshSlot(SecondarySlots, SecondaryItems);
        RefreshSlot(HelmetSlot, HelmetItem);
        RefreshSlot(BodyArmorSlot, BodyArmorItem);
        RefreshSlot(BackpackSlot, BackpackItem);
    }

    /// <summary> ������, �ε��� ���ΰ�ħ overwrite </summary>
    public void RefreshSlot(Slot Bag, Item items)
    {
        if (items != null)
        {
            Bag.GetComponentInChildren<SlotItem>().ItemProperty = items;
        }
        else
        {
            Bag.GetComponentInChildren<SlotItem>().ItemProperty = null;
        }
    }

    /// <summary> ������ ���, �ε��� ���ΰ�ħ </summary>
    public void RefreshSlot(Slot[] Bag, List<Item> items)
    {
        int i = 0;
        for (; i < items.Count && i < Bag.Length; i++)
        {
            Bag[i].SlotIndex = i;
            Bag[i].GetComponentInChildren<SlotItem>().ItemProperty = items[i];
        }
        for (; i < Bag.Length; i++)
        {
            Bag[i].SlotIndex = i;
            Bag[i].GetComponentInChildren<SlotItem>().ItemProperty = null;
        }
    }

    /// <summary> ��� Bag�� �� ���� ���� </summary>
    public void GenerateInventory()
    {
        SlotGenrate(PrimaryItems, _PrimaryCapacity);
        SlotGenrate(ConsumableItems, _ConsumableCapacity);
        SlotGenrate(items, _itemsCapacity);
    }

    /// <summary> ����Ʈ �ȿ� �� ������ ����־ �ִ� ũ�� ���� </summary>
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

    /* ---------------------------------------------------------------------------------------------------------------------------------------------------------- */

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

    /* ---------------------------------------------------------------------------------------------------------------------------------------------------------- */

    /// <summary> ������ �߰� �Լ� </summary>
    /// <param name="_item">�߰� �� ������ ����</param>
    public void AddItem(ItemData _item)
    {
        //������ Ÿ�� �˻�
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
        RefreshList();

    }

    /// <summary> ������ Ÿ�� üũ �Լ� </summary>
    public int ItemClassifier(ItemData _item)
    {
        if (_item.myProperties._type == ItemType.Primary)
        {
            return 1;
        }
        else if (_item.myProperties._type == ItemType.Secondary)
        {
            return 2;
        }
        else if (_item.myProperties._type == ItemType.Expand)
        {
            return 3;
        }
        else if (_item.myProperties._type == ItemType.Helmet)
        {
            return 4;
        }
        else if (_item.myProperties._type == ItemType.Bodyarmor)
        {
            return 5;
        }
        else if (_item.myProperties._type == ItemType.Backpack)
        {
            return 6;
        }
        else
        {
            return 7;
        }
    }

    /// <summary> ���Կ� �������� �ִ��� üũ�ϴ� �Լ� </summary>
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

    /// <summary> ���Կ� �������� �ִ��� üũ�ϴ� �Լ� overwrite </summary>
    public bool SlotFillCheck(Item Slot)
    {
        bool isStockable = false;
        if (Slot == null)
        {
            isStockable = true;
        }
        return isStockable;
    }

    /// <summary> �������� ���� ������ �ε����� ��ȯ�ϴ� �Լ� </summary>
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
