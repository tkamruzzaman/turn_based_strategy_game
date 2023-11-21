using System.Collections.Generic;

public class GridObject
{
    private readonly GridSystemHex<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private IInteractable interactable;

    public GridObject(GridSystemHex<GridObject> gridSystem, GridPosition gridPosition)
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

    public IInteractable GetInteractable() => interactable;

    public void SetInteractable(IInteractable interactable) => this.interactable = interactable;

}
