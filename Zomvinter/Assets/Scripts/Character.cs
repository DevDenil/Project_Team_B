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
    public float HP; //ü�� Health Point
    public float AP; //���� Armor Point
    public float DP; //���ݷ� Damage Point
    public float Hunger; // ���
    public float Thirsty; // ����
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
