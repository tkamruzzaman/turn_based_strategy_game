using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit;

    GridSystemVisual gridSystemVisual;

    private void Start()
    {
        gridSystemVisual = FindObjectOfType<GridSystemVisual>();    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            gridSystemVisual.HideAllGridPosition();
            List<GridPosition> validGridPositions = unit.GetMoveAction().GetValidActionGridPosition();
            gridSystemVisual.ShowGridPositionList(validGridPositions);
        }
    }
}
