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
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        if (item != null)
        {
            item.ChangeIndex(this.GetComponentInParent<Inventory>().items, this.transform);
            item.ChangeParent(this.transform);
            item.SetIndex();
            
            

        }
    }
}
