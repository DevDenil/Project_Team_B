using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Character
{
    public Transform player;
    public float CharMoveSpeed = 5.0f;
    public float CharJumpForce = 200.0f;
    new Rigidbody rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        Rotating();
    }
    
    void Moving()
    {
        Vector3 pos = Vector3.zero;
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");
        myAnim.SetFloat("pos.x", pos.x);
        myAnim.SetFloat("pos.z", pos.z);
        this.transform.Translate(pos * CharMoveSpeed * Time.deltaTime);
    }
    void Rotating()
    {
        Vector3 mPosition = Input.mousePosition; //���콺��ǥ
        Vector3 oPosition = this.transform.position; //������Ʈ��ǥ

        mPosition.y = oPosition.y - Camera.main.transform.position.y; 

        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);
        float dirz = target.z - oPosition.z;
        float dirx = target.x - oPosition.x;
        float rotateDegree = Mathf.Atan2(dirz, dirx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, rotateDegree, 0.0f);
    }
}
