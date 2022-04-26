using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//LJM
public class ZMonster_Normal : ZMoveController, BattleSystem
{
    /* 반환 변수 -----------------------------------------------------------------------------------------------*/

    /// <summary> 좀비 인식 범위 오브젝트 반환 </summary>
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



    /* 전역 변수 -----------------------------------------------------------------------------------------------*/

    /// <summary> 내 타겟 오브젝트 레이어 </summary>
    public LayerMask EnemyMask;
    /// <summary> 내 타겟 오브젝트 위치 값 </summary>
    public Transform myTarget = null;
    /// <summary> 내 공격 판정 오브젝트 위치값 </summary>
    public Transform myWeapon;

    /// <summary> 캐릭터 정보 구조체 선언 </summary>
    private MonsterData myData;
    private CharacterStat myStat;
    
    //bool AttackTerm = false;

    /* 유한 상태 기계 -----------------------------------------------------------------------------------------------*/

    /// <summary> 유한 상태 기계 선언 </summary>
    public enum STATE
    {
        NONE, IDLE, ROAM, BATTLE, DEAD
    }
    public STATE myState = STATE.NONE;

    /// <summary> 유한 상태 기계 Start </summary>
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
                myStat.DP = 5.0f;
                break;
            case STATE.ROAM:
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }

    /// <summary> 유한 상태 기계 Update </summary>
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
                //OnAttack();
                break;
            case STATE.DEAD:
                break;
        }
    }
    /* 실행 함수 -----------------------------------------------------------------------------------------------*/
    
    void Start()
    {
        ChangeState(STATE.IDLE); // 유한 상태 기계 초기화

        /// 딜리게이트 추가 ///
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
        GetComponentInChildren<AnimEvent>().AttackEnd += OnAttackEnd;
    }

    void Update()
    {
        StateProcess();
    }

    /* 배틀 시스템 - 공격 -----------------------------------------------------------------------------------------------*/
    void OnAttack()
    {
        if (myAnim.GetBool("AttackTerm"))
        {
            Debug.Log("공격 성공");
            Collider[] list = Physics.OverlapSphere(myWeapon.position, 1.0f, EnemyMask);
            foreach (Collider col in list)
            {
                BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
                if (bs != null)
                {
                    bs.OnDamage(myStat.DP);
                }
            }
        }
        else Debug.Log("ss");
    }

    void OnAttackStart() 
    {
        myAnim.SetBool("AttackTerm", true); 
        OnAttack();
    }

    void OnAttackEnd()
    {
        myAnim.SetBool("AttackTerm", false);
    }
    /* 배틀 시스템 - 피격 -----------------------------------------------------------------------------------------------*/
    public void OnDamage(float Damage)
    {

        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {
            //피격애니메이션 구현
        }
    }

    public void OnCritDamage(float CritDamage)
    {

    }

    public bool IsLive()
    {
        return true;
    }
    /* 지역 함수 -----------------------------------------------------------------------------------------------*/

    /// <summary> 타겟 검색 함수 </summary>
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

    /// <summary> 타겟 추격 함수 </summary>
    private void ChaseTarget()
    {
        //타겟Pos, 이동 속도, 공격 거리, 공격 딜레이, 공격 속도, 턴 속도
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
