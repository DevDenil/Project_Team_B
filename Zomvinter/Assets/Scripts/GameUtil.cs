using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary> 회전 연산 변수 구조체 </summary>
public struct ROTATEDATA
{
    public float Angle;
    public float Dir;
}

/// <summary> 월드 타임 변수 구조체 </summary>
public struct time
{
    public int day;
    public float Gametime;
}
public class GameUtil : MonoBehaviour, IDamageable
{
    /// <summary> 회전 연산 함수 </summary>
    /// <param name="src">회전 시작 지점</param>
    /// <param name="des">회전 끝 지점</param>
    /// <param name="right">해당 오브젝트의 Vector.Right값</param>
    /// <param name="data">연산 후 리턴 값</param>
    public static void CalcAngle(Vector3 src, Vector3 des, Vector3 right, out ROTATEDATA data)
    {
        data = new ROTATEDATA();
        float Radian = Mathf.Acos(Vector3.Dot(src, des));
        //로테이션 값
        data.Angle = 180.0f * (Radian / Mathf.PI);
        //회전의 좌, 우방향 값
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

/// <summary> 전투 시스템 인터페이스 </summary>
public interface BattleSystem
{
    void OnDamage(float Damage);
    void OnCritDamage(float CritDamage);
    bool IsLive();
    Transform transform { get; }

}