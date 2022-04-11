using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZMoveController : MonoBehaviour
{
    public enum STATE
    {
        NONE, IDLE, ROAM, ATTACK, DEAD
    }
    public STATE myState = STATE.NONE;
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.IDLE:
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
                break;
            case STATE.ROAM:
                break;
            case STATE.ATTACK:
                break;
            case STATE.DEAD:
                break;
        }
    }
    void Start()
    {
        ChangeState(STATE.IDLE);
    }

    void Update()
    {
        StateProcess();
    }
}
