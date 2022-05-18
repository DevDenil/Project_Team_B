using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootTable : MonoBehaviour
{
    // �������Ʈ 
    [SerializeField]
    private Loot[] loot;
    [SerializeField]
    LootItem _lootItem;
    [SerializeField]
    // ������ �迭
    public LootItem[] LootItems;

    // ����� ������ ����Ʈ( Ȯ�� ��� ���� �����)
    private List<LootItem> droppedItems = new List<LootItem>();

    // ���Ȯ�� ����� �����ߴ��� ���� 
    private bool rolled = false;

    //public void ShowLoot()
    //{
    //    if (!rolled)
    //    {
    //        // ����� ������ Ȯ�� ���
    //        RollLoot();
    //    }

    //    // LootWindow �� ��� �����۵��� ǥ���Ѵ�.
    //    //LootWindow.MyInstance.CreatePages(droppedItems);
    //}

    //// Ȯ�� ���
    //private void RollLoot()
    //{
    //    foreach (Loot item in loot)
    //    {
    //        int roll = Random.Range(0, 100);

    //        // ���Ȯ���� roll ���� ���ų� ũ�� �ش� ������ ���
    //        if (roll <= item.MyDropChance)
    //        {
    //            // �������Ʈ�� �߰�
    //            droppedItems.Add(item.MyItem);
    //        }
    //    }

    //    rolled = true;
    //}
    //  �ڵ������ ----------------------------------------------------------------------
    //public enum TYPE
    //{
    //    Equipment, Potion
    //}

    //public class Item
    //{
    //    public string ItemName;
    //    public Sprite ItemImage;
    //    public TYPE ItemType;
    //    public float ItemValue;

    //    public Item PotionItem()
    //    {
    //        Item PotionData = new Item();
    //        PotionData.ItemName = "Healing Potion";
    //        PotionData.ItemType = TYPE.Potion;
    //        PotionData.ItemImage = Resources.Load<Sprite>("Potion");
    //        PotionData.ItemValue = 10.0f;

    //        return PotionData;
    //    }
    //}
    //---------------------------------------------------------------------------------------------
        //��δ� spawn item code 
        public void SpawnItem()
     {
        if (!_lootItem.FindEmptySlot(LootItems))//�ڽ��� ���� or �ڽ��� �����ɶ�(start��) �� ����
        {
            int RanNum = Random.Range(0, 10);//�������� 
            int Index = _lootItem.FindEmptySlotIndex(LootItems);//�������� �ε����� 
            if (RanNum == 0)
            {
                LootItem WeaponItem = new LootItem(); // Weapon ������ ��ü ���� �� �迭�� �ʱ�ȭ
                WeaponItem = WeaponItem.WeaponItem();//��ü ������ ������ ��ũ��Ʈ���� �� ���� �������� [���÷�� ]
                LootItems[Index] = WeaponItem;//������ �迭 [�迭����] = ��ü
            }
            else if (RanNum == 1)
            {
                // Shield ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem ShieldItem = new LootItem();
                ShieldItem = ShieldItem.ShieldItem();
                LootItems[Index] = ShieldItem;
            }
            else if (RanNum == 2)
            {
                // Armor ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem ArmorItem = new LootItem();
                ArmorItem = ArmorItem.ArmorItem();
                LootItems[Index] = ArmorItem;
            }
            else if (RanNum == 3)
            {
                // Pants ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem PantsItem = new LootItem();
                PantsItem = PantsItem.PantsItem();
                LootItems[Index] = PantsItem;
            }
            else if (RanNum == 4)
            {
                // Potion ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem Ammo = new LootItem();
                Ammo = Ammo.Ammo();
                LootItems[Index] = Ammo;
            }
            else if (RanNum == 5)
            {
                // Weapon ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem WeaponItem = new LootItem();
                WeaponItem = WeaponItem.WeaponItem2();
                LootItems[Index] = WeaponItem;
            }
            else if (RanNum == 6)
            {
                // Shield ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem ShieldItem = new LootItem();
                ShieldItem = ShieldItem.ShieldItem2();
                LootItems[Index] = ShieldItem;
            }
            else if (RanNum == 7)
            {
                // Armor ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem ArmorItem = new LootItem();
                ArmorItem = ArmorItem.ArmorItem2();
                LootItems[Index] = ArmorItem;
            }
            else if (RanNum == 8)
            {
                // Pants ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem PantsItem = new LootItem();
                PantsItem = PantsItem.PantsItem2();
                LootItems[Index] = PantsItem;
            }
            else if (RanNum == 9)
            {
                // Potion ������ ��ü ���� �� �迭�� �ʱ�ȭ
                LootItem AKM = new LootItem();
                AKM = AKM.AKM();
                LootItems[Index] = AKM;
            }
        }
    }
}