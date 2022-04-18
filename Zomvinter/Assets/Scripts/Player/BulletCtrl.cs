using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 1000.0f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
    }
}
