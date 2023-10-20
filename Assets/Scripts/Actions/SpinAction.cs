using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Update()
    {
        if (!isActive) { return; }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;

        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;

        isActive = true;
        totalSpinAmount = 0f;
        print("Spin");
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new();

        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition,
        };
    }
    public override string GetActionName()
    {
        return "Spin";
    }
}
