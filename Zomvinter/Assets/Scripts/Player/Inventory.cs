using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject InventoryUI;
    bool active = false;
    
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
}
