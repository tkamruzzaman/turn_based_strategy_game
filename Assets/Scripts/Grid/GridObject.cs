using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private readonly GridSystem gridSystem;
    private readonly GridPosition gridPosition;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
       return gridPosition.ToString();
    }
}
