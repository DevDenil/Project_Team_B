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
    public Image image;
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
                text.text = _itemProperty._maxAmount.ToString();

            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    private void Start()
    {
        CurParent = this.transform.parent;
        CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurParent = this.transform.parent;
        this.transform.SetParent(CurParent.parent);
        //DragOffset = (Vector2)this.transform.position - eventData.position;
        this.gameObject.GetComponent<Image>().raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position; //+ DragOffset
        IsOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsOverUI)
        {
            //Debug.Log(eventData.pointerDrag.name);
            this.transform.SetParent(CurParent);
            this.transform.localPosition = Vector2.zero;
            this.gameObject.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            Vector3 DropPos = this.GetComponentInParent<Inventory>().myPlayerPos.position;
            DropPos.y += 2.0f;

            this.transform.SetParent(CurParent);
            this.transform.localPosition = Vector2.zero;
            this.gameObject.GetComponent<Image>().raycastTarget = true;

            Instantiate(this.gameObject.GetComponent<SlotItem>()._itemProperty._itemPrefab,
                DropPos, Quaternion.identity);

            int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
            this.gameObject.GetComponentInParent<Inventory>().items.RemoveAt(CurIndex);
            this.GetComponent<SlotItem>().image.sprite = null;
            this.GetComponent<SlotItem>().text.text = null;
            this.GetComponentInParent<Inventory>().RefreshSlot();

        }
    }
    //public void OnDrop(PointerEventData eventData)
    //{
    //    Debug.Log("OnDrop");
    //    SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
    //    if (item != null)
    //    {
    //        item.ChangeParent(CurParent);
    //    }
    //}

    public void ChangeParent(Transform parent)
    {
        SlotItem tempItem = parent.GetComponentInChildren<SlotItem>();
        if (tempItem != null)
        {
            tempItem.ChangeParent(CurParent);
        }

        CurParent = parent;
        this.transform.SetParent(CurParent);
        this.transform.localPosition = Vector2.zero;
        this.SetIndex();
    }
    public void ChangeIndex<Item>(List<Item> items, Transform parent)
    {
        Item temp = items[parent.GetComponentInChildren<SlotItem>().CurIndex];
        if(temp != null)
        {
            items[parent.GetComponentInChildren<SlotItem>().CurIndex] = items[CurIndex];
            items[CurIndex] = temp;
        }
    }

    public void SetIndex()
    {
        CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
    }
}
