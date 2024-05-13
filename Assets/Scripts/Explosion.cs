using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    public void ExplodeCreatedCubes(Vector3 explosionPosition, List<Cube> spawnedCubes)
    {
        foreach (Cube cube in spawnedCubes)
        {
            if (cube.TryGetComponent(out Rigidbody rigidBody))
                rigidBody.AddExplosionForce(_explosionForce, explosionPosition, _explosionRadius);
        }
    }

    public void Explode(Vector3 explosionPosition, Vector3 scale)
    {
        float decreaseFactor;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, _explosionRadius);
        float explosionForceFromSize = CalculateExplosionForceFromSize(scale);

        foreach (Collider collider in colliders)
        {
            decreaseFactor = CalculateExplosionDecreaseFactor(collider.transform.position, explosionPosition);

            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rigiBody))
                rigiBody.AddExplosionForce(explosionForceFromSize * decreaseFactor, explosionPosition, _explosionRadius);
        }
    }

    private float CalculateExplosionForceFromSize(Vector3 scale)
    {
        float baseCubeSize = 1f;

        return _explosionForce * (baseCubeSize / scale.x);
    }

    private float CalculateExplosionDecreaseFactor(Vector3 objectPosition, Vector3 explosionPosition)
    {
        return Mathf.Clamp01(1 - (Vector3.Distance(objectPosition, explosionPosition) / _explosionRadius));
    }
}