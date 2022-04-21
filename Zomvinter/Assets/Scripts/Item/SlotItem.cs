using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool IsOverUI = false;
    
    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurParent = this.transform.parent;
        this.transform.SetParent(CurParent.parent);
        DragOffset = (Vector2)this.transform.position - eventData.position;
        this.gameObject.GetComponent<Image>().raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = DragOffset + eventData.position;
        IsOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsOverUI)
        {
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

            Debug.Log(this.gameObject.GetComponentInParent<Slot>().item._itemPrefab);

            Instantiate(this.gameObject.GetComponentInParent<Slot>().item._itemPrefab,
                DropPos, Quaternion.identity);

            int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
            this.gameObject.GetComponentInParent<Inventory>().items.RemoveAt(CurIndex);
            this.GetComponentInParent<Inventory>().RefreshSlot();

        }
    }

    Vector2 DragOffset = Vector2.zero;
    public Transform CurParent = null;

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
    }
}
