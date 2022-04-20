using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootArea : MonoBehaviour
{
    public List<GameObject> LootableItems = new List<GameObject>();

    private LayerMask ItemLayerMask;
    public LayerMask FurMask;
    /*-----------------------------------------------------------------------------------------------*/
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    /*-----------------------------------------------------------------------------------------------*/
    private void OnTriggerEnter(Collider other)
    {
        if ((ItemLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            LootableItems.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        LootableItems.Remove(other.gameObject);
    }
}
