using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLootArea : MonoBehaviour
{
    /// <summary> UI를 불러오기 위한 변수  </summary>
    [SerializeField]
    Canvas myCanvas;
    /// <summary> 내 인벤토리UI 스크립트 변수 </summary>
    [SerializeField]
    Inventory myInventory;
    InventoryUI _inventoryUI;

    public List<GameObject> LootableItems = new List<GameObject>();

    public List<GameObject> LootableObject = new List<GameObject>();

    [SerializeField]
    private LayerMask ItemLayerMask;
    [SerializeField]
    private LayerMask LootableLayerMask;


    /// <summary> PickUp 팝업 UI </summary>
    PickUpUI myPickUpUI = null;
    GameObject InstPickupUI = null;
    [SerializeField]
    GameObject SearchingObj = null;

    /// <summary> Loot 팝업 UI </summary>
    PickUpUI myLootUI = null;
    GameObject InstLootUI = null;
    [SerializeField]
    GameObject ItemTable = null;

    /// <summary> 서칭 팝업 UI </summary>
    GameObject ObjectUI = null;

    void Start()
    {
        myInventory = myCanvas.GetComponentInChildren<Inventory>();
        _inventoryUI = myCanvas.GetComponentInChildren<InventoryUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && LootableItems.Count != 0)
        {

            ItemData item = null;
            if (LootableItems[0].GetComponent<WeaponItem>() is WeaponItem)
            {
                item = LootableItems[0].GetComponent<WeaponItem>().WeaponData;
            }
            else if (LootableItems[0].GetComponent<ArmorItem>() is ArmorItem)
            {
                item = LootableItems[0].GetComponent<ArmorItem>().ArmorData;
            }
            else if (LootableItems[0].GetComponent<PotionItem>() is PotionItem)
            {
                item = LootableItems[0].GetComponent<PotionItem>().PotionData;
            }

            int Index = myInventory.FindEmptySlotIndex(myInventory.Items, myInventory.Items.Count);
            if (Index != -1)
            {
                myInventory.Items[Index] = item;
                Destroy(LootableItems[0]);
                LootableItems.RemoveAt(0);
                Destroy(InstPickupUI);
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && LootableObject.Count != 0)
        {
            if (ItemTable == null)
            {
                if (InstLootUI != null) Destroy(InstLootUI);
                if (SearchingObj == null && LootableObject != null)
                    if (ObjectUI == null)
                    {
                        if (InstLootUI != null)
                        {
                            Destroy(InstLootUI);
                        }
                        if (SearchingObj == null && LootableObject != null)
                        {
                            SearchingObj = Instantiate(Resources.Load("UI/SearchingObj"), GameObject.Find("Canvas").transform) as GameObject;
                            StartCoroutine(SearchingObject(SearchingObj));
                        }
                    }
                    else
                    {
                        Destroy(ItemTable);
                        ItemTable = null;
                    }

                Destroy(ObjectUI);
                ObjectUI = null;
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

            if (other.GetComponent<Rigidbody>() != null)
            {
                if (InstPickupUI == null) InstPickupUI = Instantiate(Resources.Load("UI/Popup_Pickup"), GameObject.Find("Canvas").transform) as GameObject;
                myPickUpUI = InstPickupUI.GetComponent<PickUpUI>();
                myPickUpUI.Initialize(other.GetComponent<Transform>().transform, 50.0f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        LootableItems.Remove(other.gameObject);
        Destroy(InstPickupUI);
        LootableObject.Remove(other.gameObject);
        Destroy(InstLootUI);
        Destroy(SearchingObj);
        Destroy(ItemTable);
    }

    IEnumerator SearchingObject(GameObject obj)
    {
        /* 애니메이션 실행 줄 */
        while (obj.gameObject.GetComponent<Slider>().value < 1.0f)
        {
            obj.gameObject.GetComponent<Slider>().value += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        SearchingObj = null;
        ItemTable = Instantiate(Resources.Load("UI/ItemTableUI"), GameObject.Find("Canvas").transform) as GameObject;
        Destroy(ObjectUI);
    }

    //IEnumerator SearchingObject(GameObject UI)
    //{
    //    while (UI.gameObject.GetComponent<Slider>().value < 1.0f) 
    //    {
    //        UI.gameObject.GetComponent<Slider>().value += Time.deltaTime;

    //        yield return null;
    //    }
    //    Destroy(UI);
    //    SearchingObj = null;
    //    ObjectUI = Instantiate(Resources.Load("UI/ItemTableUi"), GameObject.Find("Canvas").transform) as GameObject;
    //    yield return null;
    //}
}