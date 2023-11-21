using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = 0.75f;
    private const float HEX_ODD_ROW_HORIZOLTAL_OFFSET_MULTIPLIER = 0.5f;
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;

    private readonly TGridObject[,] gridObjectArray;

    public GridType GridType {  get; private set; }

    public GridSystem(GridType gridType, int width, int height, float cellSize, Func<GridSystem< TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.GridType = gridType;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * 0.2f, Color.white, 1000);
                GridPosition gridPosition = new(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return GridType switch
        {
            GridType.Square => new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize,
            GridType.Hexagonal => new Vector3(gridPosition.x, 0, 0) * cellSize +
                                   cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER * new Vector3(0, 0, gridPosition.z) +
                                   ((gridPosition.z % 2) == 1 ? cellSize * HEX_ODD_ROW_HORIZOLTAL_OFFSET_MULTIPLIER * new Vector3(1, 0, 0) : Vector3.zero),
            _ => default,
        };
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        switch (GridType)
        {
            case GridType.Square:
                return new(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));

            case GridType.Hexagonal:
                GridPosition approximateGridPosition = new(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize / HEX_VERTICAL_OFFSET_MULTIPLIER));

                bool oddRow = (approximateGridPosition.z % 2) == 1;

                List<GridPosition> neighbourGridPositionList = new()
                {
                    approximateGridPosition + new GridPosition(-1, 0),
                    approximateGridPosition + new GridPosition(+1, 0),

                    approximateGridPosition + new GridPosition(0, +1),
                    approximateGridPosition + new GridPosition(0, -1),

                    approximateGridPosition + new GridPosition(oddRow?+1:-1, +1),
                    approximateGridPosition + new GridPosition(oddRow?+1:-1, -1),
                };

                GridPosition closestGridPosition = approximateGridPosition;
                foreach (GridPosition neighbourGridPosition in neighbourGridPositionList)
                {
                    Vector3 closestWorldPosition = GetWorldPosition(closestGridPosition);
                    Vector3 neighbourWorldPosition = GetWorldPosition(neighbourGridPosition);
                    float closestDistance = Vector3.Distance(worldPosition, closestWorldPosition);
                    float neighbourDistance = Vector3.Distance(worldPosition, neighbourWorldPosition);
                    if (neighbourDistance < closestDistance)
                    {
                        closestGridPosition = neighbourGridPosition;
                    }
                }
                return closestGridPosition;

            default:
                return default;
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)   
        => gridObjectArray[gridPosition.x, gridPosition.z]; 
    

    public bool IsValidGridPosition(GridPosition gridPosition)    
        => gridPosition.x >=0 
            && gridPosition.z >=0
            && gridPosition.x < width 
            && gridPosition.z < height;
    

    public int GetWidth() => width;

    public int GetHeight() => height;

    public void CreateDebugObjects(Transform debugPrefab, Transform parent)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new(x,z);
                Transform debugTransfom = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity, parent);
                //t.GetComponentInChildren<TMPro.TMP_Text>().text = $"x:{x},\n z:{z}";
                GridDebugObject gridDebugObject = debugTransfom.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
}


