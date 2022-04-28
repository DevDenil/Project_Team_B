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

    private void Start()
    {
        CurParent = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //CurParent = this.transform.parent;
        this.transform.SetParent(CurParent.parent);
        //DragOffset = (Vector2)this.transform.position - eventData.position;
        //this.gameObject.GetComponentInChildren<Image>().raycastTarget = false;
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
            //this.gameObject.GetComponentInChildren<Image>().raycastTarget = true;
        }
        else
        {
            Vector3 DropPos = this.GetComponentInParent<Inventory>().myPlayerPos.position;
            DropPos.y += 2.0f;

            this.transform.SetParent(CurParent);
            this.transform.localPosition = Vector2.zero;
            this.gameObject.GetComponent<Image>().raycastTarget = true;

            Debug.Log(this.gameObject.GetComponentInParent<Slot>().item._itemPrefab);

            Instantiate(this.gameObject.GetComponentInParent<Slot>().item._itemPrefab,
                DropPos, Quaternion.identity);

            int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
            this.gameObject.GetComponentInParent<Inventory>().items.RemoveAt(CurIndex);
            this.GetComponentInParent<Inventory>().RefreshSlot();

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
