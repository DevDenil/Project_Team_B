using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> items = new List<Item>(); // 아이템 형식 저장공간
    public int inventoryMax = 10;

    public Inventory()
    {

    }
    public void AddItem(Item item)
    {
        if (this.ItemCount() == inventoryMax) Debug.Log("인벤토리 꽉참"); //배열공간이 차있는지 확인
        else
        {
            items.Add(item);
            Debug.Log("아이템추가", item);
        }
    }
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
            if(item !=null)
            {
                Debug.Log(item.name);
            }
        }
    }


}
