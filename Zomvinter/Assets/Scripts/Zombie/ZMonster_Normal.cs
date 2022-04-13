using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZMonster_Normal : ZMoveController
{
    // 데이터 불러오기
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
    // 전역 변수

    public STATE myState = STATE.NONE;
    //GameUtil 구축 시 이전
    private MonsterData myData;
    private CharacterStat myStat;

    /*-----------------------------------------------------------------------------------------------*/
    // 유한 상태 기계
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
    // 지역 함수
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
            Debug.Log("AAAAAAAAA");
            //UnChaseCor = StartCoroutine(UnChaseTimer(myData.UnChaseTime));
            ChangeState(STATE.IDLE);
        }
    }
    private void ChaseTarget()
    {
        //타겟Pos, 이동 속도, 공격 거리, 공격 딜레이, 공격 속도, 턴 속도
        MoveToPosition(myTarget.transform, myData.MoveSpeed, 
            myData.AttRange, myData.AttDelay, myData.AttSpeed, myData.TurnSpeed);
    }

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
}
