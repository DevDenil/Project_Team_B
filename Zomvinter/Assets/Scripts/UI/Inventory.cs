using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary> 내 플레이어의 위치 </summary>
    public Transform myPlayerPos;
    [SerializeField]
    //public int _isStockable = 28;
    //public int _hasItems = 0;

    /// <summary> 인벤토리에 저장 될 아이템 목록 리스트 </summary>
    public List<Item> items;
    public int _itemsCapacity = 28;
    /// <summary> 인벤토리 내의 슬롯들을 모아놓는 Contents의 위치 값 </summary>
    [SerializeField]
    private Transform ItemBag;
    /// <summary> 인벤토리 내의 슬롯들을 저장 할 배열 </summary>
    public Slot[] ItemSlots;

    /// <summary> 주 무기에 저장 될 아이템 목록 리스트 </summary>
    public List<Item> PrimaryItems;
    private int _PrimaryCapacity = 2;
    /// <summary> 인벤토리 내의 슬롯들을 모아놓는 Contents의 위치 값 </summary>
    [SerializeField]
    private Transform PrimaryBag;
    /// <summary> 인벤토리 내의 슬롯들을 저장 할 배열 </summary>
    public Slot[] PrimarySlots;

    /// <summary> 보조 무기에 저장 될 아이템 </summary>
    public List<Item> SecondaryItems;
    private int _SecondaryCapacity = 1;
    /// <summary> 인벤토리 내의 슬롯들을 모아놓는 Contents의 위치 값 </summary>
    [SerializeField]
    private Transform SecondaryBag;
    /// <summary> 보조 무기가 저장 될 Slot </summary>
    public Slot[] SecondarySlots;

    /// <summary> 소모품에 저장 될 아이템 목록 리스트 </summary>
    public List<Item> ConsumableItems;
    private int _ConsumableCapacity = 2;
    /// <summary> 인벤토리 내의 슬롯들을 모아놓는 Contents의 위치 값 </summary>
    [SerializeField]
    private Transform ConsumableBag;
    /// <summary> 인벤토리 내의 슬롯들을 저장 할 배열 </summary>
    public Slot[] ConsumableSlots;

    /// <summary> 헬멧에 저장 될 아이템 </summary>
    public Item HelmetItem = null;
    /// <summary> Helmet이 저장 될 Slot </summary>
    public Slot HelmetSlot;

    /// <summary> 방어구에 저장 될 아이템 </summary>
    public Item BodyArmorItem = null;
    /// <summary> BodyArmor가 저장 될 Slot </summary>
    public Slot BodyArmorSlot;

    /// <summary> 가방에 저장 될 아이템 </summary>
    public Item BackpackItem = null;
    /// <summary> Backpack이 저장 될 Slot </summary>
    public Slot BackpackSlot;

    /* ---------------------------------------------------------------------------------------------------------------------------------------------------------- */

    /// <summary> 에디터 상에서 실행 되는 함수 </summary>
    private void OnValidate()
    {
        // 각 Bag에 담겨있는 Slot의 정보를 불러온다.
        ItemSlots = ItemBag.GetComponentsInChildren<Slot>();
        PrimarySlots = PrimaryBag.GetComponentsInChildren<Slot>();
        SecondarySlots = SecondaryBag.GetComponentsInChildren<Slot>();
        ConsumableSlots = ConsumableBag.GetComponentsInChildren<Slot>();

        RefreshList();
    }

    /// <summary> 프로세스가 시작되기 전에 실행되는 함수 </summary>
    private void Awake()
    {
        //리스트 안에 빈 오브젝트를 집어넣어서 최대 크기 지정
        GenerateInventory();
        //리스트 인덱스, 아이템 정보 새로고침
        RefreshList();
    }

    /* ---------------------------------------------------------------------------------------------------------------------------------------------------------- */

    /// <summary> 모든 Bag에 아이템 정보, 인덱스 새로고침 </summary>
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

    /// <summary> 아이템, 인덱스 새로고침 overwrite </summary>
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

    /// <summary> 아이템 목록, 인덱스 새로고침 </summary>
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

    /// <summary> 모든 Bag에 빈 슬롯 지정 </summary>
    public void GenerateInventory()
    {
        SlotGenrate(PrimaryItems, _PrimaryCapacity);
        SlotGenrate(ConsumableItems, _ConsumableCapacity);
        SlotGenrate(items, _itemsCapacity);
    }

    /// <summary> 리스트 안에 빈 슬롯을 집어넣어서 최대 크기 지정 </summary>
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

    /// <summary> 아이템 추가 함수 </summary>
    /// <param name="_item">추가 될 아이템 정보</param>
    public void AddItem(ItemData _item)
    {
        //아이템 타입 검사
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
                    Debug.Log("슬롯이 가득 차 있습니다.");
                }
                break;
            default:
                break;
        }
        RefreshList();

    }

    /// <summary> 아이템 타입 체크 함수 </summary>
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

    /// <summary> 슬롯에 아이템이 있는지 체크하는 함수 </summary>
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

    /// <summary> 슬롯에 아이템이 있는지 체크하는 함수 overwrite </summary>
    public bool SlotFillCheck(Item Slot)
    {
        bool isStockable = false;
        if (Slot == null)
        {
            isStockable = true;
        }
        return isStockable;
    }

    /// <summary> 아이템이 없는 슬롯의 인덱스를 반환하는 함수 </summary>
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
