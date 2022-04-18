using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items; // ������ ���� �������
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
            //������ ���� �˾� UI ȣ��
            Debug.Log("�κ��丮 ����");
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
        if (this.ItemCount() == inventoryMax) Debug.Log("�κ��丮 ����"); //�迭������ ���ִ��� Ȯ��
        else
        {
            items.Add(item);
            Debug.Log("�������߰�", item);
        }
    }
    */
    public int ItemCount()//�迭 ���� 
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