using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameUtil : MonoBehaviour
{
    public struct time
    {
        public float Gametime;
        public int day;
    }
    public interface BattleSystem
    {
        void OnDamage(float Damage);
        bool IsLive();
        Transform transform { get; }
    }
}
