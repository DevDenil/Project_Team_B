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
    public event UnityAction GetPistol = null;
    public event UnityAction PutPistol = null;
    public event UnityAction AnimEnd = null;

    public Transform Knife; // Į(��������)

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

    public void OnGetGun()//�� ������� �ִϸ��̼ǿ� �۵�
    {
        GetGun?.Invoke();
        // 1. ù��° ������ ������ ���
        if(_player.isFirst)
        {
            // 1-1. �̹� ���� ���� ��� (2��° ������ ��� ��)
            if(_player.Armed)
            {
                Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
                Destroy(_player.BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
                _player.GetGun(0); // �ֹ��� 1��° ������ �������� Hand ���Ͽ� ����
                _player.UpdateBackWeapon(); // �� ���� ������Ʈ
            }
            // 1-2. ������ ���� ���
            else
            {
                Destroy(_player.BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
                _player.GetGun(0); // �ֹ��� 1��° ������ �������� Hand ���Ͽ� ����
                _player.UpdateBackWeapon(); // �� ���� ������Ʈ
            }
        }
        // 2. �ι�° ������ ������ ���
        else
        {
            // 2-1. �̹� ���� ���� ��� (1��° ������ ��� ��)
            if (_player.Armed)
            {
                Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
                Destroy(_player.BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
                _player.GetGun(1); // �ֹ��� 2��° ������ �������� Hand ���Ͽ� ����
                _player.UpdateBackWeapon(); // �� ���� ������Ʈ
            }
            else
            {
                Destroy(_player.BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
                _player.GetGun(1); // �ֹ��� 1��° ������ �������� Hand ���Ͽ� ����
                _player.UpdateBackWeapon(); // �� ���� ������Ʈ
            }
        }
    }

    public void OnGetPistol()
    {
        GetPistol?.Invoke();
        // 1. ���� ���� ���
        if(_player.Armed)
        {
            Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
            Destroy(_player.PistolGrip.GetComponentInChildren<WeaponItem>().gameObject); // ���� �㸮�� �ִ� ������ ����
            _player.GetPistol(); // �ι��� ������ �������� Hand ���Ͽ� ����
            _player.UpdateBackWeapon(); // �� ���� ������Ʈ
        }
        // 2. ���� ���� �ƴ� ���
        else
        {
            Destroy(_player.PistolGrip.GetComponentInChildren<WeaponItem>().gameObject); // ���� �㸮�� �ִ� ������ ����
            _player.GetPistol(); // �ι��� ������ �������� Hand ���Ͽ� ����
            _player.UpdateBackWeapon(); // �� ���� ������Ʈ
        }
    }
    public void OnPutGun()
    {
        PutGun?.Invoke();
        Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }
    public void OnPutPistol()
    {
        PutPistol?.Invoke();
        Destroy(_player.HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }

    public void OnAnimEnd()
    {
        AnimEnd?.Invoke();
        _player.AnimEnd();
    }
}
