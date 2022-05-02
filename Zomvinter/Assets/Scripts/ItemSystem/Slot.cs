using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    /// <summary> ������ �ε��� ��ȣ </summary>
    public int SlotIndex = 0;

    /// <summary> Inventory.cs ������ ���� </summary>
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

    /// <summary> OnDrop �̺�Ʈ�� �߻����� �� ���� �� �̺�Ʈ </summary>
    /// <param name="eventData">OnDrop�� �߻� �Ǵ� ������ ������Ʈ</param>
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
