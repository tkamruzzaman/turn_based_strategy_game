using System;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyDestructibleDestroyed += DestructibleCrate_OnAnyDestructibleDestroyed;
    
    }

    private void DestructibleCrate_OnAnyDestructibleDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetWalkableGridPosition(destructibleCrate.GetGridPosition(), isWalkable: true);
    }

    private void OnDestroy()
    {
        DestructibleCrate.OnAnyDestructibleDestroyed -= DestructibleCrate_OnAnyDestructibleDestroyed;

    }

}
