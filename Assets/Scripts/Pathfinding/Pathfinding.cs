#define TESTING
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [Range(0.25f, 1.0f)]
    [SerializeField] private float obstacleDetectionRange = 0.5f;


    private GridType gridType;
    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one Pathfinding" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(GridType gridType, int width, int height, float cellSize)
    {
        this.gridType = gridType;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new(this.gridType, this.width, this.height, this.cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

#if TESTING
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab, parent: transform);
#endif     

        DetectObstacles();
    }

    [ContextMenu("DetectObstacles")]
    private void DetectObstacles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GetNode(x, z).SetIsWalkable(false);
                GridPosition gridPosition = new(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (!Physics.Raycast(
                     worldPosition + Vector3.down * raycastOffsetDistance,
                     Vector3.up,
                     raycastOffsetDistance * 2,
                     obstaclesLayerMask))
                {
                    //Obstacle in this GridPosition
                    //GetNode(x, z).SetIsWalkable(true);
                }
                if (!Physics.BoxCast(
                    worldPosition + Vector3.down * raycastOffsetDistance,
                    Vector3.one * 0.5f,
                    Vector3.up, Quaternion.identity,
                    raycastOffsetDistance * 2,
                    obstaclesLayerMask))
                {
                    //Obstacle in this GridPosition
                    GetNode(x, z).SetIsWalkable(true);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new();
        List<PathNode> closeList = new();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        switch (gridType)
        {
            case GridType.Square:
                startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
                break;
            case GridType.Hexagonal:
                startNode.SetHCost(CalculateHeuristicDistance(startGridPosition, endGridPosition));
                break;
        }
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                //reached the final node
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            List<PathNode> neighbourList = new();
            switch (gridType)
            {
                case GridType.Square:
                    neighbourList = SquareGridGetNeighbourList(currentNode);
                    break;
                case GridType.Hexagonal:
                    neighbourList = HexagonalGridGetNeighbourList(currentNode);
                    break;
            }
            foreach (PathNode neighbourNode in neighbourList)
            {
                if (closeList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.GetIsWalkable())
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                int tantativeGCost = 0;
                switch (gridType)
                {
                    case GridType.Square:
                        tantativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                        break;
                    case GridType.Hexagonal:
                        tantativeGCost = currentNode.GetGCost() + MOVE_STRAIGHT_COST;
                        break;
                }

                if (tantativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tantativeGCost);
                    switch (gridType)
                    {
                        case GridType.Square:
                            neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                            break;
                        case GridType.Hexagonal:
                            neighbourNode.SetHCost(CalculateHeuristicDistance(neighbourNode.GetGridPosition(), endGridPosition));
                            break;
                    }
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //No path found
        pathLength = 0;
        return null;
    }

    private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaingDistance = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaingDistance;
    }
    private int CalculateHeuristicDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        return Mathf.RoundToInt(MOVE_STRAIGHT_COST 
            * Vector3.Distance(gridSystem.GetWorldPosition(gridPositionA), 
            gridSystem.GetWorldPosition(gridPositionB)));
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new()
        {
            endNode
        };

        PathNode currentNode = endNode;

        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new();

        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }

    private List<PathNode> SquareGridGetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            //left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            //left-up
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
            //left-down
            if (gridPosition.z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            //right-up
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
            //right-down
            if (gridPosition.z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        if (gridPosition.z - 1 >= 0)
        {
            //down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        return neighbourList;
    }

    private List<PathNode> HexagonalGridGetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new();

        GridPosition gridPosition = currentNode.GetGridPosition();

        //left
        if (gridPosition.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
        }

        //right
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
        }

        //up
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        //down
        if (gridPosition.z - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        bool oddRow = gridPosition.z % 2 == 1;
        if (oddRow)
        {
            if (gridPosition.x + 1 < gridSystem.GetWidth())
            {
                if (gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
                }
            }
        }
        else
        {
            if (gridPosition.x - 1 >= 0)
            {
                if (gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }
            }
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int z)
        => gridSystem.GetGridObject(new GridPosition(x, z));

    public void SetWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
        => gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);

    public bool IsWalkableGridPosition(GridPosition gridPosition)
        => gridSystem.GetGridObject(gridPosition).GetIsWalkable();

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
        => FindPath(startGridPosition, endGridPosition, out _) != null;

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }

}