using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootArea : MonoBehaviour
{
    public List<GameObject> LootableItems = new List<GameObject>();
    [SerializeField]
    Canvas myCanvas;
    [SerializeField]
    GameObject myInventory;

<<<<<<< HEAD
    private LayerMask ItemLayerMask;
    public LayerMask FurMask;
=======
    List<Item> lootItems;

    public LayerMask ItemLayerMask;

    /// <summary> PickUp ÆË¾÷ UI </summary>
    PickUpUI myPickUpUI = null;
    GameObject InstPickupUI = null;
>>>>>>> 8286d2bede006c5ca82d2177d6e990d98d35b8e1
    /*-----------------------------------------------------------------------------------------------*/
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
    }
    /*-----------------------------------------------------------------------------------------------*/
    private void OnTriggerEnter(Collider other)
    {
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
    }
}
