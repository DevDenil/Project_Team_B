using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootArea : MonoBehaviour
{
    [SerializeField]
    Canvas myCanvas;
    [SerializeField]
    GameObject myInventory;

    public List<GameObject> LootableItems = new List<GameObject>();

    public List<GameObject> LootableObject = new List<GameObject>();

    [SerializeField]
    private LayerMask ItemLayerMask;
    [SerializeField]
    private LayerMask LootableLayerMask;

    List<Item> lootItems;

    /// <summary> PickUp �˾� UI </summary>
    PickUpUI myPickUpUI = null;
    PickUpUI myLootUI = null;
    GameObject InstPickupUI = null;

    GameObject InstLootUI = null;

    void Start()
    {
        lootItems = myCanvas.GetComponentInChildren<Inventory>().items;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && LootableItems.Count != 0)
        {
            lootItems.Add(LootableItems[0].GetComponent<ItemData>().myProperties);
            Destroy(LootableItems[0]);
            LootableItems.RemoveAt(0);
            Destroy(InstPickupUI);
            myInventory.GetComponent<Inventory>().RefreshSlot();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (InstLootUI == null && LootableObject != null) InstLootUI = Instantiate(Resources.Load("UI/ItemTableUI"), GameObject.Find("Canvas").transform) as GameObject;
        }
                
    }
    /*-----------------------------------------------------------------------------------------------*/
    private void OnTriggerEnter(Collider other)
    {
        if ((LootableLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            LootableObject.Add(other.gameObject);
            if (InstPickupUI == null) InstPickupUI = Instantiate(Resources.Load("UI/Popup_Loot"), GameObject.Find("Canvas").transform) as GameObject;
            myLootUI = InstPickupUI.GetComponent<PickUpUI>();
            myLootUI.Initialize(other.GetComponent<Transform>().transform, 50.0f);
        }
        if ((ItemLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            LootableItems.Add(other.gameObject);

            if(InstPickupUI == null) InstPickupUI = Instantiate(Resources.Load("UI/Popup_Pickup"), GameObject.Find("Canvas").transform) as GameObject;
            myPickUpUI = InstPickupUI.GetComponent<PickUpUI>();
            myPickUpUI.Initialize(other.GetComponent<Transform>().transform, 50.0f);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        LootableItems.Remove(other.gameObject);
        Destroy(InstPickupUI);
        LootableObject.Remove(other.gameObject);
        Destroy(InstLootUI);
    }
}
