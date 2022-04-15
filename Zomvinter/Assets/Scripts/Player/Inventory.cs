using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> items = new List<Item>(); // ������ ���� �������
    public int inventoryMax = 10;
    public GameObject InventoryUI;
    bool active = false;

    void Start()
    {
        InventoryUI.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            active = !active;
            InventoryUI.SetActive(active);
        }
    }
    public Inventory()
    {

    }
    public void AddItem(Item item)
    {
        if (this.ItemCount() == inventoryMax) Debug.Log("�κ��丮 ����"); //�迭������ ���ִ��� Ȯ��
        else
        {
            items.Add(item);
            Debug.Log("�������߰�", item);
        }
    }
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
            if(item !=null)
            {
                Debug.Log(item.name);
            }
        }
    }


}
