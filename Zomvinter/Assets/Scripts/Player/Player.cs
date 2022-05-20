using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary> 유한 상태 기계 - 상태 목록 </summary>
public enum STATE
{
    NONE, CREATE, ALIVE, BATTLE, DEAD
}

public class Player : PlayerController, BattleSystem
{
    #region 플레이어 데이터 영역
    //스텟 구조체
    private static CharacterStat Stat;
    #endregion

    #region 인벤토리 UI 영역
    [SerializeField]
    private Transform _canvas;
    private Inventory _Inventory;
    private StatUI _statUI;

    private bool ActiveInv = true;
    #endregion

    #region 벡터 영역
    //이동 벡터
    Vector3 pos = Vector3.zero;
    [SerializeField]
    // 카메라 암 축
    private Transform _cameraArm;
    [SerializeField]
    private bool Aimed;
    #endregion

    #region 이동 벡터 영역
    #endregion

    #region 유한 상태 기계 - 상태
    public STATE myState = STATE.NONE;
    #endregion

    #region 코루틴 영역
    private Coroutine aliveCycle = null;
    #endregion

    [SerializeField]
    Transform HandSorket;
    [SerializeField]
    Transform BackLeftSorket;
    [SerializeField]
    Transform BackRightSorket;

    //캐릭터 위 아래보기
    Transform mySpine;


    //총알프리팹, 총알발사위치, 총알각도
    // public Transform bullet; public Transform bulletStart; public Transform bulletRotate;

    //총 획득시 생성하기 위한 boolcheck용 
    // public bool GunCheck1 = false;
    // public bool GunCheck2 = false;

    //총 획득시 생성되는 위치의 부모 설정
    // public Transform UnArmed1; public Transform UnArmed2; //플레이어 등 부분에 스폰되는 위치
    // public Transform UnArmedGun1; public Transform UnArmedGun2; // 플레이어가 획득한 무기의 prefab을 사용해야함 

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    /// <summary> 디버깅용 에디터 이벤트 메서드 (Awake나 Start에 동일한 코드를 작성 할 것) </summary>
    private void OnValidate()
    {
        
    }

    void Awake()
    {
        // 구조체 변수 객채화
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
        ///<summary> 이동 Input 메서드 </summary>
        Move(Stat.MoveSpeed);
    }
    #endregion

