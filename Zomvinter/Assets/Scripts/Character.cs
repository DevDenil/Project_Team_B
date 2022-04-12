using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct MonsterData
{
    public float MoveSpeed;
    public float TurnSpeed;
    public float AttRange;
    public float AttDelay;
    public float AttSpeed;
}

[Serializable]
public struct CharacterStat
{
    public float HP; //체력 Health Point
    public float AP; //방어력 Armor Point
    public float DP; //공격력 Damage Point
    public float Hunger; // 허기
    public float Thirsty; // 갈증
}
public class Character : MonoBehaviour
{

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
