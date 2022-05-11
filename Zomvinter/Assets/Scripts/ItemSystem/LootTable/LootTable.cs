using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootTable : MonoBehaviour
{
    // �������Ʈ 
    [SerializeField]
    private Loot[] loot;

    // ����� ������ ����Ʈ( Ȯ�� ��� ���� �����)
    private List<Item> droppedItems = new List<Item>();

    // ���Ȯ�� ����� �����ߴ��� ���� 
    private bool rolled = false;

    public void ShowLoot()
    {
        if (!rolled)
        {
            // ����� ������ Ȯ�� ���
            RollLoot();
        }

        // LootWindow �� ��� �����۵��� ǥ���Ѵ�.
        //LootWindow.MyInstance.CreatePages(droppedItems);
    }

    // Ȯ�� ���
    private void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            // ���Ȯ���� roll ���� ���ų� ũ�� �ش� ������ ���
            if (roll <= item.MyDropChance)
            {
                // �������Ʈ�� �߰�
                droppedItems.Add(item.MyItem);
            }
        }

        rolled = true;
    }

}