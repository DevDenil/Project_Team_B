using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct MonsterData
{
    public float MoveSpeed;
    public float AttDelay;
    public float AttSpeed;
}
public class ZMoveController : Character
{
    
    public enum STATE
    {
        NONE, IDLE, ROAM, ATTACK, DEAD
    }
    private STATE myState = STATE.NONE;
    private MonsterData myData;
    private Transform myTarget = null;

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
                break;
            case STATE.ROAM:
                break;
            case STATE.ATTACK:
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
            case STATE.ATTACK:
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
        this.GetComponentInChildren<SphereCollider>();
    }

    public void MoveToPosition(Transform Target)
    {
        myAnim.SetBool("Paramier_Move", true);
        this.transform.Translate(Target.position * myData.MoveSpeed * Time.deltaTime);
    } 
    /*-----------------------------------------------------------------------------------------------*/
}
