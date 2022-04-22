using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : Character, IDamageable, BattleSystem
{
    public float damage = 5.0f; 
    public LayerMask collisionMask;
    public GameObject bullet;
    public float bulletSpeed = 50.0f;
    
    // Start is called before the first frame update
    void Start()
    {   
        GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
        StartCoroutine(bulletDestroy(3.0f));
    }
    void Update()
    {
        float moveDist = bulletSpeed * Time.deltaTime;
        CheckCollision(moveDist);
    }
    void OnAttack()
    {

    }
    void CheckCollision(float moveDist)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit , moveDist, collisionMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hit");
            BattleSystem bs = hit.collider.GetComponent<BattleSystem>();
            bs.OnDamage(damage);
        }
    }
    

    public void TakeHit(float damage, RaycastHit hit)
    {
        
    }
    public void OnDamage(float Damage)
    {

    }
    public void OnCritDamage(float CritDamage)
    {

    }
    public bool IsLive()
    {
        return true;
    }

    IEnumerator bulletDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
