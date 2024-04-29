using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
   // I plan to use this for the jump attack / ground slam
    public static CameraShake instance { get; private set;}
    private CinemachineVirtualCamera vcam;
    private float shakeTime;

    private void Awake()
    {
        instance = this;
        vcam = GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if(shakeTime <= 0)
            {
                CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0;
            }
        }
    }
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        shakeTime = time;
    }
    
}
