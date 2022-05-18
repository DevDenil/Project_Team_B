using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootTable : MonoBehaviour
{
    // 드랍리스트 
    [SerializeField]
    private Loot[] loot;
    [SerializeField]
    LootItem _lootItem;
    [SerializeField]
    // 아이템 배열
    public LootItem[] LootItems;

    // 드랍된 아이템 리스트( 확률 계산 후의 결과값)
    private List<LootItem> droppedItems = new List<LootItem>();

    // 드랍확률 계산을 실행했는지 여부 
    private bool rolled = false;

    //public void ShowLoot()
    //{
    //    if (!rolled)
    //    {
    //        // 드랍될 아이템 확률 계산
    //        RollLoot();
    //    }

    //    // LootWindow 에 드랍 아이템들을 표시한다.
    //    //LootWindow.MyInstance.CreatePages(droppedItems);
    //}

    //// 확률 계산
    //private void RollLoot()
    //{
    //    foreach (Loot item in loot)
    //    {
    //        int roll = Random.Range(0, 100);

    //        // 드랍확률이 roll 보다 같거나 크면 해당 아이템 드랍
    //        if (roll <= item.MyDropChance)
    //        {
    //            // 드랍리스트에 추가
    //            droppedItems.Add(item.MyItem);
    //        }
    //    }

    //    rolled = true;
    //}
    //  코드참고용 ----------------------------------------------------------------------
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
        //재민님 spawn item code 
        public void SpawnItem()
     {
        if (!_lootItem.FindEmptySlot(LootItems))//박스를 열때 or 박스가 스폰될때(start에) 로 변경
        {
            int RanNum = Random.Range(0, 10);//랜덤갯수 
            int Index = _lootItem.FindEmptySlotIndex(LootItems);//아이템의 인덱스값 
            if (RanNum == 0)
            {
                LootItem WeaponItem = new LootItem(); // Weapon 아이템 객체 생성 후 배열에 초기화
                WeaponItem = WeaponItem.WeaponItem();//객체 생성후 아이템 스크립트에서 값 전부 대입해줌 [상단첨부 ]
                LootItems[Index] = WeaponItem;//아이템 배열 [배열순번] = 객체
            }
            else if (RanNum == 1)
            {
                // Shield 아이템 객체 생성 후 배열에 초기화
                LootItem ShieldItem = new LootItem();
                ShieldItem = ShieldItem.ShieldItem();
                LootItems[Index] = ShieldItem;
            }
            else if (RanNum == 2)
            {
                // Armor 아이템 객체 생성 후 배열에 초기화
                LootItem ArmorItem = new LootItem();
                ArmorItem = ArmorItem.ArmorItem();
                LootItems[Index] = ArmorItem;
            }
            else if (RanNum == 3)
            {
                // Pants 아이템 객체 생성 후 배열에 초기화
                LootItem PantsItem = new LootItem();
                PantsItem = PantsItem.PantsItem();
                LootItems[Index] = PantsItem;
            }
            else if (RanNum == 4)
            {
                // Potion 아이템 객체 생성 후 배열에 초기화
                LootItem Ammo = new LootItem();
                Ammo = Ammo.Ammo();
                LootItems[Index] = Ammo;
            }
            else if (RanNum == 5)
            {
                // Weapon 아이템 객체 생성 후 배열에 초기화
                LootItem WeaponItem = new LootItem();
                WeaponItem = WeaponItem.WeaponItem2();
                LootItems[Index] = WeaponItem;
            }
            else if (RanNum == 6)
            {
                // Shield 아이템 객체 생성 후 배열에 초기화
                LootItem ShieldItem = new LootItem();
                ShieldItem = ShieldItem.ShieldItem2();
                LootItems[Index] = ShieldItem;
            }
            else if (RanNum == 7)
            {
                // Armor 아이템 객체 생성 후 배열에 초기화
                LootItem ArmorItem = new LootItem();
                ArmorItem = ArmorItem.ArmorItem2();
                LootItems[Index] = ArmorItem;
            }
            else if (RanNum == 8)
            {
                // Pants 아이템 객체 생성 후 배열에 초기화
                LootItem PantsItem = new LootItem();
                PantsItem = PantsItem.PantsItem2();
                LootItems[Index] = PantsItem;
            }
            else if (RanNum == 9)
            {
                // Potion 아이템 객체 생성 후 배열에 초기화
                LootItem AKM = new LootItem();
                AKM = AKM.AKM();
                LootItems[Index] = AKM;
            }
        }
    }
}