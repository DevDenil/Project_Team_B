using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary> 내 플레이어의 위치 </summary>
    public Transform myPlayerPos;
    /// <summary> 인벤토리에 저장 될 아이템 목록 리스트 </summary>
    public List<Item> items;
    /// <summary> 주 무기에 저장 될 아이템 목록 리스트 </summary>
    public Item[] MainItems = new Item[2];
    /// <summary> 모조 무기에 저장 될 아이템 목록 리스트 </summary>
    public Item[] SecondaryItems = new Item[2];
    /// <summary> 소모품에 저장 될 아이템 목록 리스트 </summary>
    public Item[] ConsumableItems = new Item[3];
    /// <summary> 헬멧 아이템 변수 </summary>
    public Item HelmetItem = null;
    /// <summary> 방어구 아이템 변수 </summary>
    public Item BodyArmorItem = null;
    /// <summary> 가방 아이템 변수 </summary>
    public Item BackpackItem = null;

    /// <summary> 인벤토리 내의 슬롯들을 모아놓는 Contents의 위치 값 </summary>
    [SerializeField]
    private Transform slotParent;
    /// <summary> 인벤토리 내의 슬롯들을 저장 할 배열 </summary>
    [SerializeField]
    public Slot[] slots;

    /// <summary> 에디터 상에서 실행 되는 함수 </summary>
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>(); // Slot의 자식 오브젝트를 불러와서 slots 배열에 지정
        SetIndexOnSlot();
    }

    /// <summary> 프로세스가 시작되기 전에 실행되는 함수 </summary>
    private void Awake()
    {
        RefreshSlot();
    }

    /// <summary> 슬롯에 인덱스 번호를 부여하는 함수 </summary>
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
            slots[i].GetComponentInChildren<SlotItem>().item = items[i];
        }
        for (; i< slots.Length; i++)
        {
            slots[i].GetComponentInChildren<SlotItem>().item = null;
        }
    }
    public void ItemTypeChecker(Item _item)
    {
        if (_item._type == Item.ItemType.Main)
        {
            if (MainItems[0] == null)
            {
                MainItems[0] = _item;
            }
            else
            {
                if(MainItems[1] == null)
                {
                    MainItems[1] = _item;
                }
                else
                {
                    items.Add(_item); 
                }
            }
        }
        else if (_item._type == Item.ItemType.Second)
        {
            if (SecondaryItems[0] == null)
            {
                SecondaryItems[0] = _item;
            }
            else
            {
                if (SecondaryItems[1] == null)
                {
                    SecondaryItems[1] = _item;
                }
                else
                {
                    items.Add(_item);
                }
            }
        }
        else if (_item._type == Item.ItemType.Consumable)
        {
            if (ConsumableItems[0] == null)
            {
                ConsumableItems[0] = _item;
            }
            else
            {
                if (ConsumableItems[1] == null)
                {
                    ConsumableItems[1] = _item;
                }
                else
                {
                    items.Add(_item);
                }
            }
        }
        else if (_item._type == Item.ItemType.Helmet)
        {
            HelmetItem = _item;
        }
        else if (_item._type == Item.ItemType.BodyArmor)
        {
            BodyArmorItem = _item;
        }
        else if (_item._type == Item.ItemType.Backpack)
        {
            BackpackItem = _item;
        }
        else
        {
            items.Add(_item);
        }
    }
    /// <summary> 아이템 추가 함수 </summary>
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
