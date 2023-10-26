using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer grenadeTrailRenderer;
    [SerializeField] private LayerMask grenadeDamageLayerMask;
    [SerializeField] private AnimationCurve grenadeAnimationCurve;

    private Action onGrenadeBehaviourComplete;
    private Vector3 targetPosition;
    private readonly int grenadeDamageAmount = 30;
    private float totalDistance;
    private Vector3 positionXZ;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moveSpeed * moveDirection * Time.deltaTime;
        
        float currentDistance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - currentDistance / totalDistance;

        float maxHeight = totalDistance / 3f;
        float positionY = grenadeAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = 0.2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] resultColliderArray = new Collider[10];
            
            int numberOfHitColliders = Physics.OverlapSphereNonAlloc(targetPosition, damageRadius, resultColliderArray, grenadeDamageLayerMask);

            for (int i = 0; i < numberOfHitColliders; i++)
            {
                if (resultColliderArray[i].TryGetComponent(out Unit unit))
                {
                    unit.Damage(grenadeDamageAmount);
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            grenadeTrailRenderer.transform.SetParent(null);
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviourComplete?.Invoke();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;

        positionXZ = transform.position;
        positionXZ.y = 0f;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
