using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//LJM
public class ZMoveController : MonoBehaviour
{
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                _anim = GetComponentInChildren<Animator>();
            }
            return _anim;
        }
    }
    /*-----------------------------------------------------------------------------------------------*/

    private float Angle;
    private float Dir;
    private float AttTime = 0.0f;

    /*-----------------------------------------------------------------------------------------------*/
    void Start()
    {
       
    }

    void Update()
    {
       
    }

    /*-----------------------------------------------------------------------------------------------*/
    //상속 함수
    protected void MoveToPosition(Transform Target,float MoveSpeed, float AttRange, float AttDelay, float AttSpeed, float TurnSpeed)
    {
        //myAnim.SetBool("IsMoving", true);
        if (MoveRoutine != null) StopCoroutine(MoveRoutine);
        MoveRoutine = StartCoroutine(Chasing(Target.position, MoveSpeed, AttRange, AttDelay, AttSpeed));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(Target.position, TurnSpeed));
    }

    /*-----------------------------------------------------------------------------------------------*/
    //이동 코루틴
    Coroutine MoveRoutine = null;
    protected IEnumerator Chasing(Vector3 pos,float MoveSpeed, float AttackRange, float AttackDelay, float AttackSpeed)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();

        //While 조건문으로 Aggro 해제 조건 삽입
        while (true)
        {
            //공격 거리 유지
            if (Dist > AttackRange)
            {
                myAnim.SetBool("isMoving", true);
                float delta = MoveSpeed * Time.deltaTime;

                if (Dist < delta)
                {
                    delta = Dist;
                }
                
                this.transform.Translate(Dir * delta, Space.World);
                Dist -= delta;
            }
            else
            {
                myAnim.SetBool("isMoving", false);
                AttTime += Time.deltaTime;
                if (AttackDelay <= AttTime)
                {
                    //Debug.Log(AttackTime);
                    /*if(랜덤 난수)
                     * {
                     * 크리티컬 확률 Anim
                     * }
                     * else 
                     * {
                     * 일반 공격 확률 Anim
                     * }
                     */

                    myAnim.SetTrigger("Attack");
                    AttTime = 0.0f;
                }
            }
            yield return null;
        }
    }
    /*-----------------------------------------------------------------------------------------------*/
    //회전 코루틴
    private void CalcAngle(Vector3 src, Vector3 des, Vector3 right)
    {
        float Radian = Mathf.Acos(Vector3.Dot(src, des));
        //로테이션 값
        Angle = 180.0f * (Radian / Mathf.PI);
        //회전의 좌, 우방향 값
        Dir = 1.0f;
        if (Vector3.Dot(right, des) < 0.0f)
        {
            Dir = -1.0f;
        }
    }

    Coroutine RotRoutine = null;
    protected IEnumerator Rotating(Vector3 pos, float TurnSpeed)
    {
        //지점 방향 벡터
        Vector3 _Dir = (pos - this.transform.position).normalized;
        CalcAngle(this.transform.forward, _Dir, this.transform.right);

        while (Angle > Mathf.Epsilon)
        {
            float delta = TurnSpeed * Time.deltaTime;
            delta = delta > Angle ? Angle : delta;

            this.transform.Rotate(Vector3.up * delta * Dir);

            Angle -= delta;
            yield return null;
        }
        RotRoutine = null;
    }
    /*-----------------------------------------------------------------------------------------------*/
}
