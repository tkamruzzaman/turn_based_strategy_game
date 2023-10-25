using Cinemachine;
using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera actionVirtualCamera;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        if (sender is ShootAction shootAction)
        {
            Unit shooterUnit = shootAction.GetUnit();
            Unit targetUnit = shootAction.GetTargetUnit();

            Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
            Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
            float sholderOffsetAmount = 0.5f;
            Vector3 sholderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * sholderOffsetAmount;

            Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + sholderOffset + (shootDirection * -1);
            actionVirtualCamera.transform.position = actionCameraPosition;
            actionVirtualCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

            ShowActionCamera();
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        if (sender is ShootAction)
        {
            HideActionCamera();
        }
    }

    private void ShowActionCamera()
    {
        actionVirtualCamera.gameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionVirtualCamera.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    
    }
}