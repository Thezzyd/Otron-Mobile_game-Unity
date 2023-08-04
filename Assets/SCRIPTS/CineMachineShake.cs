using Mirror;
using UnityEngine;
using Cinemachine;

public class CineMachineShake : NetworkBehaviour
{
    public static CineMachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cineVCam;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntesity;

    private void Awake()
    {
        Instance = this;
        cineVCam = GetComponent<CinemachineVirtualCamera>();

    }

    public void ShakeCamera(float intensity, float time)
    {

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cineVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startingIntesity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;

    }

    private void FixedUpdate()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.fixedDeltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cineVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntesity, 0f, (1 - shakeTimer / shakeTimerTotal));

            }
        }
    }
}
