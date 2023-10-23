using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-9)]
public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> frindlyUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new();
        frindlyUnitList = new();
        enemyUnitList = new();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;

        unitList.Add(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            frindlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            frindlyUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList() => unitList;

    public List<Unit> GetFriendlyUnitList() => frindlyUnitList;

    public List<Unit> GetEnemyUnitList() => enemyUnitList;

    private void OnDestroy()
    {
        Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }
}
