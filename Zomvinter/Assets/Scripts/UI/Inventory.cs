using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Transform myPlayerPos;

    public List<Item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    public Slot[] slots;

    /// <summary> Editor 상에서 Slot 자식 오브젝트를 불러와서 slots 배열에 지정 </summary>
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        SetIndexOnSlot();
    }

    private void Awake()
    {
        RefreshSlot();
    }
    void SetIndexOnSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SlotIndex = i;
        }
    }

    /// <summary> 아이템 목록 새로고침 </summary>
    public void RefreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i< slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _itme)
    {
        if(items.Count < slots.Length)
        {
            items.Add(_itme);
            RefreshSlot();
        }
        else
        {
            //인벤토리 꽉 찬 경우
            Debug.Log("슬롯이 가득 차 있습니다.");
        }
    }

}
