using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
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
      * 
    [기능 - 기타]
    - InvertMouse(bool) : 마우스 좌클릭/우클릭 반전 여부 설정

    // 날짜 : 2020-05-03

*/

public class InventoryUI : MonoBehaviour
{
    /***********************************************************************
    *                               Option Field
    ***********************************************************************/
    #region
    [Header("Options")]
    [SerializeField, Range(0, 10)]
    private int _horizontalSlotCount = 4; // 슬롯 가로 개수
    [SerializeField, Range(0, 10)]
    private int _verticalSlotCount = 10; // 슬롯 세로 개수
    [SerializeField] private float _slotMargin = 0.0f; // 한 슬롯의 상하좌우 여백
    [SerializeField] private float _contentAreaPadding = 20.0f; // 인벤토리 영역의 내부 여백
    [SerializeField, Range(25, 80)] private float _slotSize = 25.0f; // 각 슬롯의 크기

    [Space]
    //[SerializeField] private bool _showTolltip = true;
    //[SerializeField] private bool _showHighlist = true;
    //[SerializeField] private bool _showRemovingPopup = true;

    [Header("Connected Objects")]
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform ItemBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform PrimaryBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform SecondaryBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform ConsumableBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform EquipmentBag;

    [SerializeField] private GameObject _slotUiPrefab;     // 슬롯의 원본 프리팹
    //[SerializeField] private ItemTooltipUI _itemTooltip;   // 아이템 정보를 보여줄 툴팁 UI
    //[SerializeField] private InventoryPopupUI _popup;      // 팝업 UI 관리 객체

    [Header("Buttons")]
    //[SerializeField] private Button _trimButton;
    //[SerializeField] private Button _sortButton;

    #endregion

    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region
    /// <summary> 연결된 인벤토리 </summary>
    private Inventory _inventory;

    #region 슬롯
    /// <summary> Slot을 담을 리스트 </summary>
    public List<Slot> ItemSlots;

    /// <summary> Slot을 담을 리스트 </summary>
    public List<Slot> PrimarySlots;

    /// <summary> Slot�� ���� �� ���� </summary>
    public List<Slot> SecondarySlots;

    /// <summary> Slot을 담을 리스트 </summary>
    public List<Slot> ConsumableSlots;

    /// <summary> Slot을 담을 공간 </summary>
    public Slot HelmetSlot;
    /// <summary> Slot을 담을 공간 </summary>
    public Slot BodyArmorSlot;
    /// <summary> Slot을 담을 공간 </summary>
    public Slot BackpackSlot;
    #endregion
    private GraphicRaycaster _gr; // 캔버스 내의 오브젝트
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;


    private enum FilterOption
    {
        Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
    }
    private FilterOption _currentFilterOption = FilterOption.Any;

    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region

    private void OnValidate()
    {
        
        FindBag(out ItemBag, "BagBackpack");
        FindBag(out PrimaryBag, "BagPrimary");
        FindBag(out SecondaryBag, "BagSecondary");
        FindBag(out ConsumableBag, "BagExpand");
        FindBag(out EquipmentBag, "EquipmentBag");
    }

    private void Awake()
    {
        Init();
        InitSlots();
    }
    private void Update()
    {

    }
    #endregion

    /***********************************************************************
    *                               Init Methods
    ***********************************************************************/
    #region
    private void Init()
    {
        TryGetComponent(out _gr);
        if (_gr == null) _gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>(10);

        // Item Tooltip UI
        //if(_itemTooltip == null)
        //{
        //    // 인스펙터에서 아이템 툴팁 UI를 직접 지정하지 않아 자식에서 발견하여 초기화
        //    _itemTooltip = GetComponentInChildren<ItemTooltipUI>();
        //}
    }

