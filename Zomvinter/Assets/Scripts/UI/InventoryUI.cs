using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    /* 구현 할 기능
        [기능 - 에디터 전용]
        - 게임 시작 시 동적으로 생성될 슬롯 미리보기(개수, 크기 미리보기 가능)

        [기능 - 유저 인터페이스]
        - 슬롯에 마우스 올리기
          - 사용 가능 슬롯 : 하이라이트 이미지 표시
          - 아이템 존재 슬롯 : 아이템 정보 툴팁 표시

        - 드래그 앤 드롭
          - 아이템 존재 슬롯 -> 아이템 존재 슬롯 : 두 아이템 위치 교환
          - 아이템 존재 슬롯 -> 아이템 미존재 슬롯 : 아이템 위치 변경
            - Shift 또는 Ctrl 누른 상태일 경우 : 셀 수 있는 아이템 수량 나누기
          - 아이템 존재 슬롯 -> UI 바깥 : 아이템 버리기

        - 슬롯 우클릭
          - 사용 가능한 아이템일 경우 : 아이템 사용

        - 기능 버튼(좌측 상단)
          - Trim : 앞에서부터 빈 칸 없이 아이템 채우기
          - Sort : 정해진 가중치대로 아이템 정렬

        - 필터 버튼(우측 상단)
          - [A] : 모든 아이템 필터링
          - [E] : 장비 아이템 필터링
          - [P] : 소비 아이템 필터링

          * 필터링에서 제외된 아이템 슬롯들은 조작 불가

        [기능 - 기타]
        - InvertMouse(bool) : 마우스 좌클릭/우클릭 반전 여부 설정

        날짜 : 2022-05-04
    */


    /// <summary> 슬롯에 아이템 등록 </summary>





    /***********************************************************************
    *                               Option Field
    ***********************************************************************/
    #region

    #endregion

    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region
    /// <summary> 연결된 인벤토리 </summary>
    private Inventory _inventory;

    /// <summary> Slot을 모아놓는 오브젝트 </summary>
    private Transform ItemBag;
    /// <summary> Slot을 모아놓는 오브젝트 </summary>
    private Transform PrimaryBag;
    /// <summary> Slot을 모아놓는 오브젝트 </summary>
    private Transform SecondaryBag;
    /// <summary> Slot을 모아놓는 오브젝트 </summary>
    private Transform ConsumableBag;
    /// <summary> Slot을 모아놓는 오브젝트 </summary>
    private Transform EquipmentBag;


    private enum FilterOption
    {
        Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
    }
    private FilterOption _currentFilterOption = FilterOption.Any;

    #endregion

    

    /***********************************************************************
    *                               Public Fields
    ***********************************************************************/
    #region
    /// <summary> Slot들을 저장 할 리스트 </summary>
    public List<Slot> ItemSlots = new List<Slot>();

    /// <summary> Slot들을 저장 할 리스트 </summary>
    public List<Slot> PirmarySlots = new List<Slot>();

    /// <summary> Slot들을 저장 할 리스트 </summary>
    public List<Slot> ConsumableSlots = new List<Slot>();

    /// <summary> Slot을 저장 할 변수 </summary>
    public Slot SecondarySlots;

    /// <summary> Slot을 저장 할 변수 </summary>
    public Slot HelmetSlot;

    /// <summary> Slot을 저장 할 변수 </summary>
    public Slot BodyArmorSlot;

    /// <summary> Slot을 저장 할 변수 </summary>
    public Slot BackpackSlot;
    #endregion
    
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region
    private void Awake()
    {
        FindBag(out ItemBag, "BagBackpack");
        FindBag(out PrimaryBag, "BagPrimary");
        FindBag(out SecondaryBag, "BagSecondary");
        FindBag(out ConsumableBag, "BagExpand");
        FindBag(out EquipmentBag, "EquipmentBag");
    }
    private void Update()
    {

    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> 슬롯들을 담고 있는 가방을 찾는 함수 </summary>
    /// <param name="Bag">가방을 저장할 인자</param>
    /// <param name="name">가방의 이름</param>
    void FindBag(out Transform Bag, string name)
    {
        Bag = this.transform.Find(name);
    }
    #endregion


    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region
    /// <summary> 인벤토리 참조 등록 (Inventory.cs에서 직접 호출) </summary>
    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }

    /// <summary> 슬롯에 아이템 아이콘 등록 </summary>
    public void SetItemIcon(List<Slot> _slotUIList, int index, Sprite icon)
    {
        //EditorLog($"Set Item Icon : Slot [{index}]");

        _slotUIList[index].SetItem(icon);
    }
    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void SetItemAmountText(List<Slot> _slotUIList, int index, int amount)
    {
        //EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // NOTE : amount가 1 이하일 경우 텍스트 미표시
        _slotUIList[index].SetItemAmount(amount);
    }

    public void RemoveItem(List<Slot> _slotUIList, int index)
    {
        //EditorLog($"Remove Item : Slot [{index}]");

        _slotUIList[index].SlotItem.RemoveItem();
    }
    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void HideItemAmountText(List<Slot> _slotUIList, int index)
    {
        //EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    /// <summary> 접근 가능한 슬롯 범위 설정 </summary>
    public void SetAccessibleSlotRange(int accessibleSlotCount, List<Slot>_slotUIList)
    {
        for (int i = 0; i < _slotUIList.Count; i++)
        {
            _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }

    /// <summary> 특정 슬롯의 필터 상태 업데이트 </summary>
    public void UpdateSlotFilterState(List<Slot> _slotUIList, int index, ItemData itemData)
    {
        bool isFiltered = true;

        // null인 슬롯은 타입 검사 없이 필터 활성화
        if (itemData != null)
            switch (_currentFilterOption)
            {
                case FilterOption.Primary:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Secondary:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Helmet:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Bodyarmor:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Backpack:
                    isFiltered = (itemData is EquipmentData);
                    break;

                case FilterOption.Expand:
                    isFiltered = (itemData is ConsumableData);
                    break;
            }

        _slotUIList[index].SetItemAccessibleState(isFiltered);
    }
    #endregion
}
