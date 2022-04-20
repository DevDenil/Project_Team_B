using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    Image image;
    [SerializeField]
    TMPro.TMP_Text text;
    public int SlotIndex = 0;

    [SerializeField]
    private Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if(_item != null)
            {
                image.sprite = item._itemImage;
                image.color = new Color(1, 1, 1, 1);
                text.text = "";

            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        if (item != null)
        {
            item.ChangeParent(this.transform);
        }
    }
}
