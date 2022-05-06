using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ - ���� ������ </summary>
public class ConsumableItem : CountableItem
{
    public ConsumableItem(ConsumableItemData data, int amount = 1) : base(data, amount) { }

    public bool Use()
    {
        // �ӽ� : ���� �ϳ� ����
        Amount--;

        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new ConsumableItem(CountableData as ConsumableItemData, amount);
    }
}
