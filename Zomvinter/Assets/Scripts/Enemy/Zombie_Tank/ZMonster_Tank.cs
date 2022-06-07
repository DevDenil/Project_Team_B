using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZMonster_Tank : ZMoveController, BattleSystem
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

    /// <summary> 내 타겟 오브젝트 레이어 </summary>
    LayerMask EnemyMask;
    /// <summary> 내 타겟 오브젝트 위치 값 </summary>
    Transform myTarget = null;

    /// <summary> 내 공격 판정 오브젝트 위치값 </summary>
    public Transform myWeapon;

    /// <summary> 캐릭터 정보 구조체 선언 </summary>
    MonsterData myData;
    public CharacterStat myStat;
    float SkillTime = 0.0f;
    [SerializeField]
    float SkillDelay;

    Vector3 RushTarget;
    //bool AttackTerm = false;

    /* 유한 상태 기계 -----------------------------------------------------------------------------------------------*/

    /// <summary> 유한 상태 기계 선언 </summary>
    enum STATE
    {
        CREATE, ROAM, BATTLE, RUSH, DEAD
    }
    [SerializeField]
    STATE myState;

    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.CREATE:
                break;
            case STATE.ROAM:
                myStat.MoveSpeed = 3.0f;
                myStat.TurnSpeed = 180.0f;
                myData.AttRange = 1.8f;
                myData.AttDelay = 1.5f;
                myData.AttSpeed = 1.0f;
                myData.UnChaseTime = 3.0f;
                myStat.DP = 5.0f;
                EnemyMask = LayerMask.GetMask("Player");
                myAnim.SetBool("isMoving", false);
                myTarget = null;
                StopAllCoroutines();
                StartCoroutine(Waitting(Random.Range(1.0f, 3.0f), Roaming));
                RushTarget = Vector3.zero;
                break;
            case STATE.BATTLE:
                myAnim.SetBool("isMoving", false);
                StopAllCoroutines();
                break;
            case STATE.RUSH:
                StopAllCoroutines();
                myAnim.SetTrigger("Rush");
                if (myAnim.GetBool("IsReady") == false && myAnim.GetBool("IsRush") == false)
                {
                    RushTarget = myTarget.gameObject.transform.position;
                }
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
            case STATE.CREATE:
                break;
            case STATE.ROAM:
                FindTarget();
                break;
            case STATE.BATTLE:
                ChaseTarget();
                //OnAttack();
                break;
            case STATE.RUSH:
                RushCoroutine();
                break;
            case STATE.DEAD:
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.ROAM); // 유한 상태 기계 초기화

        /// 딜리게이트 추가 ///
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
        GetComponentInChildren<AnimEvent>().AttackEnd += OnAttackEnd;
        GetComponentInChildren<AnimEvent>().IsRushing += IsRushing;
        GetComponentInChildren<AnimEvent>().Attackclear += AttackClear;
        GetComponentInChildren<AnimEvent>().endRush += EndRush;
        GetComponentInChildren<AnimEvent>().Camerashake += CameraShake;
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void OnAttack()
    {
        if (myAnim.GetBool("AttackTerm"))
        {
            Collider[] list = Physics.OverlapSphere(myWeapon.position, 2.5f, EnemyMask);
            foreach (Collider col in list)
            {
                Debug.Log("Attack");
                BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
                if (bs != null)
                {
                    bs.OnDamage(myStat.DP);
                }
            }
        }
        else Debug.Log("ss");
    }
    /// <summary> 공격 애니메이션 시작 지점 체크 함수 </summary>
    void OnAttackStart()
    {
        myAnim.SetBool("AttackTerm", true);
        OnAttack();
    }
    /// <summary> 공격 애니메이션 끝 지점 체크 함수 </summary>
    void OnAttackEnd()
    {
        myAnim.SetBool("AttackTerm", false);
    }

    void IsRushing()
    {
        myAnim.SetBool("IsRush",true);
        myAnim.SetBool("IsReady", false);
    }

    void EndRush()
    {
        myAnim.SetBool("IsRush", false);
    }

    void CameraShake()
    {
        
    }

    void AttackClear()
    {
        myAnim.SetBool("IsAttacking", false);
    }
    /* 배틀 시스템 - 피격 -----------------------------------------------------------------------------------------------*/
    /// <summary> 피격 함수 </summary>
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
    /// <summary> 크리티컬 피격 함수 </summary>
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
            ChangeState(STATE.ROAM);
        }
    }

    void Roaming()
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(-5.0f, 5.0f);
        pos.z = Random.Range(-5.0f, 5.0f);
        base.RoamToPosition(pos, myStat.MoveSpeed, myStat.TurnSpeed, () => StartCoroutine(Waitting(Random.Range(1.0f,3.0f), Roaming)));
    }

    IEnumerator Waitting(float t, UnityAction done)
    {
        yield return new WaitForSeconds(t);
        done?.Invoke();
    }

    /// <summary> 타겟 추격 함수 </summary>
    private void ChaseTarget()
    {
        if (mySensor.myEnemy != null)
        {
            SkillTime += Time.deltaTime;
            if (SkillTime < SkillDelay)
            {
                MoveToPosition(myTarget.transform, myStat.MoveSpeed,
                    myData.AttRange, myData.AttDelay, myData.AttSpeed, myStat.TurnSpeed);
            }
            else
            {
                ChangeState(STATE.RUSH);
            }
        }
        else
        {
            ChangeState(STATE.ROAM);
        }
    }

    Coroutine rush;
    void RushCoroutine()
    {
        float MoveSpeed = myStat.MoveSpeed * 4;
        if (rush != null) StopCoroutine(rush);
        rush = StartCoroutine(IsRush(RushTarget, MoveSpeed));
    }

    IEnumerator IsRush(Vector3 pos, float MoveSpeed)
    {
        if (myAnim.GetBool("IsAttacking") == false && myAnim.GetBool("IsReady") == false)
        {
            Vector3 Dir = pos - this.transform.position;
            float Dist = Dir.magnitude;
            Dir.Normalize();
            while (Dist > Mathf.Epsilon)
            {
                float delta = MoveSpeed * Time.deltaTime;
                if (Dist < delta)
                {
                    delta = Dist;
                }
                Dist -= delta;
                this.transform.Translate(Dir * delta, Space.World);
                Debug.Log(Dist);
                yield return null;
            }
            myAnim.SetBool("IsRush", false);
            myAnim.SetBool("IsReady", false);
            myAnim.SetTrigger("Skill");
            SkillTime = 0.0f;
            ChangeState(STATE.BATTLE);
            rush = null;
        }
    }
}
