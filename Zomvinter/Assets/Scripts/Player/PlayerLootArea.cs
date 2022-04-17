using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootArea : MonoBehaviour
{
    public List<GameObject> LootableItems = new List<GameObject>();

    public LayerMask ItemLayerMask;

    PickUpUI myPickUpUI = null;

    GameObject InstPickupUI = null;
    /*-----------------------------------------------------------------------------------------------*/
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && LootableItems.Count != 0)
        {
            this.GetComponentInParent<Player>().AddItems(LootableItems[0]);
            Destroy(LootableItems[0]);
            LootableItems.RemoveAt(0);
            Destroy(InstPickupUI);
        }
    }
    /*-----------------------------------------------------------------------------------------------*/
    private void OnTriggerEnter(Collider other)
    {
        if ((ItemLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            LootableItems.Add(other.gameObject);

            if(InstPickupUI == null) InstPickupUI = Instantiate(Resources.Load("UI/UI_Popup_Pickup"), GameObject.Find("Canvas").transform) as GameObject;
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
