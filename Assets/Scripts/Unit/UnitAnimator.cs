using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

    private const string ANIM_PARAM_IS_WALKING = "IsWalking";
    private const string ANIM_PARAM_SHOOT = "Shoot";
    private const string ANIM_PARAM_SWORD_SLASH = "SwordSlash";

    MoveAction moveAction;
    ShootAction shootAction;
    SwordAction swordAction;

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

        if (TryGetComponent(out SwordAction swordAction))
        {
            this.swordAction = swordAction;
            this.swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
            this.swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool(ANIM_PARAM_IS_WALKING, true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool(ANIM_PARAM_IS_WALKING, false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger(ANIM_PARAM_SHOOT);

        Transform bulletTransform = Instantiate(bulletProjectilePrefab,
             shootPointTransform.position,
             Quaternion.identity, shootPointTransform);

        if (bulletTransform.TryGetComponent(out BulletProjectile bulletProjectile))
        {
            Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
            targetUnitShootAtPosition.y = shootPointTransform.position.y;
            bulletProjectile.Setup(targetUnitShootAtPosition);
        }
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();

        animator.SetTrigger(ANIM_PARAM_SWORD_SLASH);
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }

    private void EquipSword()
    {
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(true);
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

        if (swordAction != null)
        {
            swordAction.OnSwordActionCompleted -= SwordAction_OnSwordActionCompleted;
            swordAction.OnSwordActionStarted -= SwordAction_OnSwordActionStarted;
        }

    }
}