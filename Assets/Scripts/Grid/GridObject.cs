using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private readonly GridSystem<GridObject> gridSystem;
    private readonly GridPosition gridPosition;
    private List<Unit> unitList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new();
    }

    public override string ToString()
    {
        string unitString = string.Empty;
        foreach (Unit unit in unitList)
        {
            unitString += unit.ToString() + "\n";
        }
            return gridPosition.ToString() + "\n" + unitString;
    }

    public List<Unit> GetUnitList() => unitList;

    public void AddUnit(Unit unit) => unitList.Add(unit);

    public void RemoveUnit(Unit unit) => unitList.Remove(unit);

    public bool HasAnyUnit() => unitList.Count > 0;

    public Unit GetUnit() => HasAnyUnit() ? unitList[0] : null;
}