    private void InitSlots()
    {
        // 슬롯 프리팹 설정
        _slotUiPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(_slotSize, _slotSize); // 슬롯 사이즈 설정

        _slotUiPrefab.TryGetComponent(out Slot itemSlot);
        if (itemSlot == null) _slotUiPrefab.AddComponent<Slot>(); // 슬롯에 Slot 스크립트 부착

        _slotUiPrefab.SetActive(false);

        // --
        Vector2 beginPos = new Vector2(_contentAreaPadding, -_contentAreaPadding);
        Vector2 curPos = beginPos;

        ItemSlots = new List<Slot>(_verticalSlotCount * _horizontalSlotCount);

        // 슬롯 동적 생성
        for(int j = 0; j < _verticalSlotCount; j++)
        {
            for(int i = 0; i < _horizontalSlotCount; i++)
            {
                int slotIndex = (_horizontalSlotCount * j) + i; // 인덱스는 0부터 시작

                var slotRT = CloneSlot(ItemBag);
                slotRT.pivot = new Vector2(0.0f, 1.0f); // Left Top
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                var slotUI = slotRT.GetComponent<Slot>();
                slotUI.SetSlotIndex(slotIndex);
                ItemSlots.Add(slotUI);

                // Next X Pos
                curPos.x += (_slotMargin + _slotSize);
            }
            // Next Line
            curPos.x = beginPos.x;
            curPos.y = (_slotMargin + _slotSize);
        }

        // 슬롯 프리팹 - 프리팹이 아닌 경우 파괴
        if (_slotUiPrefab.scene.rootCount != 0) Destroy(_slotUiPrefab);

        RectTransform CloneSlot(Transform Bag)
        {
            GameObject slotGo = Instantiate(_slotUiPrefab);
            RectTransform rt = slotGo.GetComponent<RectTransform>();
            rt.SetParent(Bag);

            return rt;
        }
    }
    #endregion

    /***********************************************************************
    *                               Public Fields
    ***********************************************************************/
    #region
    
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> Slot들을 보관하는 Bag 탐색하여 지정 </summary>
    /// <param name="Bag">슬롯을 보관할 Bag</param>
    /// <param name="name">Bag의 이름</param>
    void FindBag(out Transform Bag, string name)
    {
        Bag = this.transform.Find(name);
        if(Bag == null)
        {
            Bag = this.GetComponentInChildren<GridLayoutGroup>().transform;
        }
    }
    #endregion


    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region
    /// <summary> 인벤토리 참조 등록 (인벤토리에서 직접 호출) </summary>
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

    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void HideItemAmountText(List<Slot> _slotUIList, int index)
    {
        //EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    /// <summary> 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기 </summary>
    public void RemoveItem(List<Slot> _slotUIList, int index)
    {
        //EditorLog($"Remove Item : Slot [{index}]");

        _slotUIList[index].SlotItem.RemoveItem();
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
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Secondary:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Helmet:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Bodyarmor:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Backpack:
                    isFiltered = (itemData is CountableItemData);
                    break;

                case FilterOption.Expand:
                    isFiltered = (itemData is CountableItemData);
                    break;
            }

        _slotUIList[index].SetItemAccessibleState(isFiltered);
    }

    /// <summary> 모든 슬롯 필터 상태 업데이트 </summary>
    public void UpdateAllSlotFilters(int Capacity)
    {
        int Backpack = _inventory.ItemCapacity;
        int Primary = _inventory.PrimaryCapacity;
        int Secondary = _inventory.SecondaryCapacity;
        int Consumable = _inventory.ConsumableCapacity;

        //SetBag(Backpack, ItemSlots, _inventory.Items);
        SetBag(Primary, PrimarySlots, _inventory.PrimaryItems);
        SetBag(Secondary, SecondarySlots, _inventory.SecondaryItems);
        SetBag(Consumable, ConsumableSlots, _inventory.ConsumableItems);

        // 로컬 함수
        void SetBag(int Bag, List<Slot>slotList, List<Item>itemList)
        {
            for (int i = 0; i < Bag; i++)
            {
                ItemData data = _inventory.GetItemData(i, Capacity, itemList);
                UpdateSlotFilterState(slotList, i, data);
            }
        }
    }
    #endregion
}
