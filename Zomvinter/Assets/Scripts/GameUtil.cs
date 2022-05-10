using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct ROTATEDATA
{
    public float Angle;
    public float Dir;
}

public struct time
{
    public static int day;
    public static float Gametime;
}
public class GameUtil : MonoBehaviour, IDamageable
{
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
public interface BattleSystem
{
    void OnDamage(float Damage);
    void OnCritDamage(float CritDamage);
    bool IsLive();
    Transform transform { get; }

}