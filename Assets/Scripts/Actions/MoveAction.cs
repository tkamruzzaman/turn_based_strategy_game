using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private const string ANIM_PARAM_IS_WALKING = "IsWalking";

    [SerializeField] private Animator animator;
    [SerializeField] private int maxMoveDistance;

    private Vector3 targetPosition;
    private float moveSpeed = 5f;
    private float rotationSpeed = 7.5f;
    private float stoppingDistance = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if(!isActive) { return; }

        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveSpeed * Time.deltaTime * moveDirection;

            animator.SetBool(ANIM_PARAM_IS_WALKING, true);
        }
        else
        {
            animator.SetBool(ANIM_PARAM_IS_WALKING, false);
            isActive = false;
            onActionComplete();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;

        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //invalid GridPosition
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //Same GridPosition where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition already occupied with another unit
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName() => "Move";
}