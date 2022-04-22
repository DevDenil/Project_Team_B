using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public event UnityAction Attack = null;
    public event UnityAction AttackStart = null;
    public event UnityAction AttackEnd = null;

    public void OnAttack()
    {
        Attack?.Invoke();
    }
    public void OnAttackStart()
    {
        AttackStart?.Invoke();
    }
    public void OnAttackEnd()
    {
        AttackEnd?.Invoke();
    }
}
