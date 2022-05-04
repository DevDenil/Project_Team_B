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
    /// <summary> ConsumableData로부터 가져온 정보를 Data에 저장 </summary>
    public ConsumableData Data { get; private set; }

    /// <summary> 현재 아이템 개수 </summary>
    public int Amount { get; protected set; }

    /// <summary> 하나의 슬롯이 가질 수 있는 최대 개수(기본 99) </summary>
    public int MaxAmount => Data.MaxAmount;

    /// <summary> 수량이 가득 찼는지 여부 </summary>
    public bool IsMax => Amount >= Data.MaxAmount;
    
    /// <summary> 개수가 없는지 여부 </summary>
    public bool IsEmpty => Amount <= 0;

    /// <summary> 개수 지정(범위 제한) </summary>
    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }

    /// <summary> 개수 추가 및 최대치 초과량 반환(초과량 없을 경우 0) </summary>
    public int AddAmountAndGetExcess(int amount)
    {
        //현재 갯수에 추가 갯수를 더한 값을 저장
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);

        // 저장 된 값이 최대 갯수보다 큰 경우 나머지를 반환
        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }

    private Consumable Clone(int amount)
    {
        return new Consumable(Data, amount);
    }

}
