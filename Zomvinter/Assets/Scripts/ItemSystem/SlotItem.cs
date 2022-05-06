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

    #region 옵션

    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] public Image _iconImage;

    [Tooltip("아이템 개수 텍스트")]
    [SerializeField] public TMPro.TMP_Text _amountText;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/

    #region 프로퍼티
    /// <summary> Inventory.cs 참조용 변수 </summary>
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
    private InventoryUI _inventoryUI; // InventoryUI 스크립트

    private GameObject _iconGo; // 아이콘 게임 오브젝트
    private GameObject _textGo; // 텍스트 게임 오브젝트
    private GameObject _highlightGo; // 하이라이트 게임 오브젝트

    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Private 함수
    /// <summary> 아이콘 보이기 </summary>
    public void ShowIcon() => _iconGo.SetActive(true);
    /// <summary> 아이콘 숨기기 </summary>
    public void HideIcon() => _iconGo.SetActive(false);

    /// <summary> 텍스트 보이기 </summary>
    public void ShowText() => _textGo.SetActive(true);
    /// <summary> 텍스트 숨기기 </summary>
    public void HideText() => _textGo.SetActive(false);

    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region
    

    /// <summary> 슬롯에 아이템 등록 </summary>
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

    /// <summary> 슬롯에서 아이템 제거 </summary>
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

    /// <summary> 위치가 바뀐 아이템을 Inventory.Items 리스트에서 Swap </summary>
    /// <typeparam name="Item">인벤토리 리스트를 불러올 타입</typeparam>
    /// <param name="items">바뀔 아이템의 정보가 임시 저장 될 아이템 변수</param>
    /// <param name="Swap">바뀜을 당하는 대상의 정보</param>
    public void SwapItem(List<Item> items, Transform Swap)
    {
        //임시 저장 될 아이템 타입 변수
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
