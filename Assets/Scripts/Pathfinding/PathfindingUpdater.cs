using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyDestructibleDestroyed += DestructibleCrate_OnAnyDestructibleDestroyed;
        Door.OnAnyInteractableStatusChanged += Door_OnAnyDoorStatusChanged;

    }

    private void DestructibleCrate_OnAnyDestructibleDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetWalkableGridPosition(destructibleCrate.GetGridPosition(), isWalkable: true);
    }

    private void Door_OnAnyDoorStatusChanged(object sender, bool e)
    {
        Door door = sender as Door;
        Pathfinding.Instance.SetWalkableGridPosition(door.GetGridPosition(), isWalkable: e);
    }

    private void OnDestroy()
    {
        DestructibleCrate.OnAnyDestructibleDestroyed -= DestructibleCrate_OnAnyDestructibleDestroyed;
        Door.OnAnyInteractableStatusChanged -= Door_OnAnyDoorStatusChanged;

    }

}
