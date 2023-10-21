using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;
    private float timerMax = 3;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        timer = timerMax;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            TurnSystem.Instance.NextTurn();
        }
    }
}
