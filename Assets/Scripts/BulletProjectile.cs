using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;
    private Vector3 targetPosition;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        float moveSpeed = 200f;
        transform.position += moveSpeed * moveDirection * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            //Over shoot the target
            transform.position = targetPosition;
            trailRenderer.transform.SetParent(null);
            Destroy(gameObject);

            Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);
        }
    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
