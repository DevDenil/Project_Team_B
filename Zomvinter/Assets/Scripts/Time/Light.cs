using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public float TimeScale; // 태양 회전량

    private float nightFogDensity = 0.1f; // 저녁 안개 수치 제한량
    public float FogDensity;
    public float currentFogDensity; // 현재 안개 수치

    public bool IsNight = default; // 밤 낮 구분
    // Start is called before the first frame update
    void Start()
    {
        currentFogDensity = Mathf.Epsilon;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.right, TimeScale * Time.deltaTime);

        if(this.transform.eulerAngles.x >= 170.0f)
        {
            IsNight = true;
        }
        else if(this.transform.eulerAngles.x <= 2.0f)
        {
            IsNight = false;
        }

        if(IsNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * 2 * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= Mathf.Epsilon)
            {
                currentFogDensity -= 0.1f * 2 * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
