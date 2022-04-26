using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary> �� �÷��̾��� ��ġ </summary>
    public Transform myPlayerPos;
    /// <summary> �κ��丮�� ���� �� ������ ��� ����Ʈ </summary>
    public List<Item> items;

    /// <summary> �κ��丮 ���� ���� ������ ��ġ �� </summary>
    [SerializeField]
    private Transform slotParent;
    /// <summary> �κ��丮 ���� ���Ե��� ���� �� �迭 </summary>
    [SerializeField]
    public Slot[] slots;

    /// <summary> Editor �󿡼� Slot �ڽ� ������Ʈ�� �ҷ��ͼ� slots �迭�� ���� </summary>
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        SetIndexOnSlot();
    }

    /// <summary> ���μ����� ���۵Ǳ� ���� ����Ǵ� �Լ� </summary>
    private void Awake()
    {
        RefreshSlot();
    }

    /// <summary> ���Կ� �ε��� ��ȣ�� �ο��ϴ� �Լ� </summary>
    void SetIndexOnSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SlotIndex = i;
        }
    }

    /// <summary> ������ ��� ���ΰ�ħ </summary>
    public void RefreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i< slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    /// <summary> ������ �߰� �Լ� </summary>
    public void AddItem(Item _itme)
    {
        if(items.Count < slots.Length)
        {
            items.Add(_itme);
            RefreshSlot();
        }
        else
        {
            //�κ��丮 �� �� ���
            Debug.Log("������ ���� �� �ֽ��ϴ�.");
        }
    }

}
