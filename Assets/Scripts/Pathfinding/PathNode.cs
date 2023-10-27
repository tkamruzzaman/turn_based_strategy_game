using System;

public class PathNode
{
    private GridPosition gridPosition;

    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool isWalkable = true;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost() => gCost;
    public void SetGCost(int value) => gCost = value;

    public int GetHCost() => hCost;
    public void SetHCost(int value) => hCost = value;

    public int GetFCost() => fCost;
    public void CalculateFCost() => fCost = gCost + hCost;

    public bool GetIsWalkable() => isWalkable;
    public void SetIsWalkable(bool isWalkable) => this.isWalkable = isWalkable;
    
    public PathNode GetCameFromPathNode() => cameFromPathNode;
    public void SetCameFromPathNode(PathNode pathNode) => cameFromPathNode = pathNode;
    public void ResetCameFromPathNode() => cameFromPathNode = null;

    public GridPosition GetGridPosition() => gridPosition;


}
