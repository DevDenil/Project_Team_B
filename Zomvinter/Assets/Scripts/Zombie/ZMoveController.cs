using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
            return _anim;
        }
    }
    /*-----------------------------------------------------------------------------------------------*/

    private float Angle;
    private float Dir;

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
        // myAnim.SetBool("IsMoving", true);
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
        float AttackTime = AttackDelay;
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();

        if (!myAnim.GetBool("isMoving")) myAnim.SetBool("isMoving", true);
        //While ���ǹ����� Aggro ���� ���� ����
        while (true)
        {
            Debug.Log("isOn");
            //���� �Ÿ� ����
            if (Dist > AttackRange)
            {
                float delta = MoveSpeed * Time.deltaTime;

                if (Dist < delta)
                {
                    delta = Dist;
                }
                
                Debug.Log("asd");
                this.transform.Translate(Dir * delta, Space.World);
                Dist -= delta;
            }
            else
            {
                AttackTime += Time.deltaTime;
                if (myAnim.GetBool("isMoving")) myAnim.SetBool("isMoving", false);
                //myAnim.SetBool("isMoving", false);
                if (AttackTime >= AttackDelay)
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

                    //Anim Set
                    AttackTime = 0.0f;
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
