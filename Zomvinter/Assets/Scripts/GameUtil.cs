using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary> ȸ�� ���� ���� ����ü </summary>
public struct ROTATEDATA
{
    public float Angle;
    public float Dir;
}

/// <summary> ���� Ÿ�� ���� ����ü </summary>
public struct time
{
    public int day;
    public float Gametime;
}
public class GameUtil : MonoBehaviour, IDamageable
{
    /// <summary> ȸ�� ���� �Լ� </summary>
    /// <param name="src">ȸ�� ���� ����</param>
    /// <param name="des">ȸ�� �� ����</param>
    /// <param name="right">�ش� ������Ʈ�� Vector.Right��</param>
    /// <param name="data">���� �� ���� ��</param>
    public static void CalcAngle(Vector3 src, Vector3 des, Vector3 right, out ROTATEDATA data)
    {
        data = new ROTATEDATA();
        float Radian = Mathf.Acos(Vector3.Dot(src, des));
        //�����̼� ��
        data.Angle = 180.0f * (Radian / Mathf.PI);
        //ȸ���� ��, ����� ��
        data.Dir = 1.0f;
        if (Vector3.Dot(right, des) < 0.0f)
        {
            data.Dir = -1.0f;
        }
    }
    public void TakeHit(float damage,RaycastHit hit)
    {

    }
}

/// <summary> ���� �ý��� �������̽� </summary>
public interface BattleSystem
{
    void OnDamage(float Damage);
    void OnCritDamage(float CritDamage);
    bool IsLive();
    Transform transform { get; }

}