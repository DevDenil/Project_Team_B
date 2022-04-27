using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    public Transform PlayerCam;
    Vector3 TargetRot;
    float TargetZoomDist = 0.0f;
    float ZoomDist = 0.0f;
    public Vector2 ZoomRange;
    public float ZoomSpeed = 10.0f;
    public float RotSpeed;
    float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        TargetRot = this.transform.rotation.eulerAngles;
        time = Time.deltaTime + RotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            TargetRot.y += 5.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            TargetRot.y -= 5.0f;
        }
        if(TargetRot.y > 180.0f || TargetRot.y < -180.0f)
        {
            
        }
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(TargetRot), time);
        TargetZoomDist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        TargetZoomDist = Mathf.Clamp(TargetZoomDist, ZoomRange.x, ZoomRange.y);
        ZoomDist = Mathf.Lerp(ZoomDist, TargetZoomDist, Time.deltaTime * ZoomSpeed);
        PlayerCam.localPosition = new Vector3(0.0f, 0.0f, ZoomDist);
    }
}
