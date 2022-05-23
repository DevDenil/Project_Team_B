using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    Player _player;

    public event UnityAction GetKnife = null; // Į ��Ƽ��
    public event UnityAction PutKnife = null; // Į ���Ƽ��
    public event UnityAction GetGun = null;
    public event UnityAction PutGun = null; 
    public event UnityAction ArmGun = null;
    public event UnityAction StartAim = null;// ���� ���� ���� 

    public Transform Knife; // Į(��������)

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

    public void OnGetGun()//�� ������� �ִϸ��̼ǿ� �۵�
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
    public void OnArmedGun() //���� ������ �� ����Ʈ������ �۵�
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
