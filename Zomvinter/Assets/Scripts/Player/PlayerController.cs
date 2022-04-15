using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    /*-----------------------------------------------------------------------------------------------*/
    protected void Moving(Vector3 pos, float MoveSpeed)
    {
        //Vector3 pos = Vector3.zero;
        float delta = MoveSpeed * Time.deltaTime;
        myAnim.SetFloat("pos.x", pos.x);
        myAnim.SetFloat("pos.z", pos.z);
        if (pos.magnitude > 1.0f)
        {
            pos.Normalize();
        }
        this.transform.Translate(pos * delta, Space.World);

    }

    protected void Rotate(Transform RotatePoint)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //���콺 ��ġ�� ī�޶��̸� �̿��� ī�޶󿡼� ��ũ���� ���� ���� ��ȯ
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        //������ǥ�� �ϴ� ���⿡ ũ�Ⱑ 1�� ���Ϳ� ������ ����
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength)) //���̰� ���� �����ߴ��� �ľ�
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength); //���̷����Ÿ��� ��ġ�� ��ȯ
            /*transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
            //������ ���� pointTolook ��ġ���� ĳ���Ͱ� ������*/
            Vector3 dir = pointTolook - this.transform.position; //���� ���ϱ�, ���� ���Ͱ� = ��ǥ���� - ���ۺ���
            dir.y = 0f;
            Quaternion rot = Quaternion.LookRotation(dir.normalized); //������ ���ʹϾ� �� ���ϱ�, ����Ƽ�� �� = ����Ƽ�� ���� ��(���� ����)
            RotatePoint.transform.rotation = rot; //���� ������
        }
    }
    /*-----------------------------------------------------------------------------------------------*/

    //�밢���̵�(�ִϸ��̼�) , rotating lookAt()
    /*
    void Moving()
    {
        Vector3 pos = Vector3.zero;
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");        
        myAnim.SetFloat("pos.x", pos.x);
        myAnim.SetFloat("pos.z", pos.z);
        //Debug.Log(pos.normalized);;
        if (pos.magnitude > 1.0f)
        {
            pos.Normalize();
        }
        this.transform.Translate(pos * CharMoveSpeed * Time.deltaTime);

    }
    */

    //public void Rotate(Transform RotatePoint)
    //{
    //    Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //���콺 ��ġ�� ī�޶��̸� �̿��� ī�޶󿡼� ��ũ���� ���� ���� ��ȯ
    //    Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
    //������ǥ�� �ϴ� ���⿡ ũ�Ⱑ 1�� ���Ϳ� ������ ����
    //    float rayLength;
    //    if (GroupPlane.Raycast(cameraRay, out rayLength)) //���̰� ���� �����ߴ��� �ľ�
    //    {
    //        Vector3 pointTolook = cameraRay.GetPoint(rayLength); //���̷����Ÿ��� ��ġ�� ��ȯ
    /*transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
    //������ ���� pointTolook ��ġ���� ĳ���Ͱ� ������*/
    //        Vector3 dir = pointTolook - this.transform.position; //���� ���ϱ�, ���� ���Ͱ� = ��ǥ���� - ���ۺ���
    //        dir.y = 0f;
    //        Quaternion rot = Quaternion.LookRotation(dir.normalized); //������ ���ʹϾ� �� ���ϱ�, ����Ƽ�� �� = ����Ƽ�� ���� ��(���� ����)
    //        RotatePoint.transform.rotation = rot; //���� ������
    //    }
    //}

    /*-----------------------------------------------------------------------------------------------*/
}
