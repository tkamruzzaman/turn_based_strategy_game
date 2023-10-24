using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    private int weidth;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
       gridSystem = new(10, 10, 2f,
             (GridSystem<PathNode> g, GridPosition gridPosition)
             => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab, parent: transform);

    }
}
