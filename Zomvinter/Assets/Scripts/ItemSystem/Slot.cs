using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region 프로퍼티
    Inventory _Inventory => GetComponentInParent<Inventory>();
    /// <summary> 슬롯의 인덱스 </summary>
    public int SlotIndex { get; private set; }

    public SlotItem SlotItem => GetComponentInChildren<SlotItem>();

    /// <summary> 슬롯이 아이템을 보유하고 있는지 여부 </summary>
    public bool HasItem => SlotItem.ItemProperty != null;

    /// <summary> 접근 가능한 슬롯인지 여부 </summary>
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;

    [SerializeField]
    public ItemType SlotState;
    #endregion
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region 필드
    private Image _slotImage; // 슬롯의 이미지

    // 현재 하이라이트 알파값
    //private float _currentHLAlpha = 0.0f;

    /// <summary> 슬롯 접근가능 여부 </summary>
    private bool _isAccessibleSlot = true;
    /// <summary> 아이템 접근가능 여부 </summary>
    private bool _isAccessibleItem = true;

    /// <summary> 비활성화된 슬롯의 색상 </summary>
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    /// <summary> 비활성화된 아이콘 색상 </summary>
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    #endregion

    /***********************************************************************
    *                               Unity Event
    ***********************************************************************/
    #region
    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수
    public void SetSlotIndex(int index) => SlotIndex = index;

    /// <summary> 슬롯 자체의 활성화/비활성화 여부 설정 </summary>
    public void SetSlotAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if (_isAccessibleSlot == value) return;

        if (value)
        {
            _slotImage.color = Color.black;
        }
        else
        {
            //_slotImage.color = InaccessibleSlotColor;
            //SlotItem.HideIcon();
            //SlotItem.HideText();
        }

        _isAccessibleSlot = value;
    }/// <summary> 아이템 활성화/비활성화 여부 설정 </summary>
    public void SetItemAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if (_isAccessibleItem == value) return;

        if (value)
        {
            SlotItem._iconImage.color = Color.white;
            SlotItem._amountText.color = Color.white;
        }
        else
        {
            SlotItem._iconImage.color = InaccessibleIconColor;
            SlotItem._amountText.color = InaccessibleIconColor;
        }

        _isAccessibleItem = value;
    }
    #endregion

    private void Start()
    {

    }

    public void SetItem(Sprite itemSprite)
    {
        //if (!this.IsAccessible) return;

        if (itemSprite != null)
        {
            SlotItem._iconImage.sprite = itemSprite;
            SlotItem.ShowIcon();
        }
        else
        {
            SlotItem.RemoveItem();
        }
    }

    /// <summary> 아이템 개수 텍스트 설정(amount가 1 이하일 경우 텍스트 미표시) </summary>
    public void SetItemAmount(int amount)
    {
        //if (!this.IsAccessible) return;

        if (HasItem && amount > 1)
            SlotItem.ShowText();
        else
            SlotItem.HideText();

        SlotItem._amountText.text = amount.ToString();
    }

    /***********************************************************************
    *                               Old Methods
    ***********************************************************************/

    /// <summary> OnDrop 이벤트가 발생했을 때 실행 될 이벤트 </summary>
    /// <param name="eventData">OnDrop이 발생 되는 지점의 오브젝트</param>
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        Slot itemSlot = eventData.pointerDrag.GetComponentInParent<Slot>();
        if (item != null)
        {
            if (itemSlot.SlotState == ItemType.Primary)
            {
                item.SwapItem(_Inventory.PrimaryItems, this.transform);
            }
            else if (itemSlot.SlotState == ItemType.Secondary)
            {
                item.SwapItem(_Inventory.SecondaryItems, this.transform);
            }
            else if (itemSlot.SlotState == ItemType.Expand)
            {
                item.SwapItem(_Inventory.ConsumableItems, this.transform);
            }
            else if(itemSlot.SlotState == ItemType.Any)
            {
                //item.SwapItem(_Inventory.Items, this.transform);
            }
            item.ChangeParent(this.transform);
            //item.SetIndex();
        }
        this.GetComponentInChildren<SlotItem>().CurIndex = SlotIndex;
        //_Inventory.RefreshList();
    }
}
