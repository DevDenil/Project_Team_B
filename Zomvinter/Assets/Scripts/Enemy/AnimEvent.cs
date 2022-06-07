using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    /// ��������Ʈ ���� ///
    public event UnityAction Attack = null;
    public event UnityAction AttackStart = null;
    public event UnityAction AttackEnd = null;

    /// <summary> ���� ��������Ʈ </summary>
    public void OnAttack()
    {
        Attack?.Invoke();
    }

    /// <summary> ���� ���� ���� ��������Ʈ </summary>
    public void OnAttackStart()
    {
        AttackStart?.Invoke();
    }

    /// <summary> ���� �� ���� ��������Ʈ </summary>
    public void OnAttackEnd()
    {
        AttackEnd?.Invoke();
    }
}
