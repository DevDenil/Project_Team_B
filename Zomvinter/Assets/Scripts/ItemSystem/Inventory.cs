using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /*
        [Item�� ��ӱ���]
        - Item
            - Consumable : IUsableItem.Use() -> ��� �� ���� 1 �Ҹ�

            - EquipmentItem
                - WeaponItem
                - ArmorItem

        [ItemData�� ��ӱ���]
          (ItemData�� �ش� �������� �������� ���� ������ �ʵ� ����)
          (��ü���� �޶����� �ϴ� ���� ������, ��ȭ�� ���� Item Ŭ�������� ����)

        - ItemData
            - ConsumableData : ȿ����(Value : ȸ����, ���ݷ� � ���)

            - EquipmentData : �ִ� ������
                - WeaponData : �⺻ ���ݷ�
                - ArmorData : �⺻ ����
    */

    /*
        [API]
        - bool HasItem(int) : �ش� �ε����� ���Կ� �������� �����ϴ��� ����
        - bool IsCountableItem(int) : �ش� �ε����� �������� �� �� �ִ� ���������� ����
        - int GetCurrentAmount(int) : �ش� �ε����� ������ ����
            - -1 : �߸��� �ε���
            -  0 : �� ����
            -  1 : �� �� ���� �������̰ų� ���� 1
        - ItemData GetItemData(int) : �ش� �ε����� ������ ����
        - string GetItemName(int) : �ش� �ε����� ������ �̸�

        - int Add(ItemData, int) : �ش� Ÿ���� �������� ������ ������ŭ �κ��丮�� �߰�
            - �ڸ� �������� ������ ������ŭ ����(0�̸� ��� �߰� �����ߴٴ� �ǹ�)
        - void Remove(int) : �ش� �ε����� ���Կ� �ִ� ������ ����
        - void Swap(int, int) : �� �ε����� ������ ��ġ ���� �ٲٱ�
        - void SeparateAmount(int a, int b, int amount)
            - a �ε����� �������� �� �� �ִ� �������� ���, amount��ŭ �и��Ͽ� b �ε����� ����
        - void Use(int) : �ش� �ε����� ������ ���
        - void UpdateSlot(int) : �ش� �ε����� ���� ���� �� UI ����
        - void UpdateAllSlot() : ��� ���� ���� �� UI ����
        - void UpdateAccessibleStatesAll() : ��� ���� UI�� ���� ���� ���� ����
        - void TrimAll() : �տ������� ������ ���� ä���
        - void SortAll() : �տ������� ������ ���� ä��鼭 ����
    
    // ��¥ : 2021-03-07 PM 7:33:52
    */

    /***********************************************************************
    *                               Public Properties
    ***********************************************************************/
    #region
    /// <summary> �� �÷��̾��� ��ġ </summary>
    public Transform myPlayerPos;

    /// <summary> ���� ���� �ѵ� </summary>
    [SerializeField]
    public int ItemCapacity { get; private set; }

    /// <summary> ������ ���� �ѵ� </summary>
    [SerializeField]
    public int PrimaryCapacity { get; private set; }

    /// <summary> �������� ���� �ѵ� </summary>
    [SerializeField]
    public int SecondaryCapacity { get; private set; }

    /// <summary> �Ҹ�ǰ ���� �ѵ� </summary>
    [SerializeField]
    public int ConsumableCapacity { get; private set; }
    #endregion
    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region
    /// <summary> ����� InventoryUI ��ũ��Ʈ </summary>
    private InventoryUI _inventoryUI;

    #region ����
    /// <summary> ���� ������ ��� ����Ʈ </summary>
    public List<Item> Items;
    /// <summary> ���� �ʱ� ���� �ѵ� </summary>
    [Range(4, 48)]
    private int _itemInitalCapacity = 4;
    /// <summary> ���� �ִ� ���� �ѵ� </summary>
    [Range(4, 48)]
    private int _itemsMaxCapacity = 48;
    #endregion -------------------------------------------------------------------

    #region �ֹ���
    /// <summary> �� ���� ������ ��� ����Ʈ </summary>
    public List<Item> PrimaryItems;

    /// <summary> ������ �ִ� ���� �ѵ� </summary>
    [Range(2, 2)]
    private int _PrimaryMaxCapacity = 2;
    #endregion -------------------------------------------------------------------

    #region ��������
    /// <summary> ���� ���� ������ ��� ����Ʈ </summary>
    public List<Item> SecondaryItems;

    /// <summary> �������� �ִ� ���� �ѵ� </summary>
    [Range(1, 1)]
    private int _SecondaryMaxCapacity = 1;
    #endregion -------------------------------------------------------------------

    #region �Ҹ�ǰ
    /// <summary> �Ҹ�ǰ ������ ��� ����Ʈ </summary>
    public List<Item> ConsumableItems;

    /// <summary> �Ҹ�ǰ �ִ� ���� �ѵ� </summary>
    [Range(1, 3)]
    private int _ConsumableMaxCapacity = 3;
    #endregion -------------------------------------------------------------------

    #region ���
    /// <summary> ��信 ���� �� ������ </summary>
    public Item HelmetItem = null;

    /// <summary> ���� ���� �� ������ </summary>
    public Item BodyArmorItem = null;

    /// <summary> ���濡 ���� �� ������ </summary>
    public Item BackpackItem = null;
    #endregion -------------------------------------------------------------------

    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region
    /// <summary> ������ �󿡼� ���� �Ǵ� �Լ� </summary>
    private void OnValidate()
    {
        _inventoryUI = GetComponent<InventoryUI>();
        ItemCapacity = SetInitalCapacity(_itemInitalCapacity);
        PrimaryCapacity = SetInitalCapacity(_PrimaryMaxCapacity);
        SecondaryCapacity = SetInitalCapacity(_SecondaryMaxCapacity);
        ConsumableCapacity = SetInitalCapacity(_ConsumableMaxCapacity);
    }

    /// <summary> ���μ����� ���۵Ǳ� ���� ����Ǵ� �Լ� </summary>
    private void Awake()
    {

    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> �ε����� ���� ���� ���� �ִ��� �˻� </summary>
    private bool IsValidIndex(int Index, int Capacity)
    {
        return Index >= 0 && Index < Capacity;
    }
    #endregion

    /// <summary> �տ������� ����ִ� ���� �ε��� Ž�� </summary>
    private int FindEmptySlotIndex(List<Item> list, int Capacity, int StartIndex = 0)
    {
        for (int i = StartIndex; i < Capacity; i++)
        {
            if (list[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
    /***********************************************************************
    *                               Check & Getter Methods
    ***********************************************************************/
    #region
    /// <summary> ���� �ʱ�뷮 ���� �Լ� </summary>
    int SetInitalCapacity(int inital)
    {
        return inital;
    }

    /// <summary> �ش� ������ �������� ���� �ִ��� ���� </summary>
    public bool HasItem(int Index, int Capacity, List<Item> list)
    {
        return IsValidIndex(Index, Capacity) && list[Index] != null;
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    public void UpdateAccessibleStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(ItemCapacity, _inventoryUI.ItemSlots);
        _inventoryUI.SetAccessibleSlotRange(PrimaryCapacity, _inventoryUI.PirmarySlots);
        _inventoryUI.SetAccessibleSlotRange(ConsumableCapacity, _inventoryUI.ConsumableSlots);
    }

    /// <summary> �տ������� ���� ������ �ִ� Countable �������� ���� �ε��� Ž�� </summary>
    //�̱���

    /// <summary> �ش��ϴ� �ε����� ���� ���� �� UI ���� </summary>
    public void UpdateSlot(int Index, int Capacity, List<Item> _item, List<Slot>_slotUIList)
    {
        if (!IsValidIndex(Index, Capacity)) return;

        Item item = _item[Index];

        // 1.�������� ���Կ� �����ϴ� ���
        if(item != null)
        {
            //������ ���
            _inventoryUI.SetItemIcon(_slotUIList, Index, item.Data.ItemImage);

            // 1-1.�������� �� �� �ִ� �������� ���
            if(item is Consumable con)
            {
                // 1-1-1. ������ 0�� ���, ������ ����
                if (con.IsEmpty)
                {
                    _item[Index] = null;
                    RemoveIcon(_slotUIList);
                    return;
                }
                // 1-1-2. ���� ǥ��
                else
                {
                    _inventoryUI.SetItemAmountText(_slotUIList, Index, con.Amount);
                }
            }
            // 1-2. �� �� ���� �������� ���, ���� �ؽ�Ʈ ����
            else
            {
                _inventoryUI.HideItemAmountText(_slotUIList, Index);
            }

            // ���� ���� ���� ������Ʈ
            _inventoryUI.UpdateSlotFilterState(_slotUIList, Index, item.Data);
        }
        else
        {
            RemoveIcon(_slotUIList);
        }

        // ���� �Լ� : ������ �����ϱ�
        void RemoveIcon(List<Slot>_slotsUIList)
        {
            _inventoryUI.RemoveItem(_slotsUIList, Index);
            _inventoryUI.HideItemAmountText(_slotsUIList, Index); // ���� �ؽ�Ʈ �����
        }
    }
    #endregion


    /***********************************************************************
    *                               Old Methods
    ***********************************************************************/
    #region
    /*
    

    /// <summary> ��� Bag�� ������ ����, �ε��� ���ΰ�ħ </summary>
    public void RefreshList()
    {
        RefreshSlot(ItemSlots, Items);
        //RefreshSlot(PrimarySlots, PrimaryItems);
        //RefreshSlot(ConsumableSlots, ConsumableItems);
        //RefreshSlot(SecondarySlots, SecondaryItems);
        //RefreshSlot(HelmetSlot, HelmetItem);
        //RefreshSlot(BodyArmorSlot, BodyArmorItem);
        //RefreshSlot(BackpackSlot, BackpackItem);
    }

    /// <summary> ������ ���, �ε��� ���ΰ�ħ </summary>
    public void RefreshSlot(Slot[] Bag, List<Item> items)
    {
        for (int i = 0; i < Bag.Length; i++)
        {
            if (items[i] != null)
            {
                Bag[i].SlotIndex = i;
                Bag[i].GetComponentInChildren<SlotItem>().ItemProperty = items[i];
                Debug.Log(items[i].Data);
            }
            else
            {
                Bag[i].SlotIndex = i;
                Bag[i].GetComponentInChildren<SlotItem>().ItemProperty = null;
            }
        }
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

    /// <summary> ������ �߰� �Լ� </summary>
    /// <param name="_item">�߰� �� ������ ����</param>
    public void AddItem(Item _item)
    {
        //������ Ÿ�� �˻�
        switch (ItemClassifier(_item))
        {
            case 1:
                if (SlotFillCheck(PrimaryItems))
                {

                    PrimaryItems[GetIndex(PrimaryItems)] = _item;
                }
                else
                {
                    Items[GetIndex(Items)] = _item;
                }
                break;
            case 2:
                if (SlotFillCheck(SecondaryItems))
                {
                    SecondaryItems[GetIndex(SecondaryItems)] = _item;
                }
                else
                {
                    Items[GetIndex(Items)] = _item;
                }
                break;
            case 3:
                if (SlotFillCheck(ConsumableItems))
                {
                    ConsumableItems[GetIndex(ConsumableItems)] = _item;
                }
                else
                {
                    Items[GetIndex(Items)] = _item;
                }
                break;
            case 4:
                if (SlotFillCheck(HelmetItem))
                {
                    HelmetItem = _item;
                }
                else
                {
                    Items.Add(_item);
                }
                break;
            case 5:
                if (SlotFillCheck(BodyArmorItem))
                {
                    BodyArmorItem = _item;
                }
                else
                {
                    Items.Add(_item);
                }
                break;
            case 6:
                if (SlotFillCheck(BackpackItem))
                {
                    BackpackItem = _item;
                }
                else
                {
                    Items.Add(_item);
                }
                break;
            case 7:
                if (SlotFillCheck(Items))
                {
                    Items[GetIndex(Items)] = _item;
                }
                else
                {
                    Vector3 DropPos = myPlayerPos.position;
                    DropPos.y += 2.0f;
                    GameObject obj = Instantiate(_item.Data.ItemPrefab,
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
    public int ItemClassifier(Item _item)
    {
        if (_item.Data.ItemType == ItemType.Primary)
        {
            return 1;
        }
        else if (_item.Data.ItemType == ItemType.Secondary)
        {
            return 2;
        }
        else if (_item.Data.ItemType == ItemType.Expand)
        {
            return 3;
        }
        else if (_item.Data.ItemType == ItemType.Helmet)
        {
            return 4;
        }
        else if (_item.Data.ItemType == ItemType.Bodyarmor)
        {
            return 5;
        }
        else if (_item.Data.ItemType == ItemType.Backpack)
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
    
    */
    #endregion
}