using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    public event UnityAction GetGun = null;
    public event UnityAction PutGun = null; 
    public event UnityAction ArmGun = null;
    public event UnityAction StartAim = null;// 총을 꺼낸 이후 
    public Transform ArmedGun; // 아이들, 이동 동작때 사용 
    public Transform UnArmedGun; // 등에 매달아 둔 상태 
    public Transform AnimGun; // 꺼내는동작때만 사용
    public void OnGetGun()//총 꺼내드는 애니메이션에 작동
    {
        GetGun?.Invoke();
        ArmedGun.GetComponent<Transform>().gameObject.SetActive(true);
        UnArmedGun.GetComponent<Transform>().gameObject.SetActive(false);
        AnimGun.GetComponent<Transform>().gameObject.SetActive(false);
    }
    public void OnArmedGun() //총을 꺼내든 후 블랜드트리에서 작동
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
