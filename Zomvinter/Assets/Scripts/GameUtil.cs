using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary> ���� Ÿ�� ���� ����ü </summary>
public struct time
{
    public int day;
    public float Gametime;
}
public class GameUtil : MonoBehaviour
{

}

/// <summary> ���� �ý��� �������̽� </summary>
public interface BattleSystem
{
    void OnDamage(float Damage);
    void OnCritDamage(float CritDamage);
    bool IsLive();
    Transform transform { get; }

}