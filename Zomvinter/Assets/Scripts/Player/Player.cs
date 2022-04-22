using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class Player : PlayerController, BattleSystem
{
    /*-----------------------------------------------------------------------------------------------*/
    //지역 변수
    public CharacterStat myStat;
    
    //인벤토리
    public List<GameObject> myItems = new List<GameObject>();
    
    //마우스 로테이트
    public Transform RotatePoint;
    //이동 벡터
    Vector3 pos = Vector3.zero;
    //총알프리팹, 총알발사위치, 총알각도
    public Transform bullet; public Transform bulletStart; public Transform bulletRotate;
    //총 획득시 생성하기 위한 boolcheck용 
    public bool GunCheck = false;
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
        if (myAnim.GetBool("IsGun") && Input.GetMouseButtonDown(1))
        {
            myAnim.SetBool("IsAiming", true);
            //Debug.Log("Q");
            //myAnim.runtimeAnimatorController = Resources.Load("PlayerGun") as RuntimeAnimatorController;
        }
        if (myAnim.GetBool("IsGun") && Input.GetMouseButtonUp(1))
        {
            myAnim.SetBool("IsAiming", false);
        }
        //총 획득시 Guncheck true 만들기
        //if (GunCheck)
    }
    /*-----------------------------------------------------------------------------------------------*/
    //유한 상태 기계
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
                if (Input.GetMouseButtonDown(0) && myAnim.GetBool("IsAiming") && GunCheck)
                { 
                    Fire(); 
                }
                if (Input.GetKeyDown(KeyCode.Alpha1) && !myAnim.GetBool("IsGun") &&GunCheck)
                {
                    myAnim.SetBool("IsGun", true);
                    myAnim.SetTrigger("GetGun");
                }
                if (Input.GetKeyDown(KeyCode.X) && GunCheck)
                {
                    myAnim.SetTrigger("PutGun");
                    myAnim.SetBool("IsGun", false);
                    myAnim.SetBool("IsAiming", false);
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    myAnim.SetTrigger("Melee");
                }
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }

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
    // 배틀 시스템
    public void TakeHit(float damage, RaycastHit hit)
    {

    }
    void OnAttack()
    {
        Fire();
    }
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {

        }
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
        Instantiate(bullet, bulletStart.position, bulletRotate.rotation);
    }
}
