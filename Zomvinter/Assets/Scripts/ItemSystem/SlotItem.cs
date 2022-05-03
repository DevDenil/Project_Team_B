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

    [SerializeField]
    /// <summary> ���Կ� ǥ�� �� �̹��� </summary>
    public Image image;
    /// <summary> ���Կ� ǥ�� �� �ؽ�Ʈ </summary>
    [SerializeField]
    public TMPro.TMP_Text text;

    [SerializeField]
    Item _itemProperty;
    public Item ItemProperty
    {
        get { return _itemProperty; }
        set
        {
            _itemProperty = value;
            if (_itemProperty != null)
            {
                image.sprite = _itemProperty._itemImage;
                image.color = new Color(1, 1, 1, 1);
                switch(this.GetComponentInParent<Slot>().SlotState)
                {
                    case ItemType.Primary:
                        image.GetComponent<RectTransform>().sizeDelta = new Vector2(80.0f, 45.0f);
                        break;
                    case ItemType.Secondary:
                        image.GetComponent<RectTransform>().sizeDelta = new Vector2(80.0f, 45.0f);
                        break;
                    default:
                        image.GetComponent<RectTransform>().sizeDelta = new Vector2(25.0f, 25.0f);
                        break;
                }
                text.text = _itemProperty._maxAmount.ToString();
                this.GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                image.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

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
            this.GetComponentInParent<Inventory>().RefreshList();
        }
        else
        {
            Vector3 DropPos = this.GetComponentInParent<Inventory>().myPlayerPos.position;
            DropPos.y += 2.0f;

            this.transform.SetParent(CurParent);
            this.transform.localPosition = Vector2.zero;
            this.gameObject.GetComponent<Image>().raycastTarget = true;

            if (this.gameObject.GetComponent<SlotItem>()._itemProperty._itemPrefab != null)
            {
                Instantiate(this.gameObject.GetComponent<SlotItem>()._itemProperty._itemPrefab,
                    DropPos, Quaternion.identity);
            }
            int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
            this.gameObject.GetComponentInParent<Inventory>().items.RemoveAt(CurIndex);
            //this.GetComponent<SlotItem>().image.sprite = null;
            this.GetComponent<SlotItem>().text.text = null;
            this.GetComponentInParent<Inventory>().RefreshList();

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
        if (Swap.GetComponent<Slot>().SlotState == SwapItem._type || Swap.GetComponent<Slot>().SlotState == ItemType.Any)
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
