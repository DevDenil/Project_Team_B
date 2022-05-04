using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public Consumable(ConsumableData data, int amount = 1) : base(data)
    {
        Data = data;
        SetAmount(amount);
    }
    /// <summary> ConsumableData�κ��� ������ ������ Data�� ���� </summary>
    public ConsumableData Data { get; private set; }

    /// <summary> ���� ������ ���� </summary>
    public int Amount { get; protected set; }

    /// <summary> �ϳ��� ������ ���� �� �ִ� �ִ� ����(�⺻ 99) </summary>
    public int MaxAmount => Data.MaxAmount;

    /// <summary> ������ ���� á���� ���� </summary>
    public bool IsMax => Amount >= Data.MaxAmount;
    
    /// <summary> ������ ������ ���� </summary>
    public bool IsEmpty => Amount <= 0;

    /// <summary> ���� ����(���� ����) </summary>
    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }

    /// <summary> ���� �߰� �� �ִ�ġ �ʰ��� ��ȯ(�ʰ��� ���� ��� 0) </summary>
    public int AddAmountAndGetExcess(int amount)
    {
        //���� ������ �߰� ������ ���� ���� ����
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);

        // ���� �� ���� �ִ� �������� ū ��� �������� ��ȯ
        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }

    private Consumable Clone(int amount)
    {
        return new Consumable(Data, amount);
    }

}
