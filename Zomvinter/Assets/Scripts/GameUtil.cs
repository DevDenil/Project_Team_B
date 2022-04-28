using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary> 월드 타임 변수 구조체 </summary>
public struct time
{
    public int day;
    public float Gametime;
}
public class GameUtil : MonoBehaviour
{

}

/// <summary> 전투 시스템 인터페이스 </summary>
public interface BattleSystem
{
    void OnDamage(float Damage);
    void OnCritDamage(float CritDamage);
    bool IsLive();
    Transform transform { get; }

}