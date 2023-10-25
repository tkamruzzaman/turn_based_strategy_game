using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    
    }

    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        Shake();
    }

    private void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }

    private void OnDestroy()
    {
        ShootAction.OnAnyShoot -= ShootAction_OnAnyShoot;

    }
}
