using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZMoveController : Character
{
    /*
    public enum STATE
    {
        NONE, IDLE, ROAM, BATTLE, DEAD
    }
    private STATE myState = STATE.NONE;

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
    */

    protected Transform myTarget = null;

    private float Angle;
    private float Dir;

    /*-----------------------------------------------------------------------------------------------*/
    /*
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
                mySensor.FindTarget = FindTarget;
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
                MoveToPosition(myTarget.transform);
                break;
            case STATE.DEAD:
                break;
        }
    }
    */

    private MonsterData myData;
    /*-----------------------------------------------------------------------------------------------*/
    void Start()
    {
        //ChangeState(STATE.IDLE);
    }

    void Update()
    {
        //StateProcess();
    }
    /*-----------------------------------------------------------------------------------------------*/
    /*
    protected void FindTarget()
    {
        if (mySensor.myEnemy != null)
        {
            myTarget = mySensor.myEnemy.transform;
            //ChangeState(STATE.BATTLE);
        }
    }
    */

    protected void MoveToPosition(Transform Target)
    {
        // myAnim.SetBool("IsMoving", true);
        if (MoveRoutine != null) StopCoroutine(MoveRoutine);
        MoveRoutine = StartCoroutine(Chasing(Target.position, myData.AttRange, myData.AttDelay, myData.AttSpeed));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(Target.position));
    }
    /*-----------------------------------------------------------------------------------------------*/
    Coroutine MoveRoutine = null;
    protected IEnumerator Chasing(Vector3 pos, float AttackRange, float AttackDelay, float AttackSpeed)
    {
        float AttackTime = AttackDelay;
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();

        if (!myAnim.GetBool("isMoving")) myAnim.SetBool("isMoving", true);
        //While 조건문으로 Aggro 해제 조건 삽입
        while (true)
        {
            Debug.Log("isOn");
            //공격 거리 유지
            if (Dist > AttackRange)
            {
                float delta = myData.MoveSpeed * Time.deltaTime;

                if (Dist < delta)
                {
                    delta = Dist;
                }
                
                Debug.Log("asd");
                this.transform.Translate(Dir * delta, Space.World);
                Dist -= delta;
            }
            else
            {
                AttackTime += Time.deltaTime;
                if (myAnim.GetBool("isMoving")) myAnim.SetBool("isMoving", false);
                //myAnim.SetBool("isMoving", false);
                if (AttackTime >= AttackDelay)
                {
                    //Debug.Log(AttackTime);
                    /*if(랜덤 난수)
                     * {
                     * 크리티컬 확률 Anim
                     * }
                     * else 
                     * {
                     * 일반 공격 확률 Anim
                     * }
                     */

                    //Anim Set
                    AttackTime = 0.0f;
                }
            }
            yield return null;
        }
    }

    Coroutine RotRoutine = null;
    protected IEnumerator Rotating(Vector3 pos)
    {
        //지점 방향 벡터
        Vector3 _Dir = (pos - this.transform.position).normalized;
        CalcAngle(this.transform.forward, _Dir, this.transform.right);

        while (Angle > Mathf.Epsilon)
        {
            float delta = myData.TurnSpeed * Time.deltaTime;
            delta = delta > Angle ? Angle : delta;

            this.transform.Rotate(Vector3.up * delta * Dir);

            Angle -= delta;
            yield return null;
        }
        RotRoutine = null;
    }

    private void CalcAngle(Vector3 src, Vector3 des, Vector3 right)
    {
        float Radian = Mathf.Acos(Vector3.Dot(src, des));
        //로테이션 값
        Angle = 180.0f * (Radian / Mathf.PI);
        //회전의 좌, 우방향 값
        Dir = 1.0f;
        if (Vector3.Dot(right, des) < 0.0f)
        {
            Dir = -1.0f;
        }
    }
}
