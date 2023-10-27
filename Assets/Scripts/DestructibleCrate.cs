using System;
using UnityEngine;

[SelectionBase]
public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestructibleDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;

    private GridPosition gridPosition;

    private void Start()
    {
       gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {              
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        Destroy(gameObject);

        OnAnyDestructibleDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explotionFource, Vector3 explotionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explotionFource, explotionPosition, explosionRadius);
                Destroy(child.gameObject, 3f);
            }

            ApplyExplosionToChildren(child, explotionFource, explotionPosition, explosionRadius);
        }
    }

    public GridPosition GetGridPosition() => gridPosition;
}
