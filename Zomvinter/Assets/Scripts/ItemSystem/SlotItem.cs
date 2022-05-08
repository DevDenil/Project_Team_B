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
    
    /// <summary> 아이템 이미지의 부모 슬롯 </summary>
    [SerializeField] private Transform CurParent = null;
    #endregion
    /***********************************************************************
    *                               Properties
    ***********************************************************************/

    #region 프로퍼티
    /// <summary> 아이템 이미지가 가진 인덱스 </summary>
    public int CurIndex = 0;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
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
    #region Private 함수
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

    /// <summary> 부모 변경 </summary>
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

    /// <summary> 위치가 바뀐 아이템을 Inventory.Items 리스트에서 Swap </summary>
    /// <typeparam name="Item">인벤토리 리스트를 불러올 타입</typeparam>
    /// <param name="items">바뀔 아이템의 정보가 임시 저장 될 아이템 변수</param>
    /// <param name="Swap">바뀜을 당하는 대상의 정보</param>
    public void SwapItem(List<Item> items, Transform Other)
    {
        //임시 저장 될 아이템 타입 변수
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
    #region 마우스 이벤트

    /// <summary> 드래그 시작 시 </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        CurParent = this.transform.parent;
        this.transform.SetParent(CurParent.parent);
        this.gameObject.GetComponent<Image>().raycastTarget = false;
    }

    /// <summary> 드래그 중 </summary>
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        IsOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    /// <summary> 드래그 끝날 시 </summary>
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