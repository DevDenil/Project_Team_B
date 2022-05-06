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
    #region ������Ƽ
    Inventory _Inventory => GetComponentInParent<Inventory>();
    /// <summary> ������ �ε��� </summary>
    public int SlotIndex { get; private set; }

    public SlotItem SlotItem => GetComponentInChildren<SlotItem>();

    /// <summary> ������ �������� �����ϰ� �ִ��� ���� </summary>
    public bool HasItem => SlotItem.ItemProperty != null;

    /// <summary> ���� ������ �������� ���� </summary>
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;

    [SerializeField]
    public ItemType SlotState;
    #endregion
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region �ʵ�
    private Image _slotImage; // ������ �̹���

    // ���� ���̶���Ʈ ���İ�
    //private float _currentHLAlpha = 0.0f;

    /// <summary> ���� ���ٰ��� ���� </summary>
    private bool _isAccessibleSlot = true;
    /// <summary> ������ ���ٰ��� ���� </summary>
    private bool _isAccessibleItem = true;

    /// <summary> ��Ȱ��ȭ�� ������ ���� </summary>
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    /// <summary> ��Ȱ��ȭ�� ������ ���� </summary>
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
    #region Public �Լ�
    public void SetSlotIndex(int index) => SlotIndex = index;

    /// <summary> ���� ��ü�� Ȱ��ȭ/��Ȱ��ȭ ���� ���� </summary>
    public void SetSlotAccessibleState(bool value)
    {
        // �ߺ� ó���� ����
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
    }/// <summary> ������ Ȱ��ȭ/��Ȱ��ȭ ���� ���� </summary>
    public void SetItemAccessibleState(bool value)
    {
        // �ߺ� ó���� ����
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

    /// <summary> ������ ���� �ؽ�Ʈ ����(amount�� 1 ������ ��� �ؽ�Ʈ ��ǥ��) </summary>
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

    /// <summary> OnDrop �̺�Ʈ�� �߻����� �� ���� �� �̺�Ʈ </summary>
    /// <param name="eventData">OnDrop�� �߻� �Ǵ� ������ ������Ʈ</param>
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
