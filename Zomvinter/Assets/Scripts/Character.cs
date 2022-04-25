using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ ����ü </summary>
[Serializable]
struct MonsterData
{
    public float MoveSpeed; // ĳ���� ������ ����ü�� ��ü ����
    public float TurnSpeed; // ĳ���� ������ ����ü�� ��ü ����
    public float AttRange;
    public float AttDelay;
    public float AttSpeed;
    public float UnChaseTime;
}

/// <summary> ĳ���� ������ ����ü</summary>
[Serializable]
public struct CharacterStat
{
    public float HP; //ü�� Health Point
    public float AP; //���� Armor Point
    public float DP; //���ݷ� Damage Point
    public float MoveSpeed;
    public float TurnSpeed;
    public float Hunger; // ���
    public float Thirsty; // ����
}

public class Character : MonoBehaviour
{
    /// <summary> Animator ������Ʈ ��ȯ </summary>
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
