using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private const string ANIM_PARAM_IS_WALKING = "IsWalking";
    private const string ANIM_PARAM_SHOOT = "Shoot";

    MoveAction moveAction;
    ShootAction shootAction;

    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            this.moveAction = moveAction;
            this.moveAction.OnStartMoving += MoveAction_OnStartMoving;
            this.moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            this.shootAction = shootAction;
            this.shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(ANIM_PARAM_IS_WALKING, true);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(ANIM_PARAM_IS_WALKING, false);
    }

    private void ShootAction_OnShoot(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ANIM_PARAM_SHOOT);
    }

    private void OnDestroy()
    {
        if (moveAction != null)
        {
            moveAction.OnStartMoving -= MoveAction_OnStartMoving;
            moveAction.OnStopMoving -= MoveAction_OnStopMoving;
        }

        if (shootAction != null)
        {
            shootAction.OnShoot -= ShootAction_OnShoot;
        }
    }
}