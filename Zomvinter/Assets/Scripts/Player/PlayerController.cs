using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //float Rot;
    public Transform RotPoint;
    Animator _anim = null;
    //bool isHaveGun = false;
    Quaternion myRot; //�̵����� ���� rotation�� ����
    Quaternion saveRotate;
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
    protected void Moving(Vector3 pos, float MoveSpeed, Transform tr)
    {
        float delta = MoveSpeed * Time.deltaTime;
        myAnim.SetFloat("pos.x", pos.x);
        myAnim.SetFloat("pos.z", pos.z);
        if (pos.magnitude > 1.0f)
        {
            pos.Normalize();
        }
        this.transform.Translate(pos * delta, Space.World);
        //this.transform.Rotate(transform.right);
        //this.transform.LookAt(this.transform.position + pos) ;
        //���� �������� , �� �����̼ǰ��� ���� ���������Ͽ� �ε巴�� ���ư��� ��
        if(!Mathf.Approximately(pos.magnitude, 0.0f))
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,Quaternion.LookRotation(pos),Time.deltaTime*10.0f);
        
        //myRot = this.transform.rotation;
        //StartCoroutine(CheckLog(myRot));
        saveRotate = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(pos), Time.deltaTime * 10.0f);
    }
    IEnumerator CheckLog(Quaternion context)
    {
        Debug.Log("���尪"+context);
        yield return new WaitForSeconds(3.0f);
        
    }
    protected void KeyRotate(Transform tr)
    {

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
            //Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10.0f, Color.red, 0.1f);
            // ?���� �ʱ�ȭ ��ų ���  ?�������� �� ��� ���� ���� �Ұ����� ?������ȯ�� �ε巴�� �ϴ� ���v
            // ��!�ȴ� �ִϸ��̼� �߰�, �ȴ� �ִϸ��̼� ����� �� �̵��ӵ� �����ϱ�? �޸��� �̵��ӵ� �����ϱ�? 
            // ��!��ҿ� �ȴ� �ִϸ��̼����� �Ҵ�, ����Ʈ Ű ������ �޸��� ����, �����߿� �޸��� ��Ȱ��ȭ ��Ű�� 
        }
    }

    protected void BulletRotate(Transform RotatePoint)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //���콺 ��ġ�� ī�޶��̸� �̿��� ī�޶󿡼� ��ũ���� ���� ���� ��ȯ
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        //������ǥ�� �ϴ� ���⿡ ũ�Ⱑ 1�� ���Ϳ� ������ ����
        if (GroupPlane.Raycast(cameraRay, out float rayLength)) //���̰� ���� �����ߴ��� �ľ�
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength); //���̷����Ÿ��� ��ġ�� ��ȯ
            /*transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
            //������ ���� pointTolook ��ġ���� ĳ���Ͱ� ������*/
            Vector3 dir = pointTolook - this.transform.position; //���� ���ϱ�, ���� ���Ͱ� = ��ǥ���� - ���ۺ���
            Quaternion rot = Quaternion.LookRotation(dir.normalized); //������ ���ʹϾ� �� ���ϱ�, ����Ƽ�� �� = ����Ƽ�� ���� ��(���� ����)
            
            RotatePoint.transform.rotation = rot; //���� ������
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10.0f, Color.red, 0.1f);
        }
    }
    protected void StartAiming()
    {
        saveRotate = this.transform.rotation;
        Debug.Log("�����̼� ���� " + saveRotate);

    }
    protected void StopAiming()
    {
        //myRot = Quaternion.Slerp(this.transform.rotation, myRot, Time.deltaTime * 10.0f);
        this.transform.rotation = saveRotate;
        Debug.Log("�����̼� ���� " + saveRotate);
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
