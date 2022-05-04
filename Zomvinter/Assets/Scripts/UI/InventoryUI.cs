using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    /* ���� �� ���
        [��� - ������ ����]
        - ���� ���� �� �������� ������ ���� �̸�����(����, ũ�� �̸����� ����)

        [��� - ���� �������̽�]
        - ���Կ� ���콺 �ø���
          - ��� ���� ���� : ���̶���Ʈ �̹��� ǥ��
          - ������ ���� ���� : ������ ���� ���� ǥ��

        - �巡�� �� ���
          - ������ ���� ���� -> ������ ���� ���� : �� ������ ��ġ ��ȯ
          - ������ ���� ���� -> ������ ������ ���� : ������ ��ġ ����
            - Shift �Ǵ� Ctrl ���� ������ ��� : �� �� �ִ� ������ ���� ������
          - ������ ���� ���� -> UI �ٱ� : ������ ������

        - ���� ��Ŭ��
          - ��� ������ �������� ��� : ������ ���

        - ��� ��ư(���� ���)
          - Trim : �տ������� �� ĭ ���� ������ ä���
          - Sort : ������ ����ġ��� ������ ����

        - ���� ��ư(���� ���)
          - [A] : ��� ������ ���͸�
          - [E] : ��� ������ ���͸�
          - [P] : �Һ� ������ ���͸�

          * ���͸����� ���ܵ� ������ ���Ե��� ���� �Ұ�

        [��� - ��Ÿ]
        - InvertMouse(bool) : ���콺 ��Ŭ��/��Ŭ�� ���� ���� ����

        ��¥ : 2022-05-04
    */


    /// <summary> ���Կ� ������ ��� </summary>





    /***********************************************************************
    *                               Option Field
    ***********************************************************************/
    #region

    #endregion

    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region
    /// <summary> ����� �κ��丮 </summary>
    private Inventory _inventory;

    /// <summary> Slot�� ��Ƴ��� ������Ʈ </summary>
    private Transform ItemBag;
    /// <summary> Slot�� ��Ƴ��� ������Ʈ </summary>
    private Transform PrimaryBag;
    /// <summary> Slot�� ��Ƴ��� ������Ʈ </summary>
    private Transform SecondaryBag;
    /// <summary> Slot�� ��Ƴ��� ������Ʈ </summary>
    private Transform ConsumableBag;
    /// <summary> Slot�� ��Ƴ��� ������Ʈ </summary>
    private Transform EquipmentBag;


    private enum FilterOption
    {
        Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
    }
    private FilterOption _currentFilterOption = FilterOption.Any;

    #endregion

    

    /***********************************************************************
    *                               Public Fields
    ***********************************************************************/
    #region
    /// <summary> Slot���� ���� �� ����Ʈ </summary>
    public List<Slot> ItemSlots = new List<Slot>();

    /// <summary> Slot���� ���� �� ����Ʈ </summary>
    public List<Slot> PirmarySlots = new List<Slot>();

    /// <summary> Slot���� ���� �� ����Ʈ </summary>
    public List<Slot> ConsumableSlots = new List<Slot>();

    /// <summary> Slot�� ���� �� ���� </summary>
    public Slot SecondarySlots;

    /// <summary> Slot�� ���� �� ���� </summary>
    public Slot HelmetSlot;

    /// <summary> Slot�� ���� �� ���� </summary>
    public Slot BodyArmorSlot;

    /// <summary> Slot�� ���� �� ���� </summary>
    public Slot BackpackSlot;
    #endregion
    
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region
    private void Awake()
    {
        FindBag(out ItemBag, "BagBackpack");
        FindBag(out PrimaryBag, "BagPrimary");
        FindBag(out SecondaryBag, "BagSecondary");
        FindBag(out ConsumableBag, "BagExpand");
        FindBag(out EquipmentBag, "EquipmentBag");
    }
    private void Update()
    {

    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> ���Ե��� ��� �ִ� ������ ã�� �Լ� </summary>
    /// <param name="Bag">������ ������ ����</param>
    /// <param name="name">������ �̸�</param>
    void FindBag(out Transform Bag, string name)
    {
        Bag = this.transform.Find(name);
    }
    #endregion


    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region
    /// <summary> �κ��丮 ���� ��� (Inventory.cs���� ���� ȣ��) </summary>
    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }

    /// <summary> ���Կ� ������ ������ ��� </summary>
    public void SetItemIcon(List<Slot> _slotUIList, int index, Sprite icon)
    {
        //EditorLog($"Set Item Icon : Slot [{index}]");

        _slotUIList[index].SetItem(icon);
    }
    /// <summary> �ش� ������ ������ ���� �ؽ�Ʈ ���� </summary>
    public void SetItemAmountText(List<Slot> _slotUIList, int index, int amount)
    {
        //EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // NOTE : amount�� 1 ������ ��� �ؽ�Ʈ ��ǥ��
        _slotUIList[index].SetItemAmount(amount);
    }

    public void RemoveItem(List<Slot> _slotUIList, int index)
    {
        //EditorLog($"Remove Item : Slot [{index}]");

        _slotUIList[index].SlotItem.RemoveItem();
    }
    /// <summary> �ش� ������ ������ ���� �ؽ�Ʈ ���� </summary>
    public void HideItemAmountText(List<Slot> _slotUIList, int index)
    {
        //EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    /// <summary> ���� ������ ���� ���� ���� </summary>
    public void SetAccessibleSlotRange(int accessibleSlotCount, List<Slot>_slotUIList)
    {
        for (int i = 0; i < _slotUIList.Count; i++)
        {
            _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }

    /// <summary> Ư�� ������ ���� ���� ������Ʈ </summary>
    public void UpdateSlotFilterState(List<Slot> _slotUIList, int index, ItemData itemData)
    {
        bool isFiltered = true;

        // null�� ������ Ÿ�� �˻� ���� ���� Ȱ��ȭ
        if (itemData != null)
            switch (_currentFilterOption)
            {
                case FilterOption.Primary:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Secondary:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Helmet:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Bodyarmor:
                    isFiltered = (itemData is EquipmentData);
                    break;
                case FilterOption.Backpack:
                    isFiltered = (itemData is EquipmentData);
                    break;

                case FilterOption.Expand:
                    isFiltered = (itemData is ConsumableData);
                    break;
            }

        _slotUIList[index].SetItemAccessibleState(isFiltered);
    }
    #endregion
}
