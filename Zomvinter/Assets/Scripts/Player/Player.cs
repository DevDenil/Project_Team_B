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
    [Header("플레이어 스탯")]
    [SerializeField]
    public static CharacterStat Stat;
    #endregion

    #region 인벤토리 UI 영역
    private Inventory _Inventory;
    private StatUI _statUI;

    private bool ActiveInv = true;
    private bool ActiveStat = true;
    #endregion

    #region 이동 벡터 영역
    /// <summary> 플레이어 이동 벡터 </summary>
    Vector3 pos = Vector3.zero;

    /// <summary> 크로스헤어 </summary>
    
    #endregion

    #region 바인딩 영역

    [Header("바인딩 필요")]
    [SerializeField]
    private Transform _canvas;
    [SerializeField]
    /// <summary> 카메라 조절 축 </summary>
    private Transform _cameraArm;
    #endregion

    #region 유한 상태 기계 - 상태
    [Header("상태 기계")]
    public STATE myState = STATE.NONE;
    #endregion

    #region 코루틴 영역
    // 생존 요소 코루틴
    private Coroutine aliveCycle = null;

    // 스테미너 사용 코루틴
    private Coroutine Use = null;
    // 스테미너 회복 코루틴
    private Coroutine Recovery = null;

    #endregion

    #region 무장 영역
    [Header("무장 위치 값")]
    [SerializeField]
    public GameObject myWeapon = null;
    [SerializeField]
    public GameObject myKnife = null;
    [SerializeField]
    private LayerMask EnemyMask;
    [SerializeField]
    public Transform HandSorket;
    [SerializeField]
    public Transform BackLeftSorket;
    [SerializeField]
    public Transform BackRightSorket;
    [SerializeField]
    public Transform PistolGrip;
    [SerializeField]
    public Transform KnifeGrip;

    [Header("플레이어 상태 Bool")]
    /// <summary> 무장 상태 체크 Bool </summary>
    // public bool Armed = false;
    /// <summary> 애니메이션 동작 체크 Bool </summary>
    public bool MotionEnd = true;

    [SerializeField]
    /// <summary> 첫번째 무장을 선택했는지 체크 Bool </summary>
    public bool isFirst = false;
    [SerializeField]
    /// <summary> 두번째 무장을 선택했는지 체크 Bool </summary>
    public bool isSecond = false;
    [SerializeField]
    /// <summary> 부 무장을 선택했는지 체크 Bool </summary>
    public bool isPistol = false;

    [SerializeField]
    /// <summary> 조준 상태 체크 Bool </summary>
    private bool Aimed = true;
    #endregion

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
        Stat = new CharacterStat(); // 구조체 변수 객채화
        InitScripts();
        ChangeState(STATE.CREATE);
    }

    void Update()
    {
        StateProcess();
    }

    private void FixedUpdate()
    {
        if (!myAnim.GetBool("isDead"))
        {
            Move(Stat.MoveSpeed);
        }
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
                InitStatClamp();
                InitStat();
                UpdateBackWeapon();
                GetComponentInChildren<PlayerAnimEvent>().OnAttackKnife += OnMeleeAttack;
                GetComponentInChildren<PlayerAnimEvent>().EndAttackKnife += EndMeleeAttack;
                ChangeState(STATE.ALIVE);
                break;
            case STATE.ALIVE:
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                myAnim.SetTrigger("Dead");
                myAnim.SetBool("isDead", true);
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
    private void InitStat()
    {
        // 초기 캐릭터 최대 수치
        Stat.CycleSpeed = 1.0f;
        Stat.MaxHP = 100.0f; // 최대 체력
        Stat.MaxHunger = 100.0f; // 최대 허기량
        Stat.MaxStamina = 100.0f; // 최대 스테미나
        Stat.MaxThirsty = 100.0f; // 최대 갈증 수치

        //초기 캐릭터 수치
        Stat.HP = Stat.MaxHP;
        Stat.AP = 10.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.TurnSpeed = 180.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        //초기 캐릭터 능력치 레벨
        Stat.Strength = 0;
        Stat.Constitution = 0;
        Stat.Dexterity = 0;
        Stat.Endurance = 0;
        Stat.Intelligence = 0;
    }

    /// <summary> 캐릭터 스탯 값의 최소 최대값 고정 </summary>
    private void InitStatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }

    /// <summary> 스크립트 바인딩 </summary>
    private void InitScripts()
    {
        _Inventory = _canvas.GetComponentInChildren<Inventory>();
        _statUI = _canvas.GetComponentInChildren<StatUI>();
    }
    #endregion

    /// <summary> 플레이어 이동 함수 </summary>
    void Move(float MoveSpeed)
    {
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");

        base.Moving(this.transform, pos, MoveSpeed, _cameraArm);

        base.Rotate(this.transform);
    }

    /// <summary> 생존 루틴 실행 </summary>
    void AliveCoroutine()
    {
        if (aliveCycle != null) return;
        aliveCycle = StartCoroutine(AliveCycle());
    }

    public void StatCalc()
    { 
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수
    /// <summary> 주무기 장착 </summary>
    public void GetGun(int index)
    {
        if(myWeapon != null) Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
        if(isFirst) Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거
        if(isSecond) Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거

        myWeapon = Instantiate(_Inventory.PrimaryItems[index].ItemPrefab, HandSorket.position, HandSorket.rotation); // Gun Object 생성
        myWeapon.transform.parent = HandSorket.transform; // Gun 오브젝트 소켓에 자식화
        myWeapon.layer = 0;
        Destroy(myWeapon.GetComponent<Rigidbody>()); // 충돌 가능성 있는 컴포넌트 삭제
        Destroy(myWeapon.GetComponent<BoxCollider>()); // 충돌 가능성 있는 컴포넌트 삭제
    }

    /// <summary> 보조무기 장착 </summary>
    public void GetPistol()
    {
        if (myWeapon != null) Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
        if (isFirst) Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거
        if (isSecond) Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // 꺼낼 등에 있는 아이템 제거

        myWeapon = Instantiate(_Inventory.SecondaryItems.ItemPrefab, HandSorket.position, HandSorket.rotation);
        myWeapon.transform.parent = HandSorket.transform;
        myWeapon.layer = 0;
        Destroy(myWeapon.GetComponent<Rigidbody>()); // 충돌 가능성 있는 컴포넌트 삭제
        Destroy(myWeapon.GetComponent<BoxCollider>()); // 충돌 가능성 있는 컴포넌트 삭제
    }

    /// <summary> 무기 장착 해제 </summary>
    public void PutGun()
    {
        Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // 손에 있는 아이템 제거
        myWeapon = null;
        isFirst = false;
        isSecond = false;
        isPistol = false;
        // Armed = false;
    }

    /// <summary> 무기 표시 업데이트 </summary>
    public void UpdateBackWeapon()
    {
        // 1. 주 무장 1번 슬롯에 장비가 있고 / 등 소켓 1번에 장비가 없고 / 첫번째 무기를 사용중이 아닌 경우
        if (_Inventory.PrimaryItems[0] != null && BackLeftSorket.GetComponentInChildren<WeaponItem>() == null && !isFirst)
        {
            GameObject gun1 = Instantiate(_Inventory.PrimaryItems[0].ItemPrefab, BackLeftSorket); //주무장 1에 있는 장비 생성
            Destroy(gun1.GetComponent<Rigidbody>()); // 충돌 가능성 있는 컴포넌트 삭제
            Destroy(gun1.GetComponent<BoxCollider>()); // 충돌 가능성 있는 컴포넌트 삭제
            gun1.layer = 0; // UI가 생성되지 않도록 레이어 변경
            gun1.transform.parent = BackLeftSorket.transform; // 할당된 등 소켓에 자식으로 대입
        }
        // 2. 주 무장 2번 슬롯에 장비가 있고 / 등 소켓 2번에 장비가 없고 / 첫번째 무기를 사용중인 경우
        if (_Inventory.PrimaryItems[1] != null && BackRightSorket.GetComponentInChildren<WeaponItem>() == null && !isSecond)
        {
            GameObject gun2 = Instantiate(_Inventory.PrimaryItems[1].ItemPrefab, BackRightSorket); //주무장 2에 있는 장비 생성
            Destroy(gun2.GetComponent<Rigidbody>()); // 충돌 가능성 있는 컴포넌트 삭제
            Destroy(gun2.GetComponent<BoxCollider>()); // 충돌 가능성 있는 컴포넌트 삭제
            gun2.layer = 0; // UI가 생성되지 않도록 레이어 변경
            gun2.transform.parent = BackRightSorket.transform; // 할당된 등 소켓에 자식으로 대입
        }
        if (_Inventory.SecondaryItems != null && PistolGrip.GetComponentInChildren<WeaponItem>() == null)
        {
            GameObject pistol = Instantiate(_Inventory.SecondaryItems.ItemPrefab, PistolGrip);
            Destroy(pistol.GetComponent<Rigidbody>()); // 충돌 가능성 있는 컴포넌트 삭제
            Destroy(pistol.GetComponent<BoxCollider>()); // 충돌 가능성 있는 컴포넌트 삭제
            pistol.layer = 0; // UI가 생성되지 않도록 레이어 변경
            pistol.transform.parent = PistolGrip.transform; // 할당된 허리 소켓에 자식으로 대입
        }
    }
    #endregion

    /***********************************************************************
    *                               Anim Methods
    ***********************************************************************/
    #region 애니메이션 메소드
    /// <summary> 레이어 구분을 위해 애니메이션 동작 시작 확인 </summary>
    public void AnimStart()
    {
        if (MotionEnd) MotionEnd = false;
        myAnim.SetLayerWeight(1, 1.0f);
    }

    /// <summary> 레이어 구분을 위해 애니메이션 동작 끝 확인 </summary>
    public void AnimEnd()
    {
        if (!MotionEnd) MotionEnd = true;
        myAnim.SetLayerWeight(1, 0.0f);
    }
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
            if (Stat.Stamina > 0.0f)
            {
                myAnim.SetBool("isRun", true);

                // 1. 무장 상태일 때 달리는 경우
                if (myAnim.GetBool("isArmed"))
                {
                    Stat.MoveSpeed = 2.5f;
                }
                // 2. 비 무장 상태일 때 달리는 경우
                else
                {
                    Stat.MoveSpeed = 3.0f;
                }

                StopCoroutine("RecoveryStamina");
                Recovery = null;
                if (Use != null) return;
                Use = StartCoroutine("UseStamina");
            }
        }

        ///<summary> 달리기 취소 Input 메서드 </summary>
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            myAnim.SetBool("isRun", false);

            // 1. 무장 상태일 때 걷는 경우
            if (myAnim.GetBool("isArmed"))
            {
                Stat.MoveSpeed = 1.5f;
            }
            // 2. 비 무장 상태일 때 걷는 경우
            else
            {
                Stat.MoveSpeed = 2.0f;
            }

            StopCoroutine("UseStamina");
            Use = null;
            if (Recovery != null) return;
            Recovery = StartCoroutine("RecoveryStamina");
        }
        #endregion

        #region 공격 Input Methods
        ///<summary> 사격 Input 메서드 </summary>
        if (Input.GetMouseButtonDown(0))
        {
            // 조준 상태 인 경우
            if(myAnim.GetBool("isAiming"))
            {
                Fire(myWeapon.GetComponent<WeaponItem>().WeaponData.Damage, Stat.AP);
            }
        }

        ///<summary> 근접 공격 Input 메서드 </summary>
        if (Input.GetKeyDown(KeyCode.V))
        {
            if(MotionEnd)
            {
                myKnife.transform.parent = HandSorket;
                myKnife.transform.position = HandSorket.position;
                myKnife.transform.rotation = HandSorket.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);
                myAnim.SetTrigger("MeleeAttack");
                //// 근접 공격 처리
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(MotionEnd) myAnim.SetTrigger("Reload");

            //// 재장전 동작
        }

        ///<summary> 첫번째 주무기 장비 전환 Input 메서드 </summary>
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 1. 주무기 1번 슬롯에 아이템이 있고 등 소켓의 1번 슬롯에 아이템이 있는 경우
            if (_Inventory.PrimaryItems[0] != null && !isFirst && MotionEnd)
            {
                // if (!Armed) Armed = true;
                if (!isFirst) isFirst = true; // 첫번째 주무기 true
                if (isSecond) isSecond = false; // 두번째 주무기 false
                if (isPistol) isPistol = false; // 보조무기 false

                if (isFirst) myAnim.SetTrigger("GetGun"); // 무장 애니메이션 재생

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // 무장 상태 체크
            }
        }
        ///<summary> 두번째 주무기 장비 전환 Input 메서드 </summary>
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // 1. 2번 슬롯에 아이템이 있는 경우
            if (_Inventory.PrimaryItems[1] != null && !isSecond && MotionEnd)
            {
                // if (!Armed) Armed = true;
                if (isFirst) isFirst = false; // 첫번째 주무기 false
                if (!isSecond) isSecond = true; // 두번째 주무기 true
                if (isPistol) isPistol = false; // 보조무기 false

                if (isSecond) myAnim.SetTrigger("GetGun"); // 무장 애니메이션 재생

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // 무장 상태가 false 인 경우 true
            }
        }
        ///<summary> 보조무기 장비 전환 Input 메서드 </summary>
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 1. 2번 슬롯에 아이템이 있는 경우
            if (_Inventory.SecondaryItems != null && !isPistol && MotionEnd)
            {
                // if (!Armed) Armed = true;
                if (isFirst) isFirst = false; // 첫번째 주무기 false
                if (isSecond) isSecond = false; // 두번째 주무기 false
                if (!isPistol) isPistol = true; // 보조무기 true

                if (isPistol) myAnim.SetTrigger("GetPistol");

                if(!myAnim.GetBool("isPistol")) myAnim.SetBool("isPistol", true);

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // 무장 상태가 false 인 경우 true
            }
        }

        ///<summary> 장비 해제 Input 메서드 </summary>
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

                    // if (Armed) Armed = false;
                    myAnim.SetBool("isAiming", false);
                    myAnim.SetBool("isArmed", false);
                }
            }
        }
        #endregion

        #region 조준 상태 Input Methods

        ///<summary> 조준 상태 Input 메서드 </summary>
        if (myAnim.GetBool("isArmed") && Input.GetMouseButton(1))
        {
            Aimed = true;
            myAnim.SetBool("isAiming", true);
        }

        ///<summary> 조준 상태 해제 Input 메서드 </summary>
        if (myAnim.GetBool("isArmed") && Input.GetMouseButtonUp(1))
        {
            Aimed = false;
            myAnim.SetBool("isAiming", false);

        }
        #endregion

        #region UI Input Methods
        ///<summary> 인벤토리 창 황성화/비활성화 </summary>
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ActiveInv)
            {
                _Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(1100.0f, 110.0f);
            }
            else
            {
                _Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 110.0f);
            }
            ActiveInv = !ActiveInv;
        }

        ///<summary> 플레이어 스탯 창 활성화/비활성화 </summary>
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(ActiveStat)
            {
                _statUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1100.0f, 110.0f);
            }
            else
            {
                _statUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 110.0f);
            }
            ActiveStat = !ActiveStat;
        }
        #endregion
    }
    #endregion

    /***********************************************************************
    *                       Private BattleSystem Methods
    ***********************************************************************/
    #region Private BattleSystem 함수
    private void OnMeleeAttack()
    {
        
        Collider[] list = Physics.OverlapSphere(myKnife.transform.position, 1.0f, EnemyMask);
        foreach(Collider col in list)
        {
            BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
            if (bs != null)
            {
                bs.OnDamage(Stat.AP);
            }
        }
    }

    private void EndMeleeAttack()
    {
        myKnife.transform.parent = KnifeGrip;
        myKnife.transform.position = KnifeGrip.position;
        myKnife.transform.rotation = KnifeGrip.rotation;
    }

    private void Fire(float WeaponAP, float PlayerAP)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 9999.0f))
        {
            // 두 지점의 방향 벡터
            Vector3 dir = hit.point - myWeapon.GetComponent<WeaponItem>().MuzzlePoint.position;
            // 방향 벡터의 각도
            Quaternion rot = Quaternion.LookRotation(dir.normalized);

            // 오브젝트 생성
            GameObject bullet = Instantiate(myWeapon.GetComponent<WeaponItem>().WeaponData.Bullet, myWeapon.GetComponent<WeaponItem>().MuzzlePoint.position, rot);

            // 부모 관계 null
            bullet.transform.parent = null;
            // forward 방향으로 이동하는 코루틴 호출
            bullet.GetComponent<AmmoItem>().Fire(WeaponAP, PlayerAP);
        }
    }
    #endregion

    /***********************************************************************
    *                       Public BattleSystem Methods
    ***********************************************************************/
    #region Public BattleSystem 함수

    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myAnim.SetTrigger("Hit");
        Stat.HP -= Damage;
        if (Stat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {
             // 피격 애니메이션
        }
    }
    public void OnCritDamage(float CritDamage)
    {
        if (myState == STATE.DEAD) return;
        Stat.HP -= CritDamage;
        myAnim.SetTrigger("Hit");
        if (Stat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {
            // 크리티컬 피격 애니메이션
        }
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

    /// <summary> 스테미너 소모 코루틴 </summary>
    IEnumerator UseStamina()
    {
        while (Stat.Stamina > 0.0f)
        {
            if (Stat.Stamina <= 0.0f)
            {
                Stat.Stamina = 0.0f;
            }
            Stat.Stamina -= 5.0f;
            Debug.Log(Stat.Stamina + " / " + Stat.MaxStamina);
            yield return new WaitForSeconds(1.0f);
        }
        Use = null;
    }

    /// <summary> 스테미너 회복 코루틴 </summary>
    IEnumerator RecoveryStamina()
    {
        yield return new WaitForSeconds(2.0f);
        while (Stat.Stamina < Stat.MaxStamina)
        {
            if (Stat.Stamina >= Stat.MaxStamina)
            {
                Stat.Stamina = 100.0f;
            }
            Stat.Stamina += 1.0f;
            Debug.Log(Stat.Stamina + " / " + Stat.MaxStamina);
            yield return new WaitForSeconds(1.0f);
        }
        Recovery = null;
    }
    #endregion
}
