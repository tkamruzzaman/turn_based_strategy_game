//#define TESTING

using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material squareGridMaterial;
        public Material hexaGridMaterial;
    }

    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    [Space]
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
#if TESTING
    private GridSystemVisualSingle lastSelectedGridSystemVisualSingle;
#endif


    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnUnitMovedGridPosition += LevelGrid_OnUnitMovedGridPosition;

        CreateGridVisual();

        //UpdateGridVisual();
        Invoke(nameof(UpdateGridVisual), 0.2f);

#if TESTING
        Invoke(nameof(VisualTest), 0.25f);
#endif
    }

#if TESTING
    private void Update()
    {
        if (lastSelectedGridSystemVisualSingle != null) { lastSelectedGridSystemVisualSingle.HideSelected(); }

        Vector3 mouseWorldPosition = MouseWorld.GetPostion();
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(mouseWorldPosition);
        if (LevelGrid.Instance.IsValidGridPosition(gridPosition))
        {
            lastSelectedGridSystemVisualSingle = gridSystemVisualSingleArray[gridPosition.x, gridPosition.z];
        }

        if (lastSelectedGridSystemVisualSingle != null) { lastSelectedGridSystemVisualSingle.ShowSelected(); }

    }

    private void VisualTest()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Show(GetGridVisualTypeMaterial(GridVisualType.White));
            }
        }
    }
#endif

    private void CreateGridVisual()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new(x, z);

                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab,
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity, transform);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) => UpdateGridVisual();

    private void LevelGrid_OnUnitMovedGridPosition(object sender, EventArgs e) => UpdateGridVisual();

    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        List<GridPosition> validGridPositions = selectedAction.GetValidActionGridPosition();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeCricle(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
                case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
                case InteractAction interactAction:
                gridVisualType= GridVisualType.Blue;
                break;
            default:
                gridVisualType = GridVisualType.White;
                break;
        }

        ShowGridPositionList(validGridPositions, gridVisualType);
    }

    public void HideAllGridPosition()
    {
        foreach (GridSystemVisualSingle gridSystemVisualSingle in gridSystemVisualSingleArray)
        {
            gridSystemVisualSingle.Hide();
        }
    }

    private void ShowGridPositionRangeCricle(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                switch (LevelGrid.Instance.GridType)
                {
                    case GridType.Square:
                        return gridVisualTypeMaterial.squareGridMaterial;
                    case GridType.Hexagonal:
                        return gridVisualTypeMaterial.hexaGridMaterial;
                }
            }
        }
        Debug.LogError($"Could not find GridVisualTypeMaterial for {gridVisualType}!");
        return null;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnUnitMovedGridPosition -= LevelGrid_OnUnitMovedGridPosition;

    }
}
