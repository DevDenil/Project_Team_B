using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items; // 아이템 형식 저장공간
    public int inventoryMax = 10;

    [SerializeField]
    private GameObject InventoryUI;
    bool active = false;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private ItemSlot[] slots;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
    }

    public void AddItem(Item _item)
    {
        if(items.Count < slots.Length)
        {
            items.Add(_item);
            RefreshSlots();
        }
        else
        {
            //아이템 꽉참 팝업 UI 호출
            Debug.Log("인벤토리 꽉참");
        }
    }

    void Awake()
    {
        RefreshSlots(); 
    }

    public void RefreshSlots()
    {
        int i = 0;
        for(; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for(; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    void Start()
    {
        InventoryUI.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            active = !active;
            InventoryUI.SetActive(active);
        }
    }

    /*
    public void AddItem(Item item)
    {
        if (this.ItemCount() == inventoryMax) Debug.Log("인벤토리 꽉참"); //배열공간이 차있는지 확인
        else
        {
            items.Add(item);
            Debug.Log("아이템추가", item);
        }
    }
    */
    public int ItemCount()//배열 순서 
    {
        int cnt = 0;
        foreach (var item in items)
        {
            if (item != null) cnt++;
        }
        return cnt;
    }
    public void ItemList()
    {
        foreach (Item item in items)
        {
            if (item != null)
            {
                Debug.Log(item.name);
            }
        }
    }
}