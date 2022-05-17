using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : PlayerController, BattleSystem
{
    //���� ����ü
    public static CharacterStat Stat;
    public Transform Cam;
    /*-----------------------------------------------------------------------------------------------*/
    //���� ����
    public LayerMask BulletLayer;//�Ѿ� ������
    private Vector3 dir; // �Ѿ� ���� 
    [SerializeField]
    private float CycleSpeed; // ���, ���� ���� �ӵ�
    [SerializeField]
    private float StaminaCycle; // ���׹̳� ���� �ӵ�
    //�κ��丮
    public List<GameObject> myItems = new List<GameObject>();
    public GameObject myInventory;
    private bool ActiveInv = true;
    public GameObject myStatUI;
    //���콺 ������Ʈ
    public Transform RotatePoint;
    //ĳ���� �� �Ʒ�����
    public Transform mySpine;
    //�̵� ����
    Vector3 pos = Vector3.zero;
    //�Ѿ�������, �Ѿ˹߻���ġ, �Ѿ˰���
    public Transform bullet; public Transform bulletStart; public Transform bulletRotate;
    //�� ȹ��� �����ϱ� ���� boolcheck�� 
    public bool GunCheck1 = false;
    public bool GunCheck2 = false;
    //�� ȹ��� �����Ǵ� ��ġ�� �θ� ����
    public Transform UnArmed1; public Transform UnArmed2; //�÷��̾� �� �κп� �����Ǵ� ��ġ
    public Transform UnArmedGun1; public Transform UnArmedGun2; // �÷��̾ ȹ���� ������ prefab�� ����ؾ��� 
    Coroutine aliveCycle = null;
    Coroutine move = null;
    /*-----------------------------------------------------------------------------------------------*/
    //Unity
    void Start()
    {
        RotatePoint = GetComponent<Transform>();
        CycleSpeed = 1.0f;
        Stat = new CharacterStat();
        Stat.MaxHP = 100.0f; // �ִ� ü��
        Stat.MaxHunger = 100.0f; // �ִ� ��ⷮ
        Stat.MaxStamina = 50.0f; // �ִ� ���׹̳�
        Stat.MaxThirsty = 100.0f; // �ִ� ���� ��ġ
        ChangeState(STATE.CREATE);
    }

    // ĳ���� ��ġ�� 0���� �۾����� �ʰ� �ִ� ��ġ���� Ŀ���� �ʰ� �����ϴ� �Լ�
    void DynamicStatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }

    // Update is called once per frame
    void Update()
    { 
        DynamicStatClamp();
        StateProcess();
    }
    private void LateUpdate()
    {
        //���� ����(y��) pos�� �����ؾ���
        //mySpine.localRotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(pos), Time.deltaTime * 10.0f);
        if (Input.GetKeyDown(KeyCode.Tab) && myState != STATE.DEAD)
        {
            ActiveInv = !ActiveInv;
            myInventory.SetActive(ActiveInv);
        }
        //if (myAnim.GetBool("IsGun") && Input.GetMouseButtonDown(1))
        //{
        //    myAnim.SetBool("IsAiming", true);
        //    //Debug.Log("Q");
        //    //myAnim.runtimeAnimatorController = Resources.Load("PlayerGun") as RuntimeAnimatorController;
        //}
        //if (myAnim.GetBool("IsGun") && Input.GetMouseButtonUp(1))
        //{
        //    myAnim.SetBool("IsAiming", false);
        //}
        //�� ȹ��� Guncheck true �����
        //if (GunCheck)
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
                ChangeState(STATE.ALIVE);
                StatInitialize();
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
                float ZMove = Stat.MoveSpeed / 2;
                AliveCoroutine();
                //RotatePoint.transform.rotation = Quaternion.identity;

                if (!myAnim.GetBool("IsAiming")) Move(Stat.MoveSpeed);
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    myAnim.SetBool("IsRun", true); //�޸���
                }
                if (Input.GetKeyUp(KeyCode.LeftShift)) myAnim.SetBool("IsRun", false); //�޸��ⳡ
         
                /*if (Input.GetMouseButton(0) && myAnim.GetBool("IsAiming") && !myAnim.GetBool("IsGun"))
                {  
                    Move(ZMove);
                    //Rotation();
                }*/
                if (Input.GetMouseButtonDown(0) && myAnim.GetBool("IsAiming"))// && GunCheck)
                {
                    Fire();
                }
                if (Input.GetKeyDown(KeyCode.Alpha1) && !myAnim.GetBool("IsGun"))// && GunCheck)
                {
                    Fire();
                }
                if (Input.GetKeyDown(KeyCode.Alpha1) && !myAnim.GetBool("IsGun"))//&&GunCheck
                {
                    myAnim.SetLayerWeight(1, 1.0f);
                    myAnim.SetBool("IsGun", true);
                    myAnim.SetTrigger("GetGun");
                }
                if (Input.GetKeyDown(KeyCode.X))// && GunCheck
                {
                    myAnim.SetLayerWeight(1, 0.0f);
                    myAnim.SetTrigger("PutGun");
                    myAnim.SetBool("IsGun", false);
                    myAnim.SetBool("IsAiming", false);
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    myAnim.SetTrigger("Melee");
                }
                if (Input.GetKeyDown(KeyCode.Tab) && myState != STATE.DEAD)
                {
                    ActiveInv = !ActiveInv;
                    myInventory.SetActive(ActiveInv);
                    myStatUI.SetActive(ActiveInv);
                }
                if (myAnim.GetBool("IsGun") && Input.GetMouseButtonDown(1))
                {
                    StartAiming(); //�����̼� �� ����
                    myAnim.SetBool("IsAiming", true);
                    BulletRotCtrl();
                }
                if (myAnim.GetBool("IsGun") && Input.GetMouseButton(1))
                {
                    //float ZoomMove = Stat.MoveSpeed / 2;
                    Move(Stat.MoveSpeed);
                    Rotation();
                    //BulletRotate(bulletRotate);
                }
                if (myAnim.GetBool("IsGun") && Input.GetMouseButtonUp(1))
                {
                    myAnim.SetBool("IsAiming", false);
                    StopAiming(); Debug.Log("StopAiming");

                }
                //�� ȹ��� Guncheck true �����
                //if (GunCheck)
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }

    void Move(float MoveSpeed)
    {
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");
        base.Moving(pos, MoveSpeed, Cam);
    }
    void Rotation()
    {
        base.Rotate(RotatePoint);
    }


    /*-----------------------------------------------------------------------------------------------*/
    // ��Ʋ �ý���
    void OnAttack()
    {
        //base.BulletRotate(bulletRotate);

    }
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        Stat.HP -= Damage;
        if (Stat.HP <= 0) ChangeState(STATE.DEAD);
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

    void StatInitialize()
    {
        //�ʱ� ĳ���� ��ġ
        Stat.HP = Stat.MaxHP;
        Stat.AP = 0.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        //�ʱ� ĳ���� �ɷ�ġ ����
        Stat.Strength = 0;
        Stat.Cadio = 0;
        Stat.Handicraft = 0;
        Stat.Agility = 0;
        Stat.Intellect = 0;
    }
    void AliveCoroutine()
    {
        if (aliveCycle != null) return;
        aliveCycle = StartCoroutine(AliveCycle());
    }

    IEnumerator AliveCycle()
    {
        while (Stat.HP > Mathf.Epsilon)
        {
            // ����� ��ġ 0�϶�
            while(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty > Mathf.Epsilon)
            {
                Debug.Log(Stat.Thirsty);
                Stat.HP -= CycleSpeed * 3;
                Stat.Thirsty -= CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // ���� ��ġ 0�϶�
            while (Stat.Thirsty <= Mathf.Epsilon && Stat.Hunger > Mathf.Epsilon)
            {
                Stat.Hunger -= CycleSpeed;
                Stat.Stamina += StaminaCycle / 2;
                Stat.HP -= CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // �� �� 0�϶�
            while(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty <= Mathf.Epsilon)
            {
                Stat.HP -= CycleSpeed * 5;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            Stat.Hunger -= CycleSpeed;
            Stat.Thirsty -= CycleSpeed * 2;
            Stat.Stamina += StaminaCycle;
            yield return new WaitForSecondsRealtime(1.0f);
        }
        yield return null;
    }

}
