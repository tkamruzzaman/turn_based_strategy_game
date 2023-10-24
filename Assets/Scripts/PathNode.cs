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
    public int GetHCost() => hCost;
    public int GetFCost() => fCost;
    public PathNode GetCameFromPathNode() => cameFromPathNode;
    public bool GetIsWalkable() => isWalkable;

    public void SetGCost(int value) => gCost = value;
    public void SetHCost(int value) => hCost = value;
    public void CalculateFCost() => fCost = gCost + hCost;
    public void SetCameFromPathNode(PathNode pathNode) => cameFromPathNode = pathNode;
    public void SetIsWalkable(bool isWalkable) => this.isWalkable = isWalkable;

    public void ResetCameFromPathNode() => cameFromPathNode = null;

    public GridPosition GetGridPosition() => gridPosition;


}
