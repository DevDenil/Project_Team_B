using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool IsOverUI = false;

    /***********************************************************************
    *                               Option Fields
    ***********************************************************************/
    #region
    
    /// <summary> ������ �̹����� �θ� ���� </summary>
    [SerializeField] private Transform CurParent = null;
    #endregion
    /***********************************************************************
    *                               Properties
    ***********************************************************************/

    #region ������Ƽ
    /// <summary> ������ �̹����� ���� �ε��� </summary>
    public int CurIndex = 0;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    private void Start()
    {
        SetIndex();
        SetParent();
    }

    private void Awake()
    {
        
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private �Լ�
    private void SetIndex()
    {
        CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
    }

    private void SetParent ()
    {
        CurParent = this.transform.parent;
    }
    #endregion


    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region

    /// <summary> �θ� ���� </summary>
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

        SetIndex();
    }

    /// <summary> ��ġ�� �ٲ� �������� Inventory.Items ����Ʈ���� Swap </summary>
    /// <typeparam name="Item">�κ��丮 ����Ʈ�� �ҷ��� Ÿ��</typeparam>
    /// <param name="items">�ٲ� �������� ������ �ӽ� ���� �� ������ ����</param>
    /// <param name="Swap">�ٲ��� ���ϴ� ����� ����</param>
    public void SwapItem(List<Item> items, Transform Other)
    {
        //�ӽ� ���� �� ������ Ÿ�� ����
        Item temp;
        int OtherIndex = Other.GetComponentInChildren<SlotItem>().CurIndex;
        ItemData OtherItem = Other.GetComponentInParent<Slot>().ItemProperties;
        if (Other.GetComponent<Slot>().SlotState == OtherItem.ItemType || Other.GetComponent<Slot>().SlotState == ItemType.Any)
        {
            if (OtherItem != null)
            {
                temp = items[OtherIndex];
                items[OtherIndex] = items[CurIndex];
                items[CurIndex] = temp;
            }
            else
            {
                items[OtherIndex] = items[CurIndex];
                items[CurIndex] = null;
            }
        }
    }
    #endregion

    /***********************************************************************
    *                               Mouse Events
    ***********************************************************************/
    #region ���콺 �̺�Ʈ

    /// <summary> �巡�� ���� �� </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        CurParent = this.transform.parent;
        this.transform.SetParent(CurParent.parent);
        this.gameObject.GetComponent<Image>().raycastTarget = false;
    }

    /// <summary> �巡�� �� </summary>
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        IsOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    /// <summary> �巡�� ���� �� </summary>
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

            if (this.gameObject.GetComponentInParent<Slot>().ItemProperties.ItemPrefab != null)
            {
                Instantiate(this.gameObject.GetComponentInParent<Slot>().ItemProperties.ItemPrefab,
                    DropPos, Quaternion.identity);
            }

            int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;

            this.gameObject.GetComponentInParent<Inventory>().Items.RemoveAt(CurIndex);

            this.GetComponentInParent<Slot>().RemoveItem();

        }
    }
    #endregion
}