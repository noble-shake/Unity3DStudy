using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DayNightController : MonoBehaviour
{
    private Light lightDirectional;
    [SerializeField, Range(0f, 24f)] float time;

    public static DayNightController instance;

    [SerializeField] bool auto; // 밤낮 변화
    [SerializeField] bool isNight;
    [SerializeField] Material mat;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        Light[] lights  = GameObject.FindObjectsOfType<Light>();
        int count = lights.Length;
        for (int inum = 0; inum < count; ++inum)
        {
            Light light = lights[inum];
            if (light.type == LightType.Directional)
            { 
                lightDirectional = light;
                break;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        time %= 24;
        if (auto == true)
        {
            time += Time.deltaTime;
        }
        else
        {
            //밤(22)이면 어두운 특정시간, 낮(13)이면 밝은 특정시간
            if (isNight && time != 22f)
            {
                time += Time.deltaTime * 4.0f;  // 빠르게 밤이 되도록, 가속시킨다.
                if (time > 22f)
                {
                    time = 22f;
                }
            }

            if (!isNight && time != 13f)
            {
                time += Time.deltaTime * 4.0f;  // 빠르게 낮이 되도록, 가속시킨다.
                if (time < 22f && time > 13f)
                {
                    time = 13f;
                }
            }

        }
        checkIsNight();
        updateLighting();
    }

    private void checkIsNight()
    {
        if (time > 20 || (time > 0 && time < 6))
        {
            mat.EnableKeyword("_EMISSION");
        }
        else
        {
            mat.DisableKeyword("_EMISSION");
        }
    }

    private void updateLighting()
    {
        if (lightDirectional == null) return;

        float timePercent = time / 24f;
        Vector3 sunRotation = new Vector3(timePercent * 360 -110, -30, 0);

        lightDirectional.transform.localRotation = Quaternion.Euler(sunRotation);
    }
}
