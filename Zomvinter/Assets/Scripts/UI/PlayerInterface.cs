using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    public Inventory _inventory;

    [SerializeField]
    private Transform PrimaryBag;
    [SerializeField]
    private Transform SecondaryBag;
    [SerializeField]
    private Transform ExpandBag;

    [SerializeField]
    public VisualizeSlot Primary;

    [SerializeField]
    public VisualizeSlot Seconcdary;

    [SerializeField]
    public VisualizeSlot[] Expands;

    private void OnValidate()
    {
        _canvas = GetComponentInParent<Canvas>();
        _inventory = _canvas.GetComponentInChildren<Inventory>();
        Primary = PrimaryBag.GetComponentInChildren<VisualizeSlot>();
        Seconcdary = SecondaryBag.GetComponentInChildren<VisualizeSlot>();
        Expands = ExpandBag.GetComponentsInChildren<VisualizeSlot>();
    }

    void Start()
    {

    }

    void Update()
    {
        UpdateSlot();
    }
    private void UpdateSlot()
    {
        if (_inventory.PrimaryItems[0] != null)
        {
            Primary.Data = _inventory.PrimaryItems[0];
        }
        else
        {
            Primary.Data = null;
        }
        if (_inventory.SecondaryItems != null)
        {
            Seconcdary.Data = _inventory.SecondaryItems;
        }
        else
        {
            Seconcdary.Data = null;
        }
        for (int i = 0; i < _inventory.ConsumableItems.Count; i++)
        {
            if (_inventory.ConsumableItems[i] != null)
            {
                Expands[i].Data = _inventory.ConsumableItems[i];
            }
            else
            {
                Expands[i].Data = null;
            }
        }
    }
}
