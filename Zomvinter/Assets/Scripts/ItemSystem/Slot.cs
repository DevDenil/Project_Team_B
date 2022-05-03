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


    [SerializeField]
    public ItemType SlotState;

    private void Start()
    {
        myInventry = this.GetComponentInParent<Inventory>();
    }

    /// <summary> OnDrop �̺�Ʈ�� �߻����� �� ���� �� �̺�Ʈ </summary>
    /// <param name="eventData">OnDrop�� �߻� �Ǵ� ������ ������Ʈ</param>
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        Slot itemSlot = eventData.pointerDrag.GetComponentInParent<Slot>();
        Debug.Log(item.CurIndex);
        if (item != null)
        {
            if (itemSlot.SlotState == ItemType.Primary)
            {
                item.SwapItem(myInventry.PrimaryItems, this.transform);
            }
            else if (itemSlot.SlotState == ItemType.Secondary)
            {
                item.SwapItem(myInventry.SecondaryItems, this.transform);
            }
            else if (itemSlot.SlotState == ItemType.Expand)
            {
                item.SwapItem(myInventry.ConsumableItems, this.transform);
            }
            else if(itemSlot.SlotState == ItemType.Any)
            {
                item.SwapItem(myInventry.items, this.transform);
            }
            item.ChangeParent(this.transform);
            //item.SetIndex();
        }
        this.GetComponentInChildren<SlotItem>().CurIndex = SlotIndex;
        myInventry.RefreshList();
    }
}
