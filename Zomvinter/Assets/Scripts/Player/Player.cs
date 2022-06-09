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
    private Transform _canvas;
    private Inventory _Inventory;
    private StatUI _statUI;

    private bool ActiveInv = true;
    #endregion

    #region ���� ����
    //�̵� ����
    Vector3 pos = Vector3.zero;
    Vector3 LookPos = Vector3.zero;
    [SerializeField]
    /// <summary> ī�޶� ���� �� </summary>
    private Transform _cameraArm;
    #endregion

    #region �̵� ���� ����

    #endregion

    #region ���� ���� ��� - ����
    public STATE myState = STATE.NONE;
    #endregion

    #region �ڷ�ƾ ����
    private Coroutine aliveCycle = null;
    #endregion

    #region ���� ����
    [SerializeField]
    public GameObject myWeapon = null;
    [SerializeField]
    public Transform HandSorket;
    [SerializeField]
    public Transform BackLeftSorket;
    [SerializeField]
    public Transform BackRightSorket;
    [SerializeField]
    public Transform PistolGrip;

    /// <summary> ���� ���� üũ Bool </summary>
    public bool Armed = false;
    /// <summary> �ִϸ��̼� ���� üũ Bool </summary>
    public bool MotionEnd = true;

    [SerializeField]
    /// <summary> ù��° ������ �����ߴ��� üũ Bool </summary>
    public bool isFirst = false;
    [SerializeField]
    /// <summary> �ι�° ������ �����ߴ��� üũ Bool </summary>
    public bool isSecond = false;
    [SerializeField]
    /// <summary> �� ������ �����ߴ��� üũ Bool </summary>
    public bool isPistol = false;

    [SerializeField]
    /// <summary> ���� ���� üũ Bool </summary>
    private bool Aimed = true;
    #endregion

    //ĳ���� �� �Ʒ�����
    Transform mySpine;

    [SerializeField]
    Transform BaseTrans;
    Vector3 LookDir = Vector3.zero;

    //�Ѿ�������, �Ѿ˹߻���ġ, �Ѿ˰���
    // public Transform bullet; public Transform bulletStart; public Transform bulletRotate;

    //�� ȹ��� �����ϱ� ���� boolcheck�� 
    // public bool GunCheck1 = false;
    // public bool GunCheck2 = false;

    //�� ȹ��� �����Ǵ� ��ġ�� �θ� ����
    // public Transform UnArmed1; public Transform UnArmed2; //�÷��̾� �� �κп� �����Ǵ� ��ġ
    // public Transform UnArmedGun1; public Transform UnArmedGun2; // �÷��̾ ȹ���� ������ prefab�� ����ؾ��� 

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
        Stat = new CharacterStat(); // ����ü ���� ��äȭ
        InitScripts();
        ChangeState(STATE.CREATE);
    }

    void Update()
    {
        StateProcess();
    }

    private void FixedUpdate()
    {
        Move(Stat.MoveSpeed);
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
                InitStatClamp();
                InitStat();
                UpdateBackWeapon();
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
    private void InitStat()
    {
        // �ʱ� ĳ���� �ִ� ��ġ
        Stat.CycleSpeed = 1.0f;
        Stat.MaxHP = 100.0f; // �ִ� ü��
        Stat.MaxHunger = 100.0f; // �ִ� ��ⷮ
        Stat.MaxStamina = 50.0f; // �ִ� ���׹̳�
        Stat.MaxThirsty = 100.0f; // �ִ� ���� ��ġ

        //�ʱ� ĳ���� ��ġ
        Stat.HP = Stat.MaxHP;
        Stat.AP = 0.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.TurnSpeed = 180.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        //�ʱ� ĳ���� �ɷ�ġ ����
        Stat.Strength = 0;
        Stat.Constitution = 0;
        Stat.Dexterity = 0;
        Stat.Endurance = 0;
        Stat.Intelligence = 0;
    }

    /// <summary> ĳ���� ���� ���� �ּ� �ִ밪 ���� </summary>
    private void InitStatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }

    private void InitScripts()
    {
        _Inventory = _canvas.GetComponentInChildren<Inventory>();
        //_statUI = _canvas.GetComponentInChildren<StatUI>();
    }
    #endregion

    void Move(float MoveSpeed)
    {
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");

        // Base Transform ���� ���� ����
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mousePos, out RaycastHit hit, 1 << LayerMask.NameToLayer("Ground")))
        {
            LookDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - BaseTrans.position;
            BaseTrans.forward = LookDir.normalized;
        }

        // �ڷ� �ȴ� �ӵ� ����
        if (!myAnim.GetBool("isRun") && pos.z < 0)
        {
            Stat.MoveSpeed = 2.0f;
        }
        else
        {
            Stat.MoveSpeed = 3.0f;
        }

        base.Moving(this.transform, pos, MoveSpeed, _cameraArm);
        // base.AnimMove(BaseTrans, LookPos, MoveSpeed, BaseTrans);

        if (!Aimed)
        {
            // this.transform.rotation = Quaternion.Euler(0.0f, _cameraArm.eulerAngles.y, 0.0f);
            base.Rotate(this.transform);
        }
        else
        {
            base.Rotate(this.transform);
        }
    }

    private void AliveCoroutine()
    {
        if (aliveCycle != null) return;
        aliveCycle = StartCoroutine(AliveCycle());
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    public void GetGun(int index)
    {
        if(myWeapon != null) Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        if(isFirst) Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
        if(isSecond) Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����

        myWeapon = Instantiate(_Inventory.PrimaryItems[index].ItemPrefab, HandSorket.position, HandSorket.rotation); // Gun Object ����
        myWeapon.transform.parent = HandSorket.transform; // Gun ������Ʈ ���Ͽ� �ڽ�ȭ
        myWeapon.layer = 0;
        Destroy(myWeapon.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
        Destroy(myWeapon.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
    }

    public void GetPistol()
    {
        if (myWeapon != null) Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        if (isFirst) Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
        if (isSecond) Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����

        myWeapon = Instantiate(_Inventory.SecondaryItems.ItemPrefab, HandSorket.position, HandSorket.rotation);
        myWeapon.transform.parent = HandSorket.transform;
        myWeapon.layer = 0;
        Destroy(myWeapon.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
        Destroy(myWeapon.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
    }

    public void PutGun()
    {
        Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        myWeapon = null;
        isFirst = false;
        isSecond = false;
        isPistol = false;
        Armed = false;
    }

    public void UpdateBackWeapon()
    {
        // 1. �� ���� 1�� ���Կ� ��� �ְ� / �� ���� 1���� ��� ���� / ù��° ���⸦ ������� �ƴ� ���
        if (_Inventory.PrimaryItems[0] != null && BackLeftSorket.GetComponentInChildren<WeaponItem>() == null && !isFirst)
        {
            GameObject gun1 = Instantiate(_Inventory.PrimaryItems[0].ItemPrefab, BackLeftSorket); //�ֹ��� 1�� �ִ� ��� ����
            Destroy(gun1.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            Destroy(gun1.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            gun1.layer = 0; // UI�� �������� �ʵ��� ���̾� ����
            gun1.transform.parent = BackLeftSorket.transform; // �Ҵ�� �� ���Ͽ� �ڽ����� ����
        }
        // 2. �� ���� 2�� ���Կ� ��� �ְ� / �� ���� 2���� ��� ���� / ù��° ���⸦ ������� ���
        if (_Inventory.PrimaryItems[1] != null && BackRightSorket.GetComponentInChildren<WeaponItem>() == null && !isSecond)
        {
            GameObject gun2 = Instantiate(_Inventory.PrimaryItems[1].ItemPrefab, BackRightSorket); //�ֹ��� 2�� �ִ� ��� ����
            Destroy(gun2.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            Destroy(gun2.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            gun2.layer = 0; // UI�� �������� �ʵ��� ���̾� ����
            gun2.transform.parent = BackRightSorket.transform; // �Ҵ�� �� ���Ͽ� �ڽ����� ����
        }
        if (_Inventory.SecondaryItems != null && PistolGrip.GetComponentInChildren<WeaponItem>() == null)
        {
            GameObject pistol = Instantiate(_Inventory.SecondaryItems.ItemPrefab, PistolGrip);
            Destroy(pistol.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            Destroy(pistol.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            pistol.layer = 0; // UI�� �������� �ʵ��� ���̾� ����
            pistol.transform.parent = PistolGrip.transform; // �Ҵ�� �㸮 ���Ͽ� �ڽ����� ����
        }
    }
    #endregion

    /***********************************************************************
    *                               Anim Methods
    ***********************************************************************/
    #region �ִϸ��̼� �޼ҵ�
    /// <summary> ���̾� ������ ���� �ִϸ��̼� ���� ���� Ȯ�� </summary>
    public void AnimStart()
    {
        if (MotionEnd) MotionEnd = false;
        myAnim.SetLayerWeight(1, 1.0f);
    }

    /// <summary> ���̾� ������ ���� �ִϸ��̼� ���� �� Ȯ�� </summary>
    public void AnimEnd()
    {
        if (!MotionEnd) MotionEnd = true;
        myAnim.SetLayerWeight(1, 0.0f);
    }
    #endregion

    /***********************************************************************
    *                               Input Methods
    ***********************************************************************/
    #region Input �Լ�
    private void InputMethods()
    {
        #region �̵� Input Methods

        ///<summary> �޸��� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            myAnim.SetBool("isRun", true);
            
            // ���¹̳� �Ҹ�


            // 1. ���� ������ �� �޸��� ���
            if(myAnim.GetBool("isArmed"))
            {
                Stat.MoveSpeed = 3.0f;
            }
            // 2. �� ���� ������ �� �޸��� ���
            else
            {
                Stat.MoveSpeed = 3.5f;
            }
        }

        ///<summary> �޸��� ��� Input �޼��� </summary>
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            myAnim.SetBool("isRun", false);

            // 1. ���� ������ �� �ȴ� ���
            if (myAnim.GetBool("isArmed"))
            {
                Stat.MoveSpeed = 1.5f;
            }
            // 2. �� ���� ������ �� �ȴ� ���
            else
            {
                Stat.MoveSpeed = 2.0f;
            }
        }
        #endregion

        #region ���� Input Methods
        ///<summary> ��� Input �޼��� </summary>
        if (Input.GetMouseButtonDown(0))
        {
            // ���� ���� �� ���
            if(myAnim.GetBool("isAiming"))
            {
                Fire();
            }
            // �ƴ� ��� - Null
        }

        ///<summary> ���� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.V))
        {
            myAnim.SetTrigger("MeleeAttack");

            //// ���� ���� ó��
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(MotionEnd) myAnim.SetTrigger("Reload");

            //// ������ ����
        }

        ///<summary> ù��° �ֹ��� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 1. �ֹ��� 1�� ���Կ� �������� �ְ� �� ������ 1�� ���Կ� �������� �ִ� ���
            if (_Inventory.PrimaryItems[0] != null && !isFirst && MotionEnd)
            {
                if (!Armed) Armed = true;
                if (!isFirst) isFirst = true; // ù��° �ֹ��� true
                if (isSecond) isSecond = false; // �ι�° �ֹ��� false
                if (isPistol) isPistol = false; // �������� false

                if (isFirst) myAnim.SetTrigger("GetGun"); // ���� �ִϸ��̼� ���

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // ���� ���� üũ
            }
        }
        ///<summary> �ι�° �ֹ��� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // 1. 2�� ���Կ� �������� �ִ� ���
            if (_Inventory.PrimaryItems[1] != null && !isSecond && MotionEnd)
            {
                if (!Armed) Armed = true;
                if (isFirst) isFirst = false; // ù��° �ֹ��� false
                if (!isSecond) isSecond = true; // �ι�° �ֹ��� true
                if (isPistol) isPistol = false; // �������� false

                if (isSecond) myAnim.SetTrigger("GetGun"); // ���� �ִϸ��̼� ���

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // ���� ���°� false �� ��� true
            }
        }
        ///<summary> �������� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 1. 2�� ���Կ� �������� �ִ� ���
            if (_Inventory.SecondaryItems != null && !isPistol && MotionEnd)
            {
                if (!Armed) Armed = true;
                if (isFirst) isFirst = false; // ù��° �ֹ��� false
                if (isSecond) isSecond = false; // �ι�° �ֹ��� false
                if (!isPistol) isPistol = true; // �������� true

                if (isPistol) myAnim.SetTrigger("GetPistol");

                if(!myAnim.GetBool("isPistol")) myAnim.SetBool("isPistol", true);

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // ���� ���°� false �� ��� true
            }
        }

        ///<summary> ��� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isFirst || isSecond || isPistol)
            {
                if (HandSorket.GetComponentInChildren<WeaponItem>() != null && MotionEnd)
                {
                    if (isFirst)
                    {
                        myAnim.SetTrigger("PutGun");
                    }
                    else if (isSecond)
                    {
                        myAnim.SetTrigger("PutGun");
                    }
                    else if (isPistol)
                    {
                        myAnim.SetTrigger("PutPistol");
                    }

                    if (Armed) Armed = false;
                    myAnim.SetBool("isAiming", false);
                    myAnim.SetBool("isArmed", false);
                }
            }
        }
        #endregion

        #region ���� ���� Input Methods

        ///<summary> ���� ���� Input �޼��� </summary>
        if (myAnim.GetBool("isArmed") && Input.GetMouseButton(1))
        {
            Aimed = true;
            myAnim.SetBool("isAiming", true);
        }

        ///<summary> ���� ���� ���� Input �޼��� </summary>
        if (myAnim.GetBool("isArmed") && Input.GetMouseButtonUp(1))
        {
            Aimed = false;
            myAnim.SetBool("isAiming", false);

        }
        #endregion

        #region UI Input Methods
        if (Input.GetKeyDown(KeyCode.Tab) && myState != STATE.DEAD)
        {
            if (ActiveInv)
            {
                _Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(1100.0f, 0.0f);
            }
            else
            {
                _Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            }
            ActiveInv = !ActiveInv;
        }

        #endregion
    }
    #endregion

    /***********************************************************************
    *                       Private BattleSystem Methods
    ***********************************************************************/
    #region Private BattleSystem �Լ�
    private void OnAttack()
    {
        //base.BulletRotate(bulletRotate);
    }
    private void Fire()
    {
        Instantiate(Resources.Load("Effect/WFX_MF FPS RIFLE1"), myWeapon.GetComponent<WeaponItem>().MuzzlePoint.position, myWeapon.GetComponent<WeaponItem>().MuzzlePoint.rotation);
        GameObject bullet = Instantiate(myWeapon.GetComponent<WeaponItem>().WeaponData.Bullet, myWeapon.GetComponent<WeaponItem>().MuzzlePoint.position, myWeapon.GetComponent<WeaponItem>().MuzzlePoint.rotation);

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

    /***********************************************************************
    *                               Corutine
    ***********************************************************************/
    #region �ڷ�ƾ
    IEnumerator AliveCycle()
    {
        while (Stat.HP > Mathf.Epsilon)
        {
            // ����� ��ġ 0�϶�
            while(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty > Mathf.Epsilon)
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
            while(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty <= Mathf.Epsilon)
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
