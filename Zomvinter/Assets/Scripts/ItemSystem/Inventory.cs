using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /*
        [Item의 상속구조]
        - Item
            - Consumable : IUsableItem.Use() -> 사용 및 수량 1 소모

            - EquipmentItem
                - WeaponItem
                - ArmorItem

        [ItemData의 상속구조]
          (ItemData는 해당 아이템이 공통으로 가질 데이터 필드 모음)
          (개체마다 달라져야 하는 현재 내구도, 강화도 등은 Item 클래스에서 관리)

        - ItemData
            - ConsumableData : 효과량(Value : 회복량, 공격력 등에 사용)

            - EquipmentData : 최대 내구도
                - WeaponData : 기본 공격력
                - ArmorData : 기본 방어력
    */

    /*
        [API]
        - bool HasItem(int) : 해당 인덱스의 슬롯에 아이템이 존재하는지 여부
        - bool IsCountableItem(int) : 해당 인덱스의 아이템이 셀 수 있는 아이템인지 여부
        - int GetCurrentAmount(int) : 해당 인덱스의 아이템 수량
            - -1 : 잘못된 인덱스
            -  0 : 빈 슬롯
            -  1 : 셀 수 없는 아이템이거나 수량 1
        - ItemData GetItemData(int) : 해당 인덱스의 아이템 정보
        - string GetItemName(int) : 해당 인덱스의 아이템 이름

        - int Add(ItemData, int) : 해당 타입의 아이템을 지정한 개수만큼 인벤토리에 추가
            - 자리 부족으로 못넣은 개수만큼 리턴(0이면 모두 추가 성공했다는 의미)
        - void Remove(int) : 해당 인덱스의 슬롯에 있는 아이템 제거
        - void Swap(int, int) : 두 인덱스의 아이템 위치 서로 바꾸기
        - void SeparateAmount(int a, int b, int amount)
            - a 인덱스의 아이템이 셀 수 있는 아이템일 경우, amount만큼 분리하여 b 인덱스로 복제
        - void Use(int) : 해당 인덱스의 아이템 사용
        - void UpdateSlot(int) : 해당 인덱스의 슬롯 상태 및 UI 갱신
        - void UpdateAllSlot() : 모든 슬롯 상태 및 UI 갱신
        - void UpdateAccessibleStatesAll() : 모든 슬롯 UI에 접근 가능 여부 갱신
        - void TrimAll() : 앞에서부터 아이템 슬롯 채우기
        - void SortAll() : 앞에서부터 아이템 슬롯 채우면서 정렬
    
    // 날짜 : 2021-03-07 PM 7:33:52
    */

    /***********************************************************************
    *                               Public Properties
    ***********************************************************************/
    #region
    /// <summary> 내 플레이어의 위치 </summary>
    public Transform myPlayerPos;

    /// <summary> 백팩 수용 한도 </summary>
    [SerializeField]
    public int ItemCapacity { get; private set; }

    /// <summary> 아이템 수용 한도 </summary>
    [SerializeField]
    public int PrimaryCapacity { get; private set; }

    /// <summary> 보조무기 수용 한도 </summary>
    [SerializeField]
    public int SecondaryCapacity { get; private set; }

    /// <summary> 소모품 수용 한도 </summary>
    [SerializeField]
    public int ConsumableCapacity { get; private set; }
    #endregion
    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region
    /// <summary> 연결된 InventoryUI 스크립트 </summary>
    private InventoryUI _inventoryUI;

    #region 가방
    /// <summary> 백팩 아이템 목록 리스트 </summary>
    public List<Item> Items;
    /// <summary> 백팩 초기 수용 한도 </summary>
    [Range(4, 48)]
    private int _itemInitalCapacity = 4;
    /// <summary> 백팩 최대 수용 한도 </summary>
    [Range(4, 48)]
    private int _itemsMaxCapacity = 48;
    #endregion -------------------------------------------------------------------

    #region 주무기
    /// <summary> 주 무기 아이템 목록 리스트 </summary>
    public List<Item> PrimaryItems;

    /// <summary> 아이템 최대 수용 한도 </summary>
    [Range(2, 2)]
    private int _PrimaryMaxCapacity = 2;
    #endregion -------------------------------------------------------------------

    #region 보조무기
    /// <summary> 보조 무기 아이템 목록 리스트 </summary>
    public List<Item> SecondaryItems;

    /// <summary> 보조무기 최대 수용 한도 </summary>
    [Range(1, 1)]
    private int _SecondaryMaxCapacity = 1;
    #endregion -------------------------------------------------------------------

    #region 소모품
    /// <summary> 소모품 아이템 목록 리스트 </summary>
    public List<Item> ConsumableItems;

    /// <summary> 소모품 최대 수용 한도 </summary>
    [Range(1, 3)]
    private int _ConsumableMaxCapacity = 3;
    #endregion -------------------------------------------------------------------

    #region 장비
    /// <summary> 헬멧에 저장 될 아이템 </summary>
    public Item HelmetItem = null;

    /// <summary> 방어구에 저장 될 아이템 </summary>
    public Item BodyArmorItem = null;

    /// <summary> 가방에 저장 될 아이템 </summary>
    public Item BackpackItem = null;
    #endregion -------------------------------------------------------------------

    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region
    /// <summary> 에디터 상에서 실행 되는 함수 </summary>
    private void OnValidate()
    {
        _inventoryUI = GetComponent<InventoryUI>();
        ItemCapacity = SetInitalCapacity(_itemInitalCapacity);
        PrimaryCapacity = SetInitalCapacity(_PrimaryMaxCapacity);
        SecondaryCapacity = SetInitalCapacity(_SecondaryMaxCapacity);
        ConsumableCapacity = SetInitalCapacity(_ConsumableMaxCapacity);
    }

    /// <summary> 프로세스가 시작되기 전에 실행되는 함수 </summary>
    private void Awake()
    {

    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> 인덱스가 수용 범위 내에 있는지 검사 </summary>
    private bool IsValidIndex(int Index, int Capacity)
    {
        return Index >= 0 && Index < Capacity;
    }
    #endregion

    /// <summary> 앞에서부터 비어있는 슬롯 인덱스 탐색 </summary>
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
    /// <summary> 가방 초기용량 설정 함수 </summary>
    int SetInitalCapacity(int inital)
    {
        return inital;
    }

    /// <summary> 해당 슬롯이 아이템을 갖고 있는지 여부 </summary>
    public bool HasItem(int Index, int Capacity, List<Item> list)
    {
        return IsValidIndex(Index, Capacity) && list[Index] != null;
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수
    public void UpdateAccessibleStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(ItemCapacity, _inventoryUI.ItemSlots);
        _inventoryUI.SetAccessibleSlotRange(PrimaryCapacity, _inventoryUI.PirmarySlots);
        _inventoryUI.SetAccessibleSlotRange(ConsumableCapacity, _inventoryUI.ConsumableSlots);
    }

    /// <summary> 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색 </summary>
    //미구현

    /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
    public void UpdateSlot(int Index, int Capacity, List<Item> _item, List<Slot>_slotUIList)
    {
        if (!IsValidIndex(Index, Capacity)) return;

        Item item = _item[Index];

        // 1.아이템이 슬롯에 존재하는 경우
        if(item != null)
        {
            //아이콘 등록
            _inventoryUI.SetItemIcon(_slotUIList, Index, item.Data.ItemImage);

            // 1-1.아이템이 셀 수 있는 아이템인 경우
            if(item is Consumable con)
            {
                // 1-1-1. 수량이 0인 경우, 아이템 제거
                if (con.IsEmpty)
                {
                    _item[Index] = null;
                    RemoveIcon(_slotUIList);
                    return;
                }
                // 1-1-2. 수량 표시
                else
                {
                    _inventoryUI.SetItemAmountText(_slotUIList, Index, con.Amount);
                }
            }
            // 1-2. 셀 수 없는 아이템인 경우, 수량 텍스트 제거
            else
            {
                _inventoryUI.HideItemAmountText(_slotUIList, Index);
            }

            // 슬롯 필터 상태 업데이트
            _inventoryUI.UpdateSlotFilterState(_slotUIList, Index, item.Data);
        }
        else
        {
            RemoveIcon(_slotUIList);
        }

        // 로컬 함수 : 아이콘 제거하기
        void RemoveIcon(List<Slot>_slotsUIList)
        {
            _inventoryUI.RemoveItem(_slotsUIList, Index);
            _inventoryUI.HideItemAmountText(_slotsUIList, Index); // 수량 텍스트 숨기기
        }
    }
    #endregion


    /***********************************************************************
    *                               Old Methods
    ***********************************************************************/
    #region
    /*
    

    /// <summary> 모든 Bag에 아이템 정보, 인덱스 새로고침 </summary>
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

    /// <summary> 아이템 목록, 인덱스 새로고침 </summary>
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

    /// <summary> 아이템 추가 함수 </summary>
    /// <param name="_item">추가 될 아이템 정보</param>
    public void AddItem(Item _item)
    {
        //아이템 타입 검사
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
                    Debug.Log("슬롯이 가득 차 있습니다.");
                }
                break;
            default:
                break;
        }
        RefreshList();

    }

    /// <summary> 아이템 타입 체크 함수 </summary>
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
    
    */
    #endregion
}