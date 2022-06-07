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
    public event UnityAction GetRifle = null;
    public event UnityAction GetPistol = null;
    public event UnityAction PutGun = null;
    public event UnityAction AnimStart = null;
    public event UnityAction AnimEnd = null;

    public Transform Knife; // Į(��������)

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
    /// <summary> ���� ���� �ִϸ��̼� ���� </summary>
    public void StartStabbing()
    {
        GetKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(true);
        this.GetComponent<Animator>().SetBool("IsAttacking", true);
    }

    /// <summary> ���� ���� �ִϸ��̼� �� </summary>
    public void EndStabbing()
    {
        PutKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(false);
        if (this.GetComponent<Animator>().GetBool("IsGun")) 
        this.GetComponent<Animator>().SetBool("IsAttacking", false);
    }

    /// <summary> �ֹ��� ���� </summary>
    public void OnGetRifle()//�� ������� �ִϸ��̼ǿ� �۵�
    {
        GetRifle?.Invoke();
        if(_player.isFirst) _player.GetGun(0); // �ֹ��� 1��° ������ �������� Hand ���Ͽ� ����
        if (_player.isSecond) _player.GetGun(1); // �ֹ��� 2��° ������ �������� Hand ���Ͽ� ����
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }

    /// <summary> �������� ���� </summary>
    public void OnGetPistol()
    {
        GetPistol?.Invoke();
        _player.GetPistol(); // �ι��� ������ �������� Hand ���Ͽ� ����
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }

    /// <summary> ���� ���� </summary>
    public void OnPutGun()
    {
        PutGun?.Invoke();
        _player.PutGun();
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }

    /// <summary> �ִϸ��̼� ���� ���� </summary>
    public void OnAnimStart()
    {
        AnimStart?.Invoke();
        _player.AnimStart();
    }

    /// <summary> �ִϸ��̼� ���� �� </summary>
    public void OnAnimEnd()
    {
        AnimEnd?.Invoke();
        _player.AnimEnd();
    }
}
