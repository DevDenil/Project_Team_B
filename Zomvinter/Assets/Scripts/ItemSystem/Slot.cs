using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    //[SerializeField]
    //public Image image;
    //[SerializeField]
    //public TMPro.TMP_Text text;
    public int SlotIndex = 0;

    Inventory myInventry;

    private void Start()
    {
        myInventry = this.GetComponentInParent<Inventory>();
    }
    //[SerializeField]
    //Item _item;
    //public Item item
    //{
    //    get { return _item; }
    //    set
    //    {
    //        _item = value;
    //        if(_item != null)
    //        {
    //            //image.sprite = item._itemImage;
    //            //image.color = new Color(1, 1, 1, 1);
    //            text.text = item._maxAmount.ToString();

    //        }
    //        else
    //        {
    //            //image.color = new Color(1, 1, 1, 0);
    //        }
    //    }
    //}

    /// <summary> OnDrop 이벤트가 발생했을 때 실행 될 이벤트 </summary>
    /// <param name="eventData">OnDrop이 발생 되는 지점의 오브젝트</param>
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        if (item != null)
        {
            item.ChangeIndex(myInventry.items, this.transform);
            item.ChangeParent(this.transform);
            //item.SetIndex();
        }
        this.GetComponentInChildren<SlotItem>().CurIndex = SlotIndex;
    }
}
