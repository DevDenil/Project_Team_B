using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLight : MonoBehaviour
{
    [SerializeField]
    private float TimeScale; // �¾� ȸ����
    public GameObject NightLight; // ���� �þ� ������Ʈ
    CameraArm ZoomDist;


    private float nightFogDensity = 0.12f; // ���� �Ȱ� ��ġ ���ѷ�
    public float FogDensity;
    public float currentFogDensity; // ���� �Ȱ� ��ġ
    public float Intensity; // ���� �þ� ���
    public float ZoomIntensity; // ī�޶� �ܿ� ���� ��� ����

    public bool IsNight = default; // �� �� ����
    // Start is called before the first frame update
    void Start()
    {
        currentFogDensity = Mathf.Epsilon;
        Intensity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    { 
        Intensity = Mathf.Clamp(Intensity, 0.0f, 25.0f);
        NightLight.GetComponentInChildren<Light>().intensity = Intensity;
        this.transform.Rotate(Vector3.right, TimeScale * Time.deltaTime);

        if(this.transform.eulerAngles.x >= 170.0f)
        {
            IsNight = true;
        }
        else if(this.transform.eulerAngles.x <= 2.0f)
        {
            
            IsNight = false;
        }

        if(IsNight && Intensity < 35.0f)
        {
            Intensity += 0.6f * Time.deltaTime;
        }
        else if(!IsNight && Intensity > 0.0f)
        {
            Intensity -= 0.6f * Time.deltaTime;
        }
     
        if (IsNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.01f * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
                Intensity += 1.0f * Time.deltaTime;
            }
        }
        else
        {
            if (currentFogDensity >= Mathf.Epsilon)
            {
                currentFogDensity -= 0.01f * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
                Intensity -= 1.0f * Time.deltaTime;
            }
        }
        
        
    }
}
