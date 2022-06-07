using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    Player _player;

    public event UnityAction GetKnife = null; // 칼 액티브
    public event UnityAction PutKnife = null; // 칼 언액티브
    public event UnityAction GetRifle = null;
    public event UnityAction GetPistol = null;
    public event UnityAction PutGun = null;
    public event UnityAction AnimStart = null;
    public event UnityAction AnimEnd = null;

    public Transform Knife; // 칼(근접무기)

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    private void Awake()
    {
        _player = this.GetComponentInParent<Player>();
    }

    /***********************************************************************
    *                               Anim Events
    ***********************************************************************/
    /// <summary> 근접 공격 애니메이션 시작 </summary>
    public void StartStabbing()
    {
        GetKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(true);
        this.GetComponent<Animator>().SetBool("IsAttacking", true);
    }

    /// <summary> 근접 공격 애니메이션 끝 </summary>
    public void EndStabbing()
    {
        PutKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(false);
        if (this.GetComponent<Animator>().GetBool("IsGun")) 
        this.GetComponent<Animator>().SetBool("IsAttacking", false);
    }

    /// <summary> 주무기 장착 </summary>
    public void OnGetRifle()//총 꺼내드는 애니메이션에 작동
    {
        GetRifle?.Invoke();
        if(_player.isFirst) _player.GetGun(0); // 주무장 1번째 슬롯의 아이템을 Hand 소켓에 생성
        if (_player.isSecond) _player.GetGun(1); // 주무장 2번째 슬롯의 아이템을 Hand 소켓에 생성
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }

    /// <summary> 보조무기 장착 </summary>
    public void OnGetPistol()
    {
        GetPistol?.Invoke();
        _player.GetPistol(); // 부무장 슬롯의 아이템을 Hand 소켓에 생성
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }

    /// <summary> 무장 해제 </summary>
    public void OnPutGun()
    {
        PutGun?.Invoke();
        _player.PutGun();
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }

    /// <summary> 애니메이션 동작 시작 </summary>
    public void OnAnimStart()
    {
        AnimStart?.Invoke();
        _player.AnimStart();
    }

    /// <summary> 애니메이션 동작 끝 </summary>
    public void OnAnimEnd()
    {
        AnimEnd?.Invoke();
        _player.AnimEnd();
    }
}
