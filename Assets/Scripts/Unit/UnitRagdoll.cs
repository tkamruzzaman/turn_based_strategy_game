using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        Vector3 randomDirection = new (Random.Range(-1f, +1f), 0, Random.Range(-1f, +1f)); ;
        ApplyExplosionToRagdoll(ragdollRootBone, Random.Range(300f, 350f), transform.position + randomDirection, 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.SetPositionAndRotation(child.position, child.rotation);

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explotionFource, Vector3 explotionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explotionFource, explotionPosition, explosionRadius);
            }

            ApplyExplosionToRagdoll(child, explotionFource, explotionPosition, explosionRadius);
        }
    }
}