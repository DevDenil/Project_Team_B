using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 몬스터 데이터 구조체 </summary>
[Serializable]
struct MonsterData
{
    public float MoveSpeed; // 캐릭터 데이터 구조체로 대체 예정
    public float TurnSpeed; // 캐릭터 데이터 구조체로 대체 예정
    public float AttRange;
    public float AttDelay;
    public float AttSpeed;
    public float UnChaseTime;
}

/// <summary> 캐릭터 데이터 구조체</summary>
[Serializable]
public struct CharacterStat
{
    public float HP; //체력 Health Point
    public float AP; //방어력 Armor Point
    public float DP; //공격력 Damage Point
    public float MoveSpeed;
    public float TurnSpeed;
    public float Hunger; // 허기
    public float Thirsty; // 갈증
}

public class Character : MonoBehaviour
{
    /// <summary> Animator 컴포넌트 반환 </summary>
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }
            return _anim;
        }
    }

}
