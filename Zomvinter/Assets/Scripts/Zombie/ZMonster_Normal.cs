using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZMonster_Normal : ZMoveController
{
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

    private Transform myTarget = null;

    public enum STATE
    {
        NONE, IDLE, ROAM, BATTLE, DEAD
    }
    private STATE myState = STATE.NONE;
    //GameUtil 구축 시 이전
    private MonsterData myData;
    private CharacterStat myStat;
    /*-----------------------------------------------------------------------------------------------*/
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
                base.MoveToPosition(myTarget.transform);
                break;
            case STATE.DEAD:
                break;
        }
    }
    /*-----------------------------------------------------------------------------------------------*/
    void Start()
    {
        ChangeState(STATE.IDLE);
    }

    void Update()
    {
        StateProcess();
    }
    /*-----------------------------------------------------------------------------------------------*/
    protected void FindTarget()
    {
        if (mySensor.myEnemy != null)
        {
            myTarget = mySensor.myEnemy.transform;
            ChangeState(STATE.BATTLE);
        }
    }
}
