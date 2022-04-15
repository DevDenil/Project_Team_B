using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//LJM
public class ZMonster_Normal : ZMoveController, BattleSystem
{
    // ������ �ҷ�����
    public enum STATE
    {
        NONE, IDLE, ROAM, BATTLE, DEAD
    }

    ZSensor _sensor = null;
    ZSensor mySensor
    {
        get
        {
            if (_sensor == null)
            {
                _sensor = this.GetComponentInChildren<ZSensor>();
            }
            return _sensor;
        }
    }

    public Transform myTarget = null; 

    /*-----------------------------------------------------------------------------------------------*/
    // ���� ����

    public STATE myState = STATE.NONE;
    public Transform myWeapon;
    public LayerMask EnemyMask;
    //GameUtil ���� �� ����
    private MonsterData myData;
    private CharacterStat myStat;

    /*-----------------------------------------------------------------------------------------------*/
    // ���� ���� ���
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.IDLE:
                //
                //mySensor.FindTarget = FindTarget;
                myData.MoveSpeed = 1.5f;
                myData.TurnSpeed = 180.0f;
                myData.AttRange = 1.0f;
                myData.AttDelay = 3.5f;
                myData.AttSpeed = 1.0f;
                myData.UnChaseTime = 3.0f;
                
                break;
            case STATE.ROAM:
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.IDLE:
                FindTarget();
                break;
            case STATE.ROAM:
                FindTarget();
                break;
            case STATE.BATTLE:
                ChaseTarget();
                break;
            case STATE.DEAD:
                break;
        }
    }
    /*-----------------------------------------------------------------------------------------------*/
    // Start, Update
    void Start()
    {
        ChangeState(STATE.IDLE);
    }

    void Update()
    {
        StateProcess();
    }
    /*-----------------------------------------------------------------------------------------------*/
    void OnAttack()
    {
        Debug.Log("���� ����");
        Collider[] list = Physics.OverlapSphere(myWeapon.position, 1.0f, EnemyMask);
        foreach (Collider col in list)
        {
            BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
            if (bs != null)
            {
                bs.OnDamage(50.0f);
            }
        }
    }
    public void OnDamage(float Damage)
    {

    }
    public void OnCritDamage(float CritDamage)
    {

    }
    public bool IsLive()
    {
        return true;
    }
    /*-----------------------------------------------------------------------------------------------*/
    // ���� �Լ�
    protected void FindTarget()
    {
        if (mySensor.myEnemy != null)
        {
            //if (UnChaseCor != null) StopAllCoroutines();
            myTarget = mySensor.myEnemy.transform;
            ChangeState(STATE.BATTLE);
        }
        else
        {
            //UnChaseCor = StartCoroutine(UnChaseTimer(myData.UnChaseTime));
            ChangeState(STATE.IDLE);
        }
    }
    private void ChaseTarget()
    {
        //Ÿ��Pos, �̵� �ӵ�, ���� �Ÿ�, ���� ������, ���� �ӵ�, �� �ӵ�
        MoveToPosition(myTarget.transform, myData.MoveSpeed, 
            myData.AttRange, myData.AttDelay, myData.AttSpeed, myData.TurnSpeed);
    }


    /*
    Coroutine UnChaseCor = null;
    IEnumerator UnChaseTimer (float T)
    {
        if(mySensor.myEnemy == null)
        {
            yield return new WaitForSeconds(T);
        }

        myTarget = null;
        UnChaseCor = null;
    }
    */
}
