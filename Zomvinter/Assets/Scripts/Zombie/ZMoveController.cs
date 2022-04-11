using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct MonsterData
{
    public float MoveSpeed;
    public float TurnSpeed;
    public float AttDelay;
    public float AttSpeed;
}
public class ZMoveController : Character
{
    public enum STATE
    {
        NONE, IDLE, ROAM, BATTLE, DEAD
    }
    private STATE myState = STATE.NONE;
    private MonsterData myData;

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

    public float Angle;
    public float Dir;

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
                mySensor.FindTarget = FindTarget;
                myData.MoveSpeed = 1.5f;
                myData.TurnSpeed = 180.0f;
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
    public void FindTarget()
    {
        if (mySensor.myEnemy != null)
        {
            myTarget = mySensor.myEnemy.transform;
            ChangeState(STATE.BATTLE);
        }
    }

    public void MoveToPosition(Transform Target)
    {
        //myAnim.SetBool("Paramier_Move", true);
        if (MoveRoutine != null) StopCoroutine(MoveRoutine);
        MoveRoutine = StartCoroutine(Moving(Target.position));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(Target.position));
    }
    /*-----------------------------------------------------------------------------------------------*/
    Coroutine MoveRoutine = null;
    IEnumerator Moving(Vector3 pos)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();

        while (Dist > Mathf.Epsilon)
        {
            float delta = myData.MoveSpeed * Time.deltaTime;

            if (Dist < delta)
            {
                delta = Dist;
            }
            this.transform.Translate(Dir * delta, Space.World);
            Dist -= delta;

            yield return null;
        }
    }

    Coroutine RotRoutine = null;
    IEnumerator Rotating(Vector3 pos)
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

    public void CalcAngle(Vector3 src, Vector3 des, Vector3 right)
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
