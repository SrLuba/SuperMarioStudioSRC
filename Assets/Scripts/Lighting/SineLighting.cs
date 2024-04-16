using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SineLighting : MonoBehaviour
{
    public float amplitude;
    public float speed;
    public Light2D targetLight;

    float originalIntensity;
    public void Awake()
    {
        originalIntensity = targetLight.intensity;
    }
    void FixedUpdate()
    {
        targetLight.intensity = originalIntensity + Mathf.Sin(Time.time*speed) * amplitude;
    }
}
