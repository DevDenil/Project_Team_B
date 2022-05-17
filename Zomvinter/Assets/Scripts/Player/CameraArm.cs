using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField]
    private float CameraMoveSpeed;
    public Transform Target;
    public Transform PlayerCam;
    public Transform PlayerNightLight;
    Vector3 TargetRot;
    float TargetZoomDist = 0.0f;
    float TargetLightZoomDist = 0.0f;
    public float ZoomDist = 0.0f;
    public float LightZoomDist = 0.0f;
    public Vector2 ZoomRange;
    public float ZoomSpeed = 10.0f;
    public float RotSpeed;
    float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    { 
        time = Time.deltaTime + RotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            TargetRot.y -= 5.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            TargetRot.y += 5.0f;
        }
        if(TargetRot.y > 180.0f || TargetRot.y < -180.0f)
        {
            
        }
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(TargetRot), time);
        
        TargetZoomDist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        TargetZoomDist = Mathf.Clamp(TargetZoomDist, ZoomRange.x, ZoomRange.y);
        ZoomDist = Mathf.Lerp(ZoomDist, TargetZoomDist, Time.deltaTime * ZoomSpeed);
        
        //Ä«¸Þ¶ó ÁÜ¿¡ µû¸¥ Àú³á ½Ã¾ß ÁÜÀÎ¾Æ¿ô
        TargetLightZoomDist += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        TargetLightZoomDist = Mathf.Clamp(TargetLightZoomDist, -6.0f, Mathf.Epsilon);
        LightZoomDist = Mathf.Lerp(LightZoomDist, TargetLightZoomDist, Time.deltaTime * ZoomSpeed);
        
        //Ä«¸Þ¶ó ÁÜÀÎ¾Æ¿ô
        PlayerCam.localPosition = new Vector3(0.0f, -ZoomDist, -1.0f);
        //Àú³á ½Ã¾ß ÁÜÀÎ¾Æ¿ô
        PlayerNightLight.localPosition = new Vector3(0.0f, 3.0f, LightZoomDist);
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * CameraMoveSpeed);
    }
}
