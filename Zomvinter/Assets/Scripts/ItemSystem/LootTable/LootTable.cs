using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootTable : MonoBehaviour
{
    // 드랍리스트 
    [SerializeField]
    private Loot[] loot;

    // 드랍된 아이템 리스트( 확률 계산 후의 결과값)
    private List<Item> droppedItems = new List<Item>();

    // 드랍확률 계산을 실행했는지 여부 
    private bool rolled = false;

    public void ShowLoot()
    {
        if (!rolled)
        {
            // 드랍될 아이템 확률 계산
            RollLoot();
        }

        // LootWindow 에 드랍 아이템들을 표시한다.
        //LootWindow.MyInstance.CreatePages(droppedItems);
    }

    // 확률 계산
    private void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            // 드랍확률이 roll 보다 같거나 크면 해당 아이템 드랍
            if (roll <= item.MyDropChance)
            {
                // 드랍리스트에 추가
                droppedItems.Add(item.MyItem);
            }
        }

        rolled = true;
    }

}