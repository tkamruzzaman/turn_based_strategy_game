//#define TESTING

using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-3)]
public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnUnitMovedGridPosition;

    [SerializeField] private Transform gridDebugObjectPrefab;

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2;

    private GridSystem<GridObject> gridSystem;

    [field: SerializeField]
    public GridType GridType { get; private set; }

    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private Material floorSquareGridMaterial;
    [SerializeField] private Material floorHexaGridMaterial;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        floorMeshRenderer.material = GridType == GridType.Square? floorSquareGridMaterial : floorHexaGridMaterial;

        gridSystem = new(GridType, width, height, cellSize, 
            (GridSystem<GridObject> g, GridPosition gridPosition) 
            => new GridObject(g, gridPosition));

#if TESTING
        {
            gridSystem.CreateDebugObjects(gridDebugObjectPrefab, parent: transform);
        }
#endif
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(GridType, width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);

        OnUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }

}