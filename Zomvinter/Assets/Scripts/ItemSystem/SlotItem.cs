using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool IsOverUI = false;

    //Vector2 DragOffset = Vector2.zero;
    public Transform CurParent = null;
    public int CurIndex = 0;

    /***********************************************************************
    *                               Option Fields
    ***********************************************************************/

    #region �ɼ�

    [Tooltip("������ ������ �̹���")]
    [SerializeField] public Image _iconImage;

    [Tooltip("������ ���� �ؽ�Ʈ")]
    [SerializeField] public TMPro.TMP_Text _amountText;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/

    #region ������Ƽ
    /// <summary> Inventory.cs ������ ���� </summary>
    Inventory myInventry;[SerializeField]

    Item _itemProperty;
    public Item ItemProperty
    {
        get { return _itemProperty; }
        set
        {
            _itemProperty = value;
            if (_itemProperty != null)
            {
                _iconImage.sprite = _itemProperty.Data.ItemImage;
                _iconImage.color = new Color(1, 1, 1, 1);
                switch (this.GetComponentInParent<Slot>().SlotState)
                {
                    case ItemType.Primary:
                        _iconImage.GetComponent<RectTransform>().sizeDelta = new Vector2(80.0f, 45.0f);
                        break;
                    case ItemType.Secondary:
                        _iconImage.GetComponent<RectTransform>().sizeDelta = new Vector2(80.0f, 45.0f);
                        break;
                    default:
                        _iconImage.GetComponent<RectTransform>().sizeDelta = new Vector2(25.0f, 25.0f);
                        break;
                }
                _amountText.text = _itemProperty.Data.ItemName.ToString();
                this.GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                _iconImage.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }


    #endregion
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region
    private InventoryUI _inventoryUI; // InventoryUI ��ũ��Ʈ

    private GameObject _iconGo; // ������ ���� ������Ʈ
    private GameObject _textGo; // �ؽ�Ʈ ���� ������Ʈ
    private GameObject _highlightGo; // ���̶���Ʈ ���� ������Ʈ

    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Private �Լ�
    /// <summary> ������ ���̱� </summary>
    public void ShowIcon() => _iconGo.SetActive(true);
    /// <summary> ������ ����� </summary>
    public void HideIcon() => _iconGo.SetActive(false);

    /// <summary> �ؽ�Ʈ ���̱� </summary>
    public void ShowText() => _textGo.SetActive(true);
    /// <summary> �ؽ�Ʈ ����� </summary>
    public void HideText() => _textGo.SetActive(false);

    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region
    

    /// <summary> ���Կ� ������ ��� </summary>
    public void SetItem(Sprite itemSprite)
    {
        //if (!this.IsAccessible) return;

        if (itemSprite != null)
        {
            _iconImage.sprite = itemSprite;
            ShowIcon();
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
        HideIcon();
        HideText();
    }
    #endregion

    private void Start()
    {
        CurParent = this.transform.parent;
        CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
        if(ItemProperty == null)
        {
            this.GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            this.GetComponent<Image>().raycastTarget = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurParent = this.transform.parent;
        this.transform.SetParent(CurParent.parent);
        this.gameObject.GetComponent<Image>().raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        IsOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsOverUI)
        {
            this.transform.SetParent(CurParent);
            this.transform.localPosition = Vector2.zero;
            this.gameObject.GetComponent<Image>().raycastTarget = true;
            //this.GetComponentInParent<Inventory>().RefreshList();
        }
        else
        {
            Vector3 DropPos = this.GetComponentInParent<Inventory>().myPlayerPos.position;
            DropPos.y += 2.0f;

            this.transform.SetParent(CurParent);
            this.transform.localPosition = Vector2.zero;
            this.gameObject.GetComponent<Image>().raycastTarget = true;

            if (this.gameObject.GetComponent<SlotItem>()._itemProperty.Data.ItemPrefab != null)
            {
                Instantiate(this.gameObject.GetComponent<SlotItem>()._itemProperty.Data.ItemPrefab,
                    DropPos, Quaternion.identity);
            }
            int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
            //this.gameObject.GetComponentInParent<Inventory>().Items.RemoveAt(CurIndex);
            //this.GetComponent<SlotItem>().image.sprite = null;
            this.GetComponent<SlotItem>()._amountText.text = null;
            //this.GetComponentInParent<Inventory>().RefreshList();

        }
    }

    public void ChangeParent(Transform parent)
    {
        SlotItem tempItem = parent.GetComponentInChildren<SlotItem>();
        if (tempItem != null)
        {
            tempItem.ChangeParent(CurParent);
        }

        CurParent = parent;
        this.transform.SetParent(CurParent);
        CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
        this.transform.localPosition = Vector2.zero;
    }

    /// <summary> ��ġ�� �ٲ� �������� Inventory.Items ����Ʈ���� Swap </summary>
    /// <typeparam name="Item">�κ��丮 ����Ʈ�� �ҷ��� Ÿ��</typeparam>
    /// <param name="items">�ٲ� �������� ������ �ӽ� ���� �� ������ ����</param>
    /// <param name="Swap">�ٲ��� ���ϴ� ����� ����</param>
    public void SwapItem(List<Item> items, Transform Swap)
    {
        //�ӽ� ���� �� ������ Ÿ�� ����
        Item temp;
        Item SwapItem = this.GetComponentInChildren<SlotItem>().ItemProperty;
        int SwapedIndex = Swap.GetComponentInChildren<SlotItem>().CurIndex;
        Item SwapedItem = Swap.GetComponentInChildren<SlotItem>().ItemProperty;
        if (Swap.GetComponent<Slot>().SlotState == SwapItem.Data.ItemType || Swap.GetComponent<Slot>().SlotState == ItemType.Any)
        {
            if (SwapedItem != null)
            {
                temp = items[SwapedIndex];
                items[SwapedIndex] = items[CurIndex];
                items[CurIndex] = temp;
            }
            else
            {
                items[SwapedIndex] = items[CurIndex];
                items[CurIndex] = null;
            }
        }
    }
}
