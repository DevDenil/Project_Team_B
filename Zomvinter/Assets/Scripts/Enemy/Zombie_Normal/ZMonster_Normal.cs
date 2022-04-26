using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//LJM
public class ZMonster_Normal : ZMoveController, BattleSystem
{
    /* ��ȯ ���� -----------------------------------------------------------------------------------------------*/

    /// <summary> ���� �ν� ���� ������Ʈ ��ȯ </summary>
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



    /* ���� ���� -----------------------------------------------------------------------------------------------*/

    /// <summary> �� Ÿ�� ������Ʈ ���̾� </summary>
    public LayerMask EnemyMask;
    /// <summary> �� Ÿ�� ������Ʈ ��ġ �� </summary>
    public Transform myTarget = null;
    /// <summary> �� ���� ���� ������Ʈ ��ġ�� </summary>
    public Transform myWeapon;

    /// <summary> ĳ���� ���� ����ü ���� </summary>
    private MonsterData myData;
    private CharacterStat myStat;
    
    //bool AttackTerm = false;

    /* ���� ���� ��� -----------------------------------------------------------------------------------------------*/

    /// <summary> ���� ���� ��� ���� </summary>
    public enum STATE
    {
        NONE, IDLE, ROAM, BATTLE, DEAD
    }
    public STATE myState = STATE.NONE;

    /// <summary> ���� ���� ��� Start </summary>
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

    /// <summary> ���� ���� ��� Update </summary>
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
    /* ���� �Լ� -----------------------------------------------------------------------------------------------*/
    
    void Start()
    {
        ChangeState(STATE.IDLE); // ���� ���� ��� �ʱ�ȭ

        /// ��������Ʈ �߰� ///
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
        GetComponentInChildren<AnimEvent>().AttackEnd += OnAttackEnd;
    }

    void Update()
    {
        StateProcess();
    }

    /* ��Ʋ �ý��� - ���� -----------------------------------------------------------------------------------------------*/
    void OnAttack()
    {
        if (myAnim.GetBool("AttackTerm"))
        {
            Debug.Log("���� ����");
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
    /* ��Ʋ �ý��� - �ǰ� -----------------------------------------------------------------------------------------------*/
    public void OnDamage(float Damage)
    {

        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {
            //�ǰݾִϸ��̼� ����
        }
    }

    public void OnCritDamage(float CritDamage)
    {

    }

    public bool IsLive()
    {
        return true;
    }
    /* ���� �Լ� -----------------------------------------------------------------------------------------------*/

    /// <summary> Ÿ�� �˻� �Լ� </summary>
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

    /// <summary> Ÿ�� �߰� �Լ� </summary>
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
