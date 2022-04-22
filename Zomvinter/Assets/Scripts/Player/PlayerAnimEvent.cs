using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    public event UnityAction GetKnife = null; // Į ��Ƽ��
    public event UnityAction PutKnife = null; // Į ���Ƽ��
    public event UnityAction GetGun = null;
    public event UnityAction PutGun = null; 
    public event UnityAction ArmGun = null;
    public event UnityAction StartAim = null;// ���� ���� ���� 

    public Transform Knife; // Į(��������)
    public Transform ArmedGun; // ���̵�, �̵� ���۶� ��� 
    public Transform UnArmedGun; // � �Ŵ޾� �� ���� 
    public Transform AnimGun; // ��ݶ� ���
    //--------------------------------------------------------------------------------------
    public Transform PaArmedGun; //���̵�, �̵� ���� ������ �θ� 
    public Transform PaUnArmedGun; 
    public Transform PaAnimGun;

    public void StartStabbing()
    {
        GetKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(true);
        ArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        UnArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        AnimGun.GetComponent<Transform>().gameObject.SetActive(false);
        this.GetComponent<Animator>().SetBool("IsAttacking", true);
    }
    public void EndStabbing()
    {
        PutKnife?.Invoke();
        Knife.GetComponent<Transform>().gameObject.SetActive(false);
        if (this.GetComponent<Animator>().GetBool("IsGun")) 
        ArmedGun.GetComponent<Transform>().gameObject.SetActive(true);
        this.GetComponent<Animator>().SetBool("IsAttacking", false);
    }
    public void OnGetGun()//�� ������� �ִϸ��̼ǿ� �۵�
    {
        GetGun?.Invoke();
        ArmedGun.GetComponent<Transform>().gameObject.SetActive(true);
        UnArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        AnimGun.GetComponent<Transform>().gameObject.SetActive(false);
    }
    public void OnArmedGun() //���� ������ �� ����Ʈ������ �۵�
    {
        ArmGun?.Invoke();
        ArmedGun.GetComponent<Transform>().gameObject.SetActive(true);
        UnArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        AnimGun.GetComponent<Transform>().gameObject.SetActive(false); 
    }
    public void OnPutGun()
    {
        PutGun?.Invoke();
        ArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        UnArmedGun.GetComponent<Transform>().gameObject.SetActive(true);
        AnimGun.GetComponent<Transform>().gameObject.SetActive(false);
    }
    public void OnStartAim()
    {
        StartAim?.Invoke();
        ArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        UnArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        AnimGun.GetComponent<Transform>().gameObject.SetActive(true);
    }
}
