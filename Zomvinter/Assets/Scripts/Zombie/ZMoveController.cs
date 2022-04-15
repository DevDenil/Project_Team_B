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
    //��� �Լ�
    protected void MoveToPosition(Transform Target,float MoveSpeed, float AttRange, float AttDelay, float AttSpeed, float TurnSpeed)
    {
        //myAnim.SetBool("IsMoving", true);
        if (MoveRoutine != null) StopCoroutine(MoveRoutine);
        MoveRoutine = StartCoroutine(Chasing(Target.position, MoveSpeed, AttRange, AttDelay, AttSpeed));
        if (RotRoutine != null) StopCoroutine(RotRoutine);
        RotRoutine = StartCoroutine(Rotating(Target.position, TurnSpeed));
    }

    /*-----------------------------------------------------------------------------------------------*/
    //�̵� �ڷ�ƾ
    Coroutine MoveRoutine = null;
    protected IEnumerator Chasing(Vector3 pos,float MoveSpeed, float AttackRange, float AttackDelay, float AttackSpeed)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();

        //While ���ǹ����� Aggro ���� ���� ����
        while (true)
        {
            //���� �Ÿ� ����
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
                    /*if(���� ����)
                     * {
                     * ũ��Ƽ�� Ȯ�� Anim
                     * }
                     * else 
                     * {
                     * �Ϲ� ���� Ȯ�� Anim
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
    //ȸ�� �ڷ�ƾ
    private void CalcAngle(Vector3 src, Vector3 des, Vector3 right)
    {
        float Radian = Mathf.Acos(Vector3.Dot(src, des));
        //�����̼� ��
        Angle = 180.0f * (Radian / Mathf.PI);
        //ȸ���� ��, ����� ��
        Dir = 1.0f;
        if (Vector3.Dot(right, des) < 0.0f)
        {
            Dir = -1.0f;
        }
    }

    Coroutine RotRoutine = null;
    protected IEnumerator Rotating(Vector3 pos, float TurnSpeed)
    {
        //���� ���� ����
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
