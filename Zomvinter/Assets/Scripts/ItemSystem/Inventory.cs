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

    private void Start()
    {
        UpdateAccessibleStatesAll();
    }
    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> �ε����� ���� ���� ���� �ִ��� �˻� </summary>
    private bool IsValidIndex(int Index, int Capacity)
    {
        return Index >= 0 && Index < Capacity;
    }

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

    /// <summary> �κ��丮�� �� á���� Ž�� </summary>
    private bool isFull(List<Item> list, int Capacity)
    {
        if (list.Count <= Capacity) return true;
        else return false;
    }

    /// <summary> �տ������� ���� ������ �ִ� Countable �������� ���� �ε��� Ž�� </summary>
    private int FindCountableItemSlotIndex(CountableItemData target, int Capacity, List<Item>list, int startIndex = 0)
    {
        for(int i = startIndex; i< Capacity; i++)
        {
            var current = list[i];
            if (current == null) continue;

            // ������ ���� ��ġ, ���� ���� Ȯ��
            if(current.Data == target && current is CountableItem ci)
            {
                if (!ci.IsMax) return i;
            }
        }

        return -1;
    }

    /// <summary> �ش��ϴ� �ε����� ���� ���� �� UI ���� </summary>
    public void UpdateSlot(int Index, int Capacity, List<Item> _item, List<Slot> _slotUIList)
    {
        if (!IsValidIndex(Index, Capacity)) return;

        Item item = _item[Index];

        // 1.�������� ���Կ� �����ϴ� ���
        if (item != null)
        {
            //������ ���
            _inventoryUI.SetItemIcon(_slotUIList, Index, item.Data.ItemImage);

            // 1-1.�������� �� �� �ִ� �������� ���
            if (item is ConsumableItem con)
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
        void RemoveIcon(List<Slot> _slotsUIList)
        {
            _inventoryUI.RemoveItem(_slotsUIList, Index);
            _inventoryUI.HideItemAmountText(_slotsUIList, Index); // ���� �ؽ�Ʈ �����
        }
    }

    /// <summary> �ش��ϴ� �ε����� ���Ե��� ���� �� UI ���� </summary>
    private void UpdateSlot(int Capacity, List<Item>list, List<Slot>_slotUIList, params int[] indices)
    {
        foreach (var i in indices)
        {
            UpdateSlot(i, Capacity, list, _slotUIList);
        }
    }

    /// <summary> ��� ���Ե��� ���¸� UI�� ���� </summary>
    private void UpdateAllSlot(int Capacity, List<Item>list, List<Slot> _slotUIList)
    {
        for(int i = 0; i < Capacity; i++)
        {
            UpdateSlot(i, Capacity, list, _slotUIList);
        }
    }

    #endregion
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


    /// <summary> �ش� ������ �� �� �ִ� ���������� ���� </summary>
    public bool IsConsumableItem(int Index, int Capacity, List<Item> list)
    {
        return HasItem(Index, Capacity, list) && list[Index] is ConsumableItem;
    }

    /// <summary> 
    /// �ش� ������ ���� ������ ���� ����
    /// <para/> - �߸��� �ε��� : -1 ����
    /// <para/> - �� ���� : 0 ����
    /// <para/> - �� �� ���� ������ : 1 ����
    /// </summary>
    public int GetCurrentAmount(int Index, int Capacity, List<Item> list)
    {
        if (!IsValidIndex(Index, Capacity)) return -1;
        if (list[Index] == null) return 0;

        ConsumableItem con = list[Index] as ConsumableItem;
        if (con == null) return 1;

        return con.Amount;
    }


    /// <summary> �ش� ������ ������ ���� ���� </summary>
    public ItemData GetItemData (int Index, int Capacity, List<Item> list)
    {
        if (!IsValidIndex(Index, Capacity)) return null;
        if (list[Index] == null) return null;

        return list[Index].Data;
    }

    /// <summary> �ش� ������ ������ �̸� ���� </summary>
    public string GetItemName(int Index, int Capacity, List<Item> list)
    {
        if (!IsValidIndex(Index, Capacity)) return "";
        if (list[Index] == null) return "";

        return list[Index].Data.ItemName;
    }

    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�

    public void ConnectUI(InventoryUI inventoryUI)
    {
        _inventoryUI = inventoryUI;
        _inventoryUI.SetInventoryReference(this);
    }

    /// <summary> �κ��丮�� ������ �߰�
    /// <para/> �ִ� �� ������ �׿� ������ ���� ����
    /// <para/> ������ 0�̸� �ִµ� ��� �����ߴٴ� �ǹ�
    /// </summary>
    public int Add(ItemData itemdata, int Capacity, List<Item> ItemList, List<Slot>slotList, int amount = 1)
    {
        int index;
        // 1. ������ �ִ� ������
        if(itemdata is CountableItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;

            while(amount > 0)
            {
                // 1-1. �̹� �ش� �������� �κ��丮 ���� �����ϰ�, ���� ���� �ִ��� �˻�
                if(findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, Capacity, ItemList, index + 1);

                    // ���� ���� �ִ� ������ ������ ���̻� ���ٰ� �ǴܵǴ� ���, �� ���Ժ��� Ž�� ����
                    if(index == -1)
                    {
                        findNextCountable = false;
                    }
                    // ������ ������ ã�� ���, �� ������Ű�� �ʰ��� ���� �� amount�� �ʱ�ȭ
                    else
                    {
                        CountableItem ci = ItemList[index] as CountableItem;
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index, Capacity, ItemList, slotList);
                    }
                }
                // 1-2. �� ���� Ž��
                else
                {
                    index = FindEmptySlotIndex(ItemList, Capacity, index + 1);

                    // �� �������� ���� ��� ����
                    if(index == -1)
                    {
                        break;
                    }
                    // �� ���� �߰� ��, ���Կ� ������ �߰� �� �׿��� ���
                    else
                    {
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        // ���Կ� �߰�
                        ItemList[index] = ci;

                        // ���� ���� ���
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index, Capacity, ItemList, slotList);
                    }
                }
            }
        }
        // 2. ������ ���� ������
        else
        {
            // 2-1. 1���� �ִ� ���, ������ ����
            if(amount == 1)
            {
                index = FindEmptySlotIndex(ItemList, Capacity);

                if(index != -1)
                {
                    // �������� �����Ͽ� ���Կ� �߰�
                    ItemList[index] = itemdata.CreateItem();
                    amount = 0;

                    UpdateSlot(index, Capacity, ItemList, slotList);
                }
            }

            // 2-2. 2�� �̻��� ������ ���� �������� ���ÿ� �߰��ϴ� ���
            index = -1;
            for(; amount > 0; amount--)
            {
                // ������ ���� �ε����� ���� �ε������� ���� Ž��
                index = FindEmptySlotIndex(ItemList, Capacity, index + 1);

                // �� ���� ���� ��� ���� ����
                if (index == -1)
                {
                    break;
                }

                // �������� �����Ͽ� ���Կ� �߰�
                ItemList[index] = itemdata.CreateItem();

                UpdateSlot(index, Capacity, ItemList, slotList);
            }
        }
        return amount;
    }

    /// <summary> ��� ���� UI�� ���� ���� ���� ������Ʈ </summary>
    public void UpdateAccessibleStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(ItemCapacity, _inventoryUI.ItemSlots);
        _inventoryUI.SetAccessibleSlotRange(PrimaryCapacity, _inventoryUI.PrimarySlots);
        _inventoryUI.SetAccessibleSlotRange(ConsumableCapacity, _inventoryUI.ConsumableSlots);
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