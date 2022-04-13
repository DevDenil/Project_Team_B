using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZMonster_Normal : ZMoveController
{
    // ������ �ҷ�����
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

    public enum STATE
    {
        NONE, IDLE, ROAM, BATTLE, DEAD
    }
    /*-----------------------------------------------------------------------------------------------*/
    // ���� ����

    public STATE myState = STATE.NONE;
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
                //Ÿ��Pos, �̵� �ӵ�, ���� �Ÿ�, ���� ������, ���� �ӵ�, �� �ӵ�
                base.MoveToPosition(myTarget.transform, myData.MoveSpeed, myData.AttRange, myData.AttDelay, myData.AttSpeed,myData.TurnSpeed);
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
    // ���� �Լ�
    protected void FindTarget()
    {
        if (mySensor.myEnemy != null)
        {
            myTarget = mySensor.myEnemy.transform;
            ChangeState(STATE.BATTLE);
        }
    }
}
