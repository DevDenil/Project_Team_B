using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool IsOverUI = false;

    Vector2 DragOffset = Vector2.zero;
    public Transform CurParent = null;

    [SerializeField]
    public Image image;
    [SerializeField]
    public TMPro.TMP_Text text;

    [SerializeField]
    Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                //image.sprite = item._itemImage;
                //image.color = new Color(1, 1, 1, 1);
                text.text = item._maxAmount.ToString();

            }
            else
            {
                //image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    private void Start()
    {
        CurParent = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //CurParent = this.transform.parent;
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
            Debug.Log("OnEndDrag");
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

            Instantiate(this.gameObject.GetComponent<SlotItem>().item._itemPrefab,
                DropPos, Quaternion.identity);

            int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
            this.gameObject.GetComponentInParent<Inventory>().items.RemoveAt(CurIndex);
            this.GetComponent<SlotItem>().image.sprite = null;
            this.GetComponent<SlotItem>().text.text = null;
            this.GetComponentInParent<Inventory>().RefreshSlot();

        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        if (item != null)
        {
            item.ChangeParent(CurParent);
        }
    }

    public void ChangeParent(Transform parent)
    {
        SlotItem tempItem = parent.GetComponent<SlotItem>();
        if (tempItem != null)
        {
            tempItem.ChangeParent(CurParent);
        }

        CurParent = parent;
        this.transform.SetParent(CurParent);
        this.transform.localPosition = Vector2.zero;
    }
}
