using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZSensor : MonoBehaviour
{
    public UnityAction FindTarget = null;
    //������ �����ϴ� ���̾�
    public LayerMask myEnemyMask;
    //Ÿ��
    //public BattleSystem myTarget = null;
    //�ݰ� �ȿ� ���� �� ���
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
