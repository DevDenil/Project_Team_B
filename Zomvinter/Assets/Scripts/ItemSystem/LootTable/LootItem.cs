using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//아이템 속성값 저장하는곳 
public enum TYPE
{
    Equipment, Potion, Ammo, Etc
}
public class LootItem
{
    //값을 루트 테이블에서 사용함 
    public string ItemName;
    public Sprite ItemImage;
    public TYPE ItemType;
    public float ItemValue;
    public bool FindEmptySlot(LootItem[] _itemList)
    {
        bool isFull = true;
        for (int i = 0; i < _itemList.Length; i++)
        {
            if (_itemList[i] == null)
            {
                return false;
            }
        }
        return isFull;
    }
    public int FindEmptySlotIndex(LootItem[] _itemList, int StartIndex = 0)
    {
        for (int i = StartIndex; i < _itemList.Length; i++)
        {
            if (_itemList[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
    public LootItem Ammo()
    {
        LootItem Ammo = new LootItem();
        Ammo.ItemName = "7.62mm";
        Ammo.ItemType = TYPE.Ammo;
        Ammo.ItemImage = Resources.Load<Sprite>("Ammo");
        Ammo.ItemValue = 30.0f;

        return Ammo;
    }
    public LootItem AKM()
    {
        LootItem AKM = new LootItem();
        AKM.ItemName = "AKM";
        AKM.ItemType = TYPE.Equipment;
        AKM.ItemImage = Resources.Load<Sprite>("AKM");
        AKM.ItemValue = 50.0f;

        return AKM;
    }

    public LootItem WeaponItem()
    {
        LootItem WeaponData = new LootItem();
        WeaponData.ItemName = "Sword";
        WeaponData.ItemType = TYPE.Equipment;
        WeaponData.ItemImage = Resources.Load<Sprite>("Sword");
        WeaponData.ItemValue = 30.0f;

        return WeaponData;
    }

    public LootItem WeaponItem2()
    {
        LootItem WeaponData = new LootItem();
        WeaponData.ItemName = "Excalibur";
        WeaponData.ItemType = TYPE.Equipment;
        WeaponData.ItemImage = Resources.Load<Sprite>("Excalibur");
        WeaponData.ItemValue = 100.0f;

        return WeaponData;
    }
    public LootItem ShieldItem()
    {
        LootItem ShieldData = new LootItem();
        ShieldData.ItemName = "Shield";
        ShieldData.ItemType = TYPE.Equipment;
        ShieldData.ItemImage = Resources.Load<Sprite>("Shield");
        ShieldData.ItemValue = 50.0f;

        return ShieldData;
    }
    public LootItem ShieldItem2()
    {
        LootItem ShieldData = new LootItem();
        ShieldData.ItemName = "Knight's Shield";
        ShieldData.ItemType = TYPE.Equipment;
        ShieldData.ItemImage = Resources.Load<Sprite>("KnightShield");
        ShieldData.ItemValue = 80.0f;

        return ShieldData;
    }
    public LootItem ArmorItem()
    {
        LootItem ArmorData = new LootItem();
        ArmorData.ItemName = "Iron Armor";
        ArmorData.ItemType = TYPE.Equipment;
        ArmorData.ItemImage = Resources.Load<Sprite>("IronArmor");
        ArmorData.ItemValue = 60.0f;

        return ArmorData;
    }
    public LootItem ArmorItem2()
    {
        LootItem ArmorData = new LootItem();
        ArmorData.ItemName = "Hero's Armor";
        ArmorData.ItemType = TYPE.Equipment;
        ArmorData.ItemImage = Resources.Load<Sprite>("HeroArmor");
        ArmorData.ItemValue = 120.0f;

        return ArmorData;
    }
    public LootItem PantsItem()
    {
        LootItem PantsData = new LootItem();
        PantsData.ItemName = "Pants";
        PantsData.ItemType = TYPE.Equipment;
        PantsData.ItemImage = Resources.Load<Sprite>("Pants");
        PantsData.ItemValue = 40.0f;

        return PantsData;
    }
    public LootItem PantsItem2()
    {
        LootItem PantsData = new LootItem();
        PantsData.ItemName = "Plate Pants";
        PantsData.ItemType = TYPE.Equipment;
        PantsData.ItemImage = Resources.Load<Sprite>("PlatePants");
        PantsData.ItemValue = 80.0f;

        return PantsData;
    }
}
