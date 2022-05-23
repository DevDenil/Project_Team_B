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
    public event UnityAction ArmGun = null;
    public event UnityAction StartAim = null;// 총을 꺼낸 이후 

    public Transform Knife; // 칼(근접무기)

    private void OnValidate()
    {
        
    }

    private void Awake()
    {
        _player = this.GetComponent<Player>();
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
        if(_player.isFirst)
        {
            if(_player.HandSorket.GetComponentInChildren<WeaponItem>() != null)
            {
                Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject);
            }
            _player.GetGun(0);
        }
        else
        {
            if (_player.HandSorket.GetComponentInChildren<WeaponItem>() != null)
            {
                Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject);
            }
            _player.GetGun(1);
        }
    }
    public void OnArmedGun() //총을 꺼내든 후 블랜드트리에서 작동
    {
        ArmGun?.Invoke();
    }
    public void OnPutGun()
    {
        PutGun?.Invoke();
    }
    public void OnStartAim()
    {
        StartAim?.Invoke();
    }
}
