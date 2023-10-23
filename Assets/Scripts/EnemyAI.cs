using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;
    private float timerMax = 2;

    private void Awake()
    {
        state = State.WaitingForTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAcion(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        //No more enemy have actions they can take, end enemy turn
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
            default:
                break;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        state = State.TakingTurn;
        timer = timerMax;
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAcion(Action onEnemyAIActionComplete)
    {
        print("Take Enemy AI Acion!");

        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAcion(enemyUnit, onEnemyAIActionComplete))
            { return true; }
        }
        return false;
    }

    private bool TryTakeEnemyAIAcion(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            return false;
        }
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        {
            return false;
        }
        print("Spin Action!");
        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;

    }

}