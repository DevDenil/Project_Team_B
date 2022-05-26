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
    private static CharacterStat Stat;
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
    // ī�޶� �� ��
    private Transform _cameraArm;
    [SerializeField]
    // �� ��ġ
    private Transform _model;
    [SerializeField]
    private bool Aimed = true;
    #endregion

    #region �̵� ���� ����
    #endregion

    #region ���� ���� ��� - ����
    public STATE myState = STATE.NONE;
    #endregion

    #region �ڷ�ƾ ����
    private Coroutine aliveCycle = null;
    #endregion

    [SerializeField]
    public Transform HandSorket;
    [SerializeField]
    Transform BackLeftSorket;
    [SerializeField]
    Transform BackRightSorket;

    //ĳ���� �� �Ʒ�����
    Transform mySpine;

    public bool isFirst = true;
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
        // ����ü ���� ��äȭ
        Stat = new CharacterStat();
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
        Debug.Log(pos.x);
        Debug.Log(pos.z);
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
                InitStat();
                DynamicStatClamp();
                ChangeState(STATE.ALIVE);
                break;
            case STATE.ALIVE:
                StackWeapon();
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
    private void DynamicStatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }
    private void InitStat()
    {
        //�ʱ� ĳ���� ��ġ
        Stat.HP = Stat.MaxHP;
        Stat.AP = 0.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.TurnSpeed = 180.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        // �ʱ� ĳ���� �ִ� ��ġ
        Stat.CycleSpeed = 1.0f;
        Stat.MaxHP = 100.0f; // �ִ� ü��
        Stat.MaxHunger = 100.0f; // �ִ� ��ⷮ
        Stat.MaxStamina = 50.0f; // �ִ� ���׹̳�
        Stat.MaxThirsty = 100.0f; // �ִ� ���� ��ġ

        //�ʱ� ĳ���� �ɷ�ġ ����
        Stat.Strength = 0;
        Stat.Cadio = 0;
        Stat.Handicraft = 0;
        Stat.Agility = 0;
        Stat.Intellect = 0;
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
        if(Physics.Raycast(mousePos, out RaycastHit hit, 1 << LayerMask.NameToLayer("Ground")))
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

    private void StackWeapon()
    {
        if(_Inventory.PrimaryItems[0] != null)
        {
            GameObject gun1 = Instantiate(_Inventory.PrimaryItems[0].ItemPrefab, BackLeftSorket);
            Destroy(gun1.GetComponent<Rigidbody>());
            Destroy(gun1.GetComponent<BoxCollider>());
            gun1.transform.parent = BackLeftSorket.transform;
            //StartCoroutine(UpdateEquipment(gun1, BackLeftSorket));
        }
        if(_Inventory.PrimaryItems[1] != null)
        {
            GameObject gun2 = Instantiate(_Inventory.PrimaryItems[1].ItemPrefab, BackRightSorket);
            Destroy(gun2.GetComponent<Rigidbody>());
            Destroy(gun2.GetComponent<BoxCollider>());
            gun2.transform.parent = BackRightSorket.transform;
            //StartCoroutine(UpdateEquipment(gun2, BackRightSorket));
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

    #endregion

    /***********************************************************************
    *                               Input Methods
    ***********************************************************************/
    #region Input �Լ�
    public void GetGun(int index)
    {
        GameObject Gun = Instantiate(_Inventory.PrimaryItems[index].ItemPrefab, HandSorket.position, HandSorket.rotation);
        Gun.transform.parent = HandSorket.transform;
        Destroy(Gun.GetComponent<Rigidbody>());
        Destroy(Gun.GetComponent<BoxCollider>());
        //StartCoroutine(UpdateEquipment(Gun, HandSorket));
    }

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
            myAnim.SetTrigger("Reload");

            //// ������ ����
        }

        ///<summary> �ֹ��� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isFirst = true;
            // 1. 2�� ���Կ� �������� �ִ� ���
            if (_Inventory.PrimaryItems[0] != null)
            {
                // 1-1. �������� ��� ���� ���
                if (!myAnim.GetBool("isArmed"))
                {
                    myAnim.SetTrigger("GetGun");
                    myAnim.SetBool("isArmed", true);
                    if (BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject != null)
                    {
                        Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject);
                    }
                }
                // 1-2. �������� ��� �ִ� ���
                else
                {
                    myAnim.SetTrigger("GetGun");
                    myAnim.SetBool("isArmed", true);
                    if (BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject != null)
                    {
                        Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject);

                        GameObject gun2 = Instantiate(_Inventory.PrimaryItems[1].ItemPrefab, BackRightSorket);
                        Destroy(gun2.GetComponent<Rigidbody>());
                        Destroy(gun2.GetComponent<BoxCollider>());
                        gun2.transform.parent = BackRightSorket.transform;
                    }
                }
            }

            // ���� ���� �ִϸ��̼� ����
            myAnim.SetLayerWeight(1, 1.0f);
        }
        ///<summary> �ֹ��� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isFirst = false;
            // 1. 2�� ���Կ� �������� �ִ� ���
            if (_Inventory.PrimaryItems[1] != null)
            {
                // 1-1. �������� ��� ���� ���
                if (!myAnim.GetBool("isArmed"))
                {
                    myAnim.SetTrigger("GetGun");
                    myAnim.SetBool("isArmed", true);
                    if (BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject != null)
                    {
                        Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject);
                    }
                }
                // 1-2. �������� ��� �ִ� ���
                else
                {
                    myAnim.SetTrigger("GetGun");
                    myAnim.SetBool("isArmed", true);
                    if (BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject != null)
                    {
                        Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject);

                        GameObject gun1 = Instantiate(_Inventory.PrimaryItems[1].ItemPrefab, BackLeftSorket);
                        Destroy(gun1.GetComponent<Rigidbody>());
                        Destroy(gun1.GetComponent<BoxCollider>());
                        gun1.transform.parent = BackLeftSorket.transform;
                    }
                }
            }

            // ���� ���� �ִϸ��̼� ����
            myAnim.SetLayerWeight(1, 1.0f);
        }
        ///<summary> �������� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 1. �������� ��� ���� ���
            if (!myAnim.GetBool("isArmed") && _Inventory.SecondaryItems != null)
            {
                // �ǽ��� �̴� �ִϸ��̼�
                // �ǽ��� ���� ����
            }
            // 2. �������� ��� �ִ� ���
            else
            {
                // �ǽ��� �̴� �ִϸ��̼�
                // �ǽ��� ���� ����
            }

            // ���� ���� �ִϸ��̼� ����
            myAnim.SetLayerWeight(1, 1.0f);
        }

        ///<summary> ��� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(myAnim.GetBool("isArmed"))
            {
                myAnim.SetLayerWeight(1, 0.0f);
                myAnim.SetTrigger("PutGun");
                myAnim.SetBool("isArmed", false);
                myAnim.SetBool("IsAiming", false);
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
        //Instantiate(bullet, bulletStart.position, bulletRotate.rotation);
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
