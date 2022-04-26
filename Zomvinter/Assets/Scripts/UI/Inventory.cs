using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary> 내 플레이어의 위치 </summary>
    public Transform myPlayerPos;
    /// <summary> 인벤토리에 저장 될 아이템 목록 리스트 </summary>
    public List<Item> items;

    /// <summary> 인벤토리 내의 슬롯 프리펩 위치 값 </summary>
    [SerializeField]
    private Transform slotParent;
    /// <summary> 인벤토리 내의 슬롯들을 저장 할 배열 </summary>
    [SerializeField]
    public Slot[] slots;

    /// <summary> Editor 상에서 Slot 자식 오브젝트를 불러와서 slots 배열에 지정 </summary>
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
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
            slots[i].item = items[i];
        }
        for (; i< slots.Length; i++)
        {
            slots[i].item = null;
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
