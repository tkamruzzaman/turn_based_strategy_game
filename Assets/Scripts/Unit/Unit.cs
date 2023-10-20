using System;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointChanged;

    private const int ACTION_POINT_MAX = 2;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    private int actionPoints = ACTION_POINT_MAX;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition)
        {
            //unit changed GridPosition
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
       ResetActionPoints();
    }

    public GridPosition GetGridPosition() => gridPosition;

    public MoveAction GetMoveAction() => moveAction;

    public SpinAction GetSpinAction() => spinAction;

    public BaseAction[] GetBaseActionArray() => baseActionArray;

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            
            return true;
        }
        return false;
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction) 
        => actionPoints >= baseAction.GetActionPointsCost();

    private void ResetActionPoints()
    {
        actionPoints = ACTION_POINT_MAX;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints() => actionPoints;

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;

    }

}