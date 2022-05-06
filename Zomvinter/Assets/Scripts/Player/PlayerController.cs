using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //float Rot;
    public Transform RotPoint;
    Animator _anim = null;
    //bool isHaveGun = false;
    Quaternion myRot; //이동했을 때의 rotation값 저장
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
        //구면 선형보간 , 내 로테이션값을 구면 선형보간하여 부드럽게 돌아가게 함
        if(!Mathf.Approximately(pos.magnitude, 0.0f))
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,Quaternion.LookRotation(pos),Time.deltaTime*10.0f);
        
        //myRot = this.transform.rotation;
        //StartCoroutine(CheckLog(myRot));
        saveRotate = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(pos), Time.deltaTime * 10.0f);
    }
    IEnumerator CheckLog(Quaternion context)
    {
        Debug.Log("저장값"+context);
        yield return new WaitForSeconds(3.0f);
        
    }
    protected void KeyRotate(Transform tr)
    {

    }
    
    protected void Rotate(Transform RotatePoint)
    {
        
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //마우스 위치를 카메라레이를 이용해 카메라에서 스크린의 점을 통해 반환
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        //월드좌표로 하늘 방향에 크기가 1인 백터와 원점을 갖음
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength)) //레이가 평면과 교차했는지 파악
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength); //레이렝스거리에 위치값 반환
            /*transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
            //위에서 구한 pointTolook 위치값을 캐릭터가 보게함*/
            Vector3 dir = pointTolook - this.transform.position; //뱡향 구하기, 방향 벡터값 = 목표벡터 - 시작벡터
            dir.y = 0f;
            Quaternion rot = Quaternion.LookRotation(dir.normalized); //방향의 쿼터니언 값 구하기, 쿼너티언 값 = 쿼너티언 방향 값(방향 벡터)
            RotatePoint.transform.rotation = rot; //방향 돌리기
            //Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10.0f, Color.red, 0.1f);
            // ?방향 초기화 시킬 방법  ?견착중일 때 어떻게 각도 제한 할것인지 ?방향전환때 부드럽게 하는 방법v
            // 집!걷는 애니메이션 추가, 걷는 애니메이션 사용할 때 이동속도 제한하기? 달릴때 이동속도 증가하기? 
            // 집!평소에 걷는 애니메이션으로 할당, 쉬프트 키 누르면 달리기 구현, 견착중에 달리기 비활성화 시키기 
        }
    }

    protected void BulletRotate(Transform RotatePoint)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //마우스 위치를 카메라레이를 이용해 카메라에서 스크린의 점을 통해 반환
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        //월드좌표로 하늘 방향에 크기가 1인 백터와 원점을 갖음
        if (GroupPlane.Raycast(cameraRay, out float rayLength)) //레이가 평면과 교차했는지 파악
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength); //레이렝스거리에 위치값 반환
            /*transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
            //위에서 구한 pointTolook 위치값을 캐릭터가 보게함*/
            Vector3 dir = pointTolook - this.transform.position; //뱡향 구하기, 방향 벡터값 = 목표벡터 - 시작벡터
            Quaternion rot = Quaternion.LookRotation(dir.normalized); //방향의 쿼터니언 값 구하기, 쿼너티언 값 = 쿼너티언 방향 값(방향 벡터)
            
            RotatePoint.transform.rotation = rot; //방향 돌리기
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10.0f, Color.red, 0.1f);
        }
    }
    protected void StartAiming()
    {
        saveRotate = this.transform.rotation;
        Debug.Log("로테이션 저장 " + saveRotate);

    }
    protected void StopAiming()
    {
        //myRot = Quaternion.Slerp(this.transform.rotation, myRot, Time.deltaTime * 10.0f);
        this.transform.rotation = saveRotate;
        Debug.Log("로테이션 복귀 " + saveRotate);
    }

    /*-----------------------------------------------------------------------------------------------*/

    //대각선이동(애니메이션) , rotating lookAt()
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
    //마우스 위치를 카메라레이를 이용해 카메라에서 스크린의 점을 통해 반환
    //    Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
    //월드좌표로 하늘 방향에 크기가 1인 백터와 원점을 갖음
    //    float rayLength;
    //    if (GroupPlane.Raycast(cameraRay, out rayLength)) //레이가 평면과 교차했는지 파악
    //    {
    //        Vector3 pointTolook = cameraRay.GetPoint(rayLength); //레이렝스거리에 위치값 반환
    /*transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
    //위에서 구한 pointTolook 위치값을 캐릭터가 보게함*/
    //        Vector3 dir = pointTolook - this.transform.position; //뱡향 구하기, 방향 벡터값 = 목표벡터 - 시작벡터
    //        dir.y = 0f;
    //        Quaternion rot = Quaternion.LookRotation(dir.normalized); //방향의 쿼터니언 값 구하기, 쿼너티언 값 = 쿼너티언 방향 값(방향 벡터)
    //        RotatePoint.transform.rotation = rot; //방향 돌리기
    //    }
    //}

    /*-----------------------------------------------------------------------------------------------*/
}
