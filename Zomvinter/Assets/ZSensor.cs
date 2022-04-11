using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZSensor : MonoBehaviour
{
    public UnityAction FindTarget = null;
    //적인지 구분하는 레이어
    public LayerMask myEnemyMask;
    //타겟
    //public BattleSystem myTarget = null;
    //반경 안에 들어온 적 목록
    public GameObject myEnemy = null;
    private void OnTriggerEnter(Collider other)
    {
        //EnemyMask & (1 << other.gameObject.layer)) != 0
        if ((myEnemyMask & (1 << other.gameObject.layer)) != 0)
        {
            myEnemy = other.gameObject;
            FindTarget?.Invoke();
            /*
            if (myTarget == null)
            {
                myTarget = other.gameObject.GetComponent<BattleSystem>();
                FindTarget?.Invoke();
            }
            */
        }
    }
    private void OnTriggerExit(Collider other)
    {
        myEnemy = null;
    }
}
