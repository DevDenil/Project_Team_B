using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : PlayerController, BattleSystem
{
    //스텟 구조체
    public static CharacterStat Stat;
    public Transform Cam;
    /*-----------------------------------------------------------------------------------------------*/
    //지역 변수
    public LayerMask BulletLayer;//총알 도착지
    private Vector3 dir; // 총알 각도 
    [SerializeField]
    private float CycleSpeed; // 허기, 갈증 감소 속도
    [SerializeField]
    private float StaminaCycle; // 스테미나 감소 속도
    //인벤토리
    public List<GameObject> myItems = new List<GameObject>();
    public GameObject myInventory;
    private bool ActiveInv = true;
    public GameObject myStatUI;
    //마우스 로테이트
    public Transform RotatePoint;
    //캐릭터 위 아래보기
    public Transform mySpine;
    //이동 벡터
    Vector3 pos = Vector3.zero;
    //총알프리팹, 총알발사위치, 총알각도
    public Transform bullet; public Transform bulletStart; public Transform bulletRotate;
    //총 획득시 생성하기 위한 boolcheck용 
    public bool GunCheck1 = false;
    public bool GunCheck2 = false;
    //총 획득시 생성되는 위치의 부모 설정
    public Transform UnArmed1; public Transform UnArmed2; //플레이어 등 부분에 스폰되는 위치
    public Transform UnArmedGun1; public Transform UnArmedGun2; // 플레이어가 획득한 무기의 prefab을 사용해야함 
    Coroutine aliveCycle = null;
    Coroutine move = null;
    /*-----------------------------------------------------------------------------------------------*/
    //Unity
    void Start()
    {
        RotatePoint = GetComponent<Transform>();
        CycleSpeed = 1.0f;
        Stat = new CharacterStat();
        Stat.MaxHP = 100.0f; // 최대 체력
        Stat.MaxHunger = 100.0f; // 최대 허기량
        Stat.MaxStamina = 50.0f; // 최대 스테미나
        Stat.MaxThirsty = 100.0f; // 최대 갈증 수치
        ChangeState(STATE.CREATE);
    }

    // 캐릭터 수치가 0보다 작아지지 않고 최대 수치보다 커지지 않게 고정하는 함수
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
        //보는 방향(y축) pos값 변경해야함
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
                    myAnim.SetBool("IsRun", true); //달리기
                }
                if (Input.GetKeyUp(KeyCode.LeftShift)) myAnim.SetBool("IsRun", false); //달리기끝
         
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
                    StartAiming(); //로테이션 값 저장
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
                //총 획득시 Guncheck true 만들기
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
    // 배틀 시스템
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
    void GetWeapon()//주무기 획득시 등에 스폰
    {
        if (GunCheck1 == true)
            Instantiate(UnArmedGun1, UnArmed1);
        if (GunCheck2 == true)
            Instantiate(UnArmedGun2, UnArmed2);
    }
    void PutWeapon()//주무기 버릴때 없앰
    {
        if (GunCheck1 == false)
            if (UnArmedGun1 != null) Destroy(UnArmedGun1);
        if (GunCheck2 == false)
            if (UnArmedGun2 != null) Destroy(UnArmedGun2);
    }

    void StatInitialize()
    {
        //초기 캐릭터 수치
        Stat.HP = Stat.MaxHP;
        Stat.AP = 0.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        //초기 캐릭터 능력치 레벨
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
            // 배고픔 수치 0일때
            while(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty > Mathf.Epsilon)
            {
                Debug.Log(Stat.Thirsty);
                Stat.HP -= CycleSpeed * 3;
                Stat.Thirsty -= CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // 갈증 수치 0일때
            while (Stat.Thirsty <= Mathf.Epsilon && Stat.Hunger > Mathf.Epsilon)
            {
                Stat.Hunger -= CycleSpeed;
                Stat.Stamina += StaminaCycle / 2;
                Stat.HP -= CycleSpeed;
                yield return new WaitForSecondsRealtime(1.0f);
            }
            // 둘 다 0일때
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
