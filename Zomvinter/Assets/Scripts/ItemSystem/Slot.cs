using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    /// <summary> 슬롯의 인덱스 번호 </summary>
    public int SlotIndex = 0;

    /// <summary> Inventory.cs 참조용 변수 </summary>
    Inventory myInventry;

    public enum STATE 
    { 
        Primary, Secondary, Expand, Inventory
    }
    [SerializeField]
    public STATE SlotState;

    private void Start()
    {
        myInventry = this.GetComponentInParent<Inventory>();
    }

    /// <summary> OnDrop 이벤트가 발생했을 때 실행 될 이벤트 </summary>
    /// <param name="eventData">OnDrop이 발생 되는 지점의 오브젝트</param>
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        if (item != null)
        {
            item.SwapItem(myInventry.items, this.transform);
            item.ChangeParent(this.transform);
            //item.SetIndex();
        }
        this.GetComponentInChildren<SlotItem>().CurIndex = SlotIndex;
        myInventry.Refresh();
    }
}
