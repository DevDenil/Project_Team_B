using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerController, BattleSystem
{
    /*-----------------------------------------------------------------------------------------------*/
    //���� ����
    public CharacterStat myStat;

    //�κ��丮
    public List<GameObject> myItems = new List<GameObject>();

    //���콺 ������Ʈ
    public Transform RotatePoint;
    //�̵� ����
    Vector3 pos = Vector3.zero;
    /*-----------------------------------------------------------------------------------------------*/
    //Unity
    void Start()
    {
        ChangeState(STATE.CREATE);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    /*-----------------------------------------------------------------------------------------------*/
    //���� ���� ���
    public enum STATE
    {
        NONE, CREATE, ALIVE, BATTLE, DEAD
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
            case STATE.CREATE:
                myStat.MoveSpeed = 3.0f;
                ChangeState(STATE.ALIVE);
                break;
            case STATE.ALIVE:
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
            case STATE.CREATE:
                break;
            case STATE.ALIVE:
                Move();
                Rotation();
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }
    /*-----------------------------------------------------------------------------------------------*/
    void Move()
    { 
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");
        base.Moving(pos ,myStat.MoveSpeed);
    }
    void Rotation()
    {
        base.Rotate(RotatePoint);
    }
    /*-----------------------------------------------------------------------------------------------*/
    public void AddItems(GameObject obj)
    {
        myItems.Add(Instantiate(obj));
    }

    /*-----------------------------------------------------------------------------------------------*/
    // ��Ʋ �ý���
    void OnAttack()
    {
        Fire();
    }
    public void OnDamage(float Damage)
    {

    }
    public void OnCritDamage(float CritDamage)
    {

    }
    public bool IsLive()
    {
        return true;
    }
    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
