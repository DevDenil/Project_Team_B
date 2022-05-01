using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootArea : MonoBehaviour
{
    /// <summary> UI를 불러오기 위한 변수  </summary>
    [SerializeField]
    Canvas myCanvas;
    /// <summary> 내 인벤토리UI 스크립트 변수 </summary>
    [SerializeField]
    Inventory myInventory;
    [SerializeField]
    List<Item> myItems;

    public List<GameObject> LootableItems = new List<GameObject>();

    public List<GameObject> LootableObject = new List<GameObject>();

    [SerializeField]
    private LayerMask ItemLayerMask;
    [SerializeField]
    private LayerMask LootableLayerMask;


    /// <summary> PickUp 팝업 UI </summary>
    PickUpUI myPickUpUI = null;
    GameObject InstPickupUI = null;

    /// <summary> Loot 팝업 UI </summary>
    PickUpUI myLootUI = null;
    GameObject InstLootUI = null;

    void Start()
    {
        myInventory = myCanvas.GetComponentInChildren<Inventory>();
        myItems = myInventory.items;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && LootableItems.Count != 0)
        {
            ItemData ItemData = LootableItems[0].GetComponent<ItemData>();
            myInventory.AddItem(ItemData);

            Destroy(LootableItems[0]);
            LootableItems.RemoveAt(0);
            Destroy(InstPickupUI);
            myInventory.GetComponent<Inventory>().Refresh();
        }

        if (Input.GetKeyDown(KeyCode.F) && LootableObject.Count != 0)
        {
            if (InstLootUI != null)
            { 
                Destroy(InstLootUI);
                InstLootUI = Instantiate(Resources.Load("UI/ItemTableUI"), GameObject.Find("Canvas").transform) as GameObject;
            }
        }
    }
    /*-----------------------------------------------------------------------------------------------*/
    private void OnTriggerEnter(Collider other)
    {
        if ((LootableLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            LootableObject.Add(other.gameObject);

            if (InstLootUI == null) InstLootUI = Instantiate(Resources.Load("UI/Popup_Loot"), GameObject.Find("Canvas").transform) as GameObject;
            myLootUI = InstLootUI.GetComponent<PickUpUI>();
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

    IEnumerator SearchingObject()
    {
        while (true)
        {

            yield return new WaitForSeconds(1.0f);
        }
    }
}