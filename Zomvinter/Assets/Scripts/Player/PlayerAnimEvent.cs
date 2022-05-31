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
    public event UnityAction GetGun = null;
    public event UnityAction PutGun = null;
    public event UnityAction GetPistol = null;
    public event UnityAction PutPistol = null;
    public event UnityAction AnimEnd = null;

    public Transform Knife; // 칼(근접무기)

    private void OnValidate()
    {
        
    }

    private void Awake()
    {
        _player = this.GetComponentInParent<Player>();
    }

    public void StartStabbing()
    {
        GetKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(true);
        this.GetComponent<Animator>().SetBool("IsAttacking", true);
    }
    public void EndStabbing()
    {
        PutKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(false);
        if (this.GetComponent<Animator>().GetBool("IsGun")) 
        this.GetComponent<Animator>().SetBool("IsAttacking", false);
    }

    public void OnGetGun()//총 꺼내드는 애니메이션에 작동
    {
        GetGun?.Invoke();
        // 1. 첫번째 무장을 선택한 경우
        if(_player.isFirst)
        {
            // 1-1. 이미 무장 중인 경우 (2번째 무장을 사용 중)
            if(_player.Armed)
            {
                Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
                Destroy(_player.BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거
                _player.GetGun(0); // 주무장 1번째 슬롯의 아이템을 Hand 소켓에 생성
                _player.UpdateBackWeapon(); // 백 소켓 업데이트
            }
            // 1-2. 무장이 없는 경우
            else
            {
                Destroy(_player.BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거
                _player.GetGun(0); // 주무장 1번째 슬롯의 아이템을 Hand 소켓에 생성
                _player.UpdateBackWeapon(); // 백 소켓 업데이트
            }
        }
        // 2. 두번째 무장을 선택한 경우
        else
        {
            // 2-1. 이미 무장 중인 경우 (1번째 무장을 사용 중)
            if (_player.Armed)
            {
                Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
                Destroy(_player.BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거
                _player.GetGun(1); // 주무장 2번째 슬롯의 아이템을 Hand 소켓에 생성
                _player.UpdateBackWeapon(); // 백 소켓 업데이트
            }
            else
            {
                Destroy(_player.BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거
                _player.GetGun(1); // 주무장 1번째 슬롯의 아이템을 Hand 소켓에 생성
                _player.UpdateBackWeapon(); // 백 소켓 업데이트
            }
        }
    }

    public void OnGetPistol()
    {
        GetPistol?.Invoke();
        // 1. 무장 중인 경우
        if(_player.Armed)
        {
            Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
            Destroy(_player.PistolGrip.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 허리에 있는 아이템 제거
            _player.GetPistol(); // 부무장 슬롯의 아이템을 Hand 소켓에 생성
            _player.UpdateBackWeapon(); // 백 소켓 업데이트
        }
        // 2. 무장 중이 아닌 경우
        else
        {
            Destroy(_player.PistolGrip.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 허리에 있는 아이템 제거
            _player.GetPistol(); // 부무장 슬롯의 아이템을 Hand 소켓에 생성
            _player.UpdateBackWeapon(); // 백 소켓 업데이트
        }
    }
    public void OnPutGun()
    {
        PutGun?.Invoke();
        Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }
    public void OnPutPistol()
    {
        PutPistol?.Invoke();
        Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }

    public void OnAnimEnd()
    {
        AnimEnd?.Invoke();
        _player.AnimEnd();
    }
}
