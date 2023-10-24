using System;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;

    private readonly TGridObject[,] gridObjectArray;

    public GridSystem(int width, int height, float cellSize, Func<GridSystem< TGridObject>, GridPosition, TGridObject> createGridObject)
    {
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
        => new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    

    public GridPosition GetGridPosition(Vector3 worldPosition)  
        => new (Mathf.RoundToInt(worldPosition.x / cellSize),
                                Mathf.RoundToInt(worldPosition.z / cellSize));
    

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


