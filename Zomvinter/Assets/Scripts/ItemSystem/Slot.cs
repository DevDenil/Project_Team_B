using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    /***********************************************************************
*                               Option Fields
***********************************************************************/
    #region .
    [Tooltip("���� ������ �����ܰ� ���� ������ ����")]
    [SerializeField] private float _padding = 1.0f;

    [Tooltip("������ ������ �̹���")]
    [SerializeField] private Image _iconImage;

    [Tooltip("������ ���� �ؽ�Ʈ")]
    [SerializeField] private TMPro.TMP_Text _amountText;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region ������Ƽ
    /// <summary> ������ �ε��� </summary>
    [SerializeField]
    public int SlotIndex { get; private set; }

    public ItemData _item;

    public ItemData ItemProperties
    {
        get { return _item; }
        set { _item = value; }
    }

    /// <summary> ������ Ÿ�� </summary>
    public ItemType SlotState;

    /// <summary> ������ �������� �����ϰ� �ִ��� ���� </summary>
    public bool HasItem => ItemProperties != null;

    /// <summary> ���� ������ �������� ���� </summary>
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;
    #endregion
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region �ʵ�
    /// <summary> �κ��丮 </summary>
    private Inventory _Inventory;
    /// <summary> �κ��丮UI </summary>
    private InventoryUI _InventoryUI;
    /// <summary> ���� </summary>
    private SlotItem _slotItem;

    /// <summary> ���� �̹��� </summary>
    private Image _slotImage;

    private RectTransform _slotRect;
    private RectTransform _iconRect;

    [SerializeField]
    private GameObject _iconGo;
    [SerializeField]
    private GameObject _textGo;

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
    private void OnValidate()
    {
        InitComponenet();
        InitValue();
    }

    private void Awake()
    {
        Debug.Log(SlotIndex);
    }

    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> �ʱ� ���� �Լ� </summary>
    private void InitComponenet ()
    {
        // Scripts
        _Inventory = GetComponentInParent<Inventory>();
        _InventoryUI = GetComponentInParent<InventoryUI>();
        _slotItem = GetComponentInChildren<SlotItem>();

        // Rect
        _amountText = GetComponentInChildren<TMPro.TMP_Text>();
        _slotRect = GetComponent<RectTransform>();
        _iconRect = _iconImage.rectTransform;

        // Image
        _slotImage = GetComponent<Image>();

        // GameObject
        _iconGo = _iconImage.gameObject;
        _textGo = _amountText.gameObject;
    }

    private void InitValue()
    {
        // 1. Item Icon, Highlight Rect

        switch (TypeCheck())
        {
            case 1:
                
            case 2:
                _iconRect.pivot = new Vector2(0.5f, 0.5f); // �ǹ��� �߾�
                _iconRect.anchoredPosition = new Vector2(0.0f, 15.0f); // ��ġ
                _iconRect.sizeDelta = new Vector2(80.0f, 32.5f); // ������
                break;
            case 3:
                _iconRect.pivot = new Vector2(0.5f, 0.0f); // �ǹ��� �߾�
                _iconRect.anchoredPosition = new Vector2(0.0f, 0.0f); // ��ġ
                _iconRect.sizeDelta = new Vector2(25.0f, 25.0f); // ������
                break;
        }

        // 2. Image
        _iconImage.raycastTarget = false;

        // 3. Deactivate Icon
        //HideIcon();

        // 4. GetItem
        switch (TypeCheck())
        {
            case 1:
                //GetItemInfo(_Inventory.PrimaryItems);
                break;
            case 2:
                //GetItemInfo(_Inventory.SecondaryItems);
                break;
            case 3:
                //GetItemInfo(_Inventory.ConsumableItems);
                break;
            case 4:
                //GetItemInfo(_Inventory.HelmetItem);
                break;
            case 5:
                //GetItemInfo(_Inventory.BodyArmorItem);
                break;
            case 6:
                //GetItemInfo(_Inventory.BackpackItem);
                break;
            case 7:
                //GetItemInfo(_Inventory.Items);
                break;
            default:
                break;
        }
    }

    private void ShowIcon() => _iconGo.SetActive(true);
    //private void HideIcon() => _iconGo.SetActive(false);

    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private int TypeCheck()
    {
        if(SlotState == ItemType.Primary)
        {
            return 1;
        }
        else if (SlotState == ItemType.Secondary)
        {
            return 2;
        }
        else if( SlotState == ItemType.Expand)
        {
            return 3;
        }
        else if (SlotState == ItemType.Helmet)
        {
            return 4;
        }
        else if (SlotState == ItemType.Bodyarmor)
        {
            return 5;
        }
        else if (SlotState == ItemType.Backpack)
        {
            return 6;
        }
        else
        {
            return 7;
        }
    }

    private void GetItemInfo(List<Item> list)
    {
        //if (list[SlotIndex].Data != null)
        //{
        //    ItemProperties = list[SlotIndex].Data;
        //}
    }
    private void GetItemInfo(Item item)
    {
        //if (item != null)
        //{
        //    ItemProperties = item.Data;
        //}
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    public int SetIndex(int index) => SlotIndex = index;

    /// <summary> ���� ��ü�� Ȱ��ȭ/��Ȱ��ȭ ���� ���� </summary>
    public void SetSlotAccessState(bool value)
    {
        // �ߺ� ó���� ����
        if (_isAccessibleSlot == value) return;

        if (value)
        {
            // ���� �̹��� �� Ȱ��ȭ
            _slotImage.color = Color.black;
        }
        else
        {
            // ������ �̹��� �� Ȱ��ȭ
            _slotImage.color = InaccessibleSlotColor;
            //HideIcon();
            HideText();
        }

        _isAccessibleSlot = value;
    }
    
    /// <summary> ������ Ȱ��ȭ/��Ȱ��ȭ ���� ���� </summary>
    public void SetItemAccessState(bool value)
    {
        // �ߺ� ó���� ����
        if (_isAccessibleItem == value) return;

        if (value)
        {
            Debug.Log("Filtered");
            _iconImage.raycastTarget = true;
            _iconImage.color = Color.white;
            _amountText.color = Color.white;

        }
        else
        {
            Debug.Log("Filtered");
            _iconImage.raycastTarget = false;
            _iconImage.color = InaccessibleIconColor;
            _amountText.color = InaccessibleIconColor;
        }

        _isAccessibleItem = value;
    }
    #endregion

    /// <summary> ���Կ� ������ ��� </summary>
    public void SetItem(Sprite sprite)
    {
        //if (!this.IsAccessible) return;

        if (sprite != null)
        {
            _iconImage.sprite = sprite;
            ShowIcon();
            SetSlotAccessState(true);
            SetItemAccessState(true);
        }
        else
        {
            RemoveItem();
        }
    }
    
    /// <summary> ���Կ��� ������ ���� </summary>
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        //HideIcon();
        HideText();
    }

    /// <summary> ������ ���� �ؽ�Ʈ ����(amount�� 1 ������ ��� �ؽ�Ʈ ��ǥ��) </summary>
    public void SetItemAmount(int amount)
    {
        //if (!this.IsAccessible) return;

        if (HasItem && amount > 1)
        {
            ShowText();
        }
        else
        {
            HideText();
        }

        _amountText.text = amount.ToString();
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
            item.SwapItem(_Inventory.Items, this.transform);
            item.ChangeParent(this.transform);
        }
        this.GetComponentInChildren<SlotItem>().CurIndex = SlotIndex;
    }
}
