using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    /// 딜리게이트 선언 ///
    public event UnityAction Attack = null;
    public event UnityAction AttackStart = null;
    public event UnityAction AttackEnd = null;

    /// <summary> 공격 딜리게이트 </summary>
    public void OnAttack()
    {
        Attack?.Invoke();
    }

    /// <summary> 공격 시작 지점 딜리게이트 </summary>
    public void OnAttackStart()
    {
        AttackStart?.Invoke();
    }

    /// <summary> 공격 끝 지점 딜리게이트 </summary>
    public void OnAttackEnd()
    {
        AttackEnd?.Invoke();
    }
}