    /***********************************************************************
    *                               Finite-state machine
    ***********************************************************************/
    #region 유한 상태 기계
    /// <summary> 상태 기계 상태 변환 함수 </summary>
    /// <param name="s">상태</param>
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
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }
    /// <summary> 상태 기계 Update 함수 </summary>
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.CREATE:
                break;
            /// <summary> Player가 살아 있는 경우 </summary>
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
    #region Private 함수

    #region Init Methods
    /// <summary> 캐릭터 스탯 값의 최소 최대값 고정 </summary>
    private void DynamicStatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }
    private void InitStat()
    {
        //초기 캐릭터 수치
        Stat.HP = Stat.MaxHP;
        Stat.AP = 0.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.TurnSpeed = 180.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        // 초기 캐릭터 최대 수치
        Stat.CycleSpeed = 1.0f;
        Stat.MaxHP = 100.0f; // 최대 체력
        Stat.MaxHunger = 100.0f; // 최대 허기량
        Stat.MaxStamina = 50.0f; // 최대 스테미나
        Stat.MaxThirsty = 100.0f; // 최대 갈증 수치

        //초기 캐릭터 능력치 레벨
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
        base.Moving(pos, MoveSpeed, _cameraArm);

        if (!Aimed)
        {
            this.transform.rotation = Quaternion.Euler(0.0f, _cameraArm.eulerAngles.y, 0.0f);
        }
        else
        {
            Rotate(this.transform);
        }
    }
   
    private void GetWeapon() // 주무기 획득시 등에 스폰
    {
        //if (GunCheck1 == true)
        //    Instantiate(UnArmedGun1, UnArmed1);
        //if (GunCheck2 == true)
        //    Instantiate(UnArmedGun2, UnArmed2);
    }
    private void PutWeapon() // 주무기 버릴때 없앰
    {
        //if (GunCheck1 == false)
        //    if (UnArmedGun1 != null) Destroy(UnArmedGun1);
        //if (GunCheck2 == false)
        //    if (UnArmedGun2 != null) Destroy(UnArmedGun2);
    }

    private void AliveCoroutine()
    {
        if (aliveCycle != null) return;
        aliveCycle = StartCoroutine(AliveCycle());
    }

    private IEnumerator UpdateEquipment(GameObject obj)
    {
        while(myAnim.GetBool("isArmed"))
        {
            obj.transform.position = HandSorket.position;
            obj.transform.rotation = HandSorket.rotation;
            yield return null;
        }
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수

    #endregion

    /***********************************************************************
    *                               Input Methods
    ***********************************************************************/
    #region Input 함수
    private void InputMethods()
    {
        #region 이동 Input Methods
        ///<summary> 달리기 시작 Input 메서드 </summary>
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            myAnim.SetBool("IsRun", true); //달리기
        }

        ///<summary> 달리기 취소 Input 메서드 </summary>
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            myAnim.SetBool("IsRun", false);
        }
        #endregion

        #region 공격 Input Methods
        ///<summary> 사격 Input 메서드 </summary>
        if (Input.GetMouseButtonDown(0))
        {
            // 조준 상태 인 경우
            if(myAnim.GetBool("isAiming"))
            {
                Fire();
            }
            // 아닌 경우 - Null
        }

        ///<summary> 근접 공격 Input 메서드 </summary>
        if (Input.GetKeyDown(KeyCode.V))
        {
            myAnim.SetTrigger("MeleeAttack");
        }

        ///<summary> 장비 전환 Input 메서드 </summary>
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 1. 착용중인 장비가 없는 경우
            if (!myAnim.GetBool("isArmed") && _Inventory.PrimaryItems[0] != null)
            {
                myAnim.SetTrigger("GetGun");
                myAnim.SetBool("isArmed", true);
                // 아이템 소켓
                GameObject Gun = Instantiate(_Inventory.PrimaryItems[0].ItemPrefab, HandSorket.position, Quaternion.Euler(HandSorket.rotation.eulerAngles));
                StartCoroutine(UpdateEquipment(Gun));
            }
            // 2. 착용중인 장비가 있는 경우
            else
            {
                myAnim.SetTrigger("GetGun");
                // 아이템 스왑 & 소켓
            }

            // 무기 변경 애니메이션 실행
            myAnim.SetLayerWeight(1, 1.0f);
            myAnim.SetBool("IsGun", true);
        }

        ///<summary> 장비 해제 Input 메서드 </summary>
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

        #region 조준 상태 Input Methods

        ///<summary> 조준 상태 Input 메서드 </summary>
        if (myAnim.GetBool("IsGun") && Input.GetMouseButton(1))
        {
            Aimed = true;
            Move(Stat.MoveSpeed / 2);
            // Rotation(Stat.TurnSpeed);
            myAnim.SetBool("IsAiming", true);
        }

        ///<summary> 조준 상태 해제 Input 메서드 </summary>
        if (myAnim.GetBool("IsGun") && Input.GetMouseButtonUp(1))
        {
            Aimed = false;
            myAnim.SetBool("IsAiming", false);
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
    #region Private BattleSystem 함수
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
    #region Public BattleSystem 함수

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
    #region 코루틴
    IEnumerator AliveCycle()
    {
        while (Stat.HP > Mathf.Epsilon)
        {
            // 배고픔 수치 0일때
            while(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty > Mathf.Epsilon)
            {
                Debug.Log(Stat.Thirsty);
                Stat.HP -= Stat.CycleSpeed * 3;
                Stat.Thirsty -= Stat.CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // 갈증 수치 0일때
            while (Stat.Thirsty <= Mathf.Epsilon && Stat.Hunger > Mathf.Epsilon)
            {
                Stat.Hunger -= Stat.CycleSpeed;
                Stat.Stamina += Stat.StaminaCycle / 2;
                Stat.HP -= Stat.CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // 둘 다 0일때
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
