using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }
    void Rotate()
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
            Vector3 dir = pointTolook - transform.position; //���� ���ϱ�, ���� ���Ͱ� = ��ǥ���� - ���ۺ���
            dir.y = 0f;
            Quaternion rot = Quaternion.LookRotation(dir.normalized); //������ ���ʹϾ� �� ���ϱ�, ����Ƽ�� �� = ����Ƽ�� ���� ��(���� ����)
            transform.rotation = rot; //���� ������
        }
    }
}
