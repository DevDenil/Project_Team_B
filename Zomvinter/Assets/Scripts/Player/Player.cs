using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class Player : PlayerController, BattleSystem
{
    /*-----------------------------------------------------------------------------------------------*/
    //���� ����
    public CharacterStat myStat;
    public LayerMask BulletLayer;//�Ѿ� ������
    private Vector3 dir; // �Ѿ� ���� 
    //�κ��丮
    public List<GameObject> myItems = new List<GameObject>();
    public GameObject myInventory;
    private bool ActiveInv = false;

    //���콺 ������Ʈ
    public Transform RotatePoint;
    //�̵� ����
    Vector3 pos = Vector3.zero;
    //�Ѿ�������, �Ѿ˹߻���ġ, �Ѿ˰���
    public Transform bullet; public Transform bulletStart; public Transform bulletRotate;
    //�� ȹ��� �����ϱ� ���� boolcheck�� 
    public bool GunCheck1 = false; public bool GunCheck2 = false;
    //�� ȹ��� �����Ǵ� ��ġ�� �θ� ����
    public Transform UnArmed1; public Transform UnArmed2; //�÷��̾� �� �κп� �����Ǵ� ��ġ
    public Transform UnArmedGun1; public Transform UnArmedGun2; // �÷��̾ ȹ���� ������ prefab�� ����ؾ��� 

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
        if (Input.GetKeyDown(KeyCode.Tab) && myState != STATE.DEAD)
        {
            ActiveInv = !ActiveInv;
            myInventory.SetActive(ActiveInv);
        }
        if (myAnim.GetBool("IsGun") && Input.GetMouseButtonDown(1))
        {
            myAnim.SetBool("IsAiming", true);
            base.Rotate(RotatePoint);
            BulletRotCtrl();
            //Debug.Log("Q");
            //myAnim.runtimeAnimatorController = Resources.Load("PlayerGun") as RuntimeAnimatorController;
        }
        if (myAnim.GetBool("IsGun") && Input.GetMouseButtonUp(1))
        {
            myAnim.SetBool("IsAiming", false);
        }
        //�� ȹ��� Guncheck true �����
        //if (GunCheck)
    }
    private void LateUpdate()
    {
        base.BulletRotate(bulletRotate);
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
                //Rotation();
                if (Input.GetMouseButton(0) && myAnim.GetBool("IsAiming"))//&& GunCheck
                {
                    Fire();
                }
                if (Input.GetKeyDown(KeyCode.Alpha1) && !myAnim.GetBool("IsGun"))//&&GunCheck
                {
                    myAnim.SetBool("IsGun", true);
                    myAnim.SetTrigger("GetGun");
                }
                if (Input.GetKeyDown(KeyCode.X))// && GunCheck
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
        base.Moving(pos, myStat.MoveSpeed, RotatePoint);
    }
    void Rotation()
    {
        //base.Rotate(RotatePoint);
    }


    /*-----------------------------------------------------------------------------------------------*/
    // ��Ʋ �ý���
    public void TakeHit(float damage, RaycastHit hit)
    {

    }
    void OnAttack()
    {
        //base.BulletRotate(bulletRotate);

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
    void BulletRotCtrl()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out RaycastHit hit, 9999.0f))
        //{
        //    Vector3 pos = bulletStart.position - hit.point;
        //}
        //Quaternion rotation = Quaternion.LookRotation(pos);
        //transform.rotation = rotation;  
    }
    void GetWeapon()//�ֹ��� ȹ��� � ����
    {
        if (GunCheck1 == true)
            Instantiate(UnArmedGun1, UnArmed1);
        if (GunCheck2 == true)
            Instantiate(UnArmedGun2, UnArmed2);
    }
    void PutWeapon()//�ֹ��� ������ ����
    {
        if (GunCheck1 == false)
            if (UnArmedGun1 != null) Destroy(UnArmedGun1);
        if (GunCheck2 == false)
            if (UnArmedGun2 != null) Destroy(UnArmedGun2);
    }
}
