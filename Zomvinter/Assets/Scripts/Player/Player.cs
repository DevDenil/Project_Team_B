using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary> ���� ���� ��� - ���� ��� </summary>
public enum STATE
{
    NONE, CREATE, ALIVE, BATTLE, DEAD
}

public class Player : PlayerController, BattleSystem
{
    #region �÷��̾� ������ ����
    //���� ����ü
    public static CharacterStat Stat;
    #endregion

    #region �κ��丮 UI ����
    [SerializeField]
    public Inventory myInventory;
    public StatUI myStatUI;

    private bool ActiveInv = true;
    #endregion

    #region ���� ����
    // ī�޶� �� ��
    public Transform Cam;
    #endregion

    #region ���� ���� ��� - ����
    public STATE myState = STATE.NONE;
    #endregion

    #region �ڷ�ƾ ����
    Coroutine aliveCycle = null;
    #endregion


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

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� Start�� ������ �ڵ带 �ۼ� �� ��) </summary>
    private void OnValidate()
    {

    }

    void Awake()
    {
        RotatePoint = GetComponent<Transform>();
        // ����ü ���� ��äȭ
        Stat = new CharacterStat();
        Stat.CycleSpeed = 1.0f;
        Stat.MaxHP = 100.0f; // �ִ� ü��
        Stat.MaxHunger = 100.0f; // �ִ� ��ⷮ
        Stat.MaxStamina = 50.0f; // �ִ� ���׹̳�
        Stat.MaxThirsty = 100.0f; // �ִ� ���� ��ġ
        ChangeState(STATE.CREATE);
    }

    void Update()
    {
        DynamicStatClamp();
        StateProcess();
    }
    #endregion

    /***********************************************************************
    *                               Finite-state machine
    ***********************************************************************/
    #region ���� ���� ���
    /// <summary> ���� ��� ���� ��ȯ �Լ� </summary>
    /// <param name="s">����</param>
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
    /// <summary> ���� ��� Update �Լ� </summary>
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.CREATE:
                break;
            /// <summary> Player�� ��� �ִ� ��� </summary>
            case STATE.ALIVE:
                AliveCoroutine();
                InputMethods();
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }
    #endregion

    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region Private �Լ�

    #region Init Methods
    /// <summary> ĳ���� ���� ���� �ּ� �ִ밪 ���� </summary>
    void DynamicStatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }
    #endregion

    void Move(float MoveSpeed)
    {
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");
        base.Moving(pos, MoveSpeed, Cam);
    }
    void Rotation()
    {
        base.Rotate(Cam);
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�

    #endregion

    /***********************************************************************
    *                       Private BattleSystem Methods
    ***********************************************************************/
    #region Private BattleSystem �Լ�
    void OnAttack()
    {
        //base.BulletRotate(bulletRotate);
    }

    void InputMethods()
    {
        #region �̵� Input Methods

        ///<summary> �̵� Input �޼��� </summary>
        if (!myAnim.GetBool("IsAiming")) Move(Stat.MoveSpeed);

        ///<summary> �޸��� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            myAnim.SetBool("IsRun", true); //�޸���
        }

        ///<summary> �޸��� ��� Input �޼��� </summary>
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            myAnim.SetBool("IsRun", false);
        }
        #endregion

        #region ���� Input Methods
        ///<summary> ��� Input �޼��� </summary>
        if (Input.GetMouseButtonDown(0) && myAnim.GetBool("IsAiming"))// && GunCheck)
        {
            Fire();
        }

        ///<summary> ���� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.V))
        {
            myAnim.SetTrigger("Melee");
        }

        ///<summary> ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha1) && !myAnim.GetBool("IsGun"))//&&GunCheck
        {
            // ���� ���� �ִϸ��̼� ����
            myAnim.SetLayerWeight(1, 1.0f);
            myAnim.SetBool("IsGun", true);
            myAnim.SetTrigger("GetGun");
        }

        ///<summary> ��� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.X))// && GunCheck
        {
            myAnim.SetLayerWeight(1, 0.0f);
            myAnim.SetTrigger("PutGun");
            myAnim.SetBool("IsGun", false);
            myAnim.SetBool("IsAiming", false);
        }
        #endregion

        #region ���� ���� Input Methods
        ///<summary> ���� ���� Input �޼��� </summary>
        if (myAnim.GetBool("IsGun") && Input.GetMouseButton(1))
        {
            Move(Stat.MoveSpeed / 2);
            Rotation();
            myAnim.SetBool("IsAiming", true);
        }

        ///<summary> ���� ���� ���� Input �޼��� </summary>
        if (myAnim.GetBool("IsGun") && Input.GetMouseButtonUp(1))
        {
            myAnim.SetBool("IsAiming", false);
        }
        #endregion

        #region UI Input Methods
        if (Input.GetKeyDown(KeyCode.Tab) && myState != STATE.DEAD)
        {
            if (ActiveInv)
            {
                myInventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(1100.0f, 0.0f);
            }
            else
            {
                myInventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            }
            ActiveInv = !ActiveInv;
        }

        #endregion
    }
    #endregion

    /***********************************************************************
    *                       Public BattleSystem Methods
    ***********************************************************************/
    #region Public BattleSystem �Լ�

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
    #endregion

    void Fire()
    {
        Instantiate(bullet, bulletStart.position, bulletRotate.rotation);
    }
    void GetWeapon() // �ֹ��� ȹ��� � ����
    {
        if (GunCheck1 == true)
            Instantiate(UnArmedGun1, UnArmed1);
        if (GunCheck2 == true)
            Instantiate(UnArmedGun2, UnArmed2);
    }
    void PutWeapon() // �ֹ��� ������ ����
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
    /***********************************************************************
    *                               Corutine
    ***********************************************************************/
    #region �ڷ�ƾ
    IEnumerator AliveCycle()
    {
        while (Stat.HP > Mathf.Epsilon)
        {
            // ����� ��ġ 0�϶�
            while (Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty > Mathf.Epsilon)
            {
                Debug.Log(Stat.Thirsty);
                Stat.HP -= Stat.CycleSpeed * 3;
                Stat.Thirsty -= Stat.CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // ���� ��ġ 0�϶�
            while (Stat.Thirsty <= Mathf.Epsilon && Stat.Hunger > Mathf.Epsilon)
            {
                Stat.Hunger -= Stat.CycleSpeed;
                Stat.Stamina += Stat.StaminaCycle / 2;
                Stat.HP -= Stat.CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // �� �� 0�϶�
            while (Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty <= Mathf.Epsilon)
            {
                Stat.HP -= Stat.CycleSpeed * 5;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            Stat.Hunger -= Stat.CycleSpeed;
            Stat.Thirsty -= Stat.CycleSpeed * 2;
            Stat.Stamina += Stat.StaminaCycle;
            yield return new WaitForSecondsRealtime(1.0f);
        }
        yield return null;
    }
    #endregion
}
