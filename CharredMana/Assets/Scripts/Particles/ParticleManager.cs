using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private int _maxParticlesPerType = 15;
    [SerializeField] private SerializableEnumDictionary<ParticleType, GameObject> _registeredParticles = new();

    private readonly EnumDictionary<ParticleType, ObjectPool> _pools = new();

    private void Awake()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        transform.localScale = Vector3.one;

        EnumDictionary<ParticleType, GameObject> prefabs = _registeredParticles.ToEnumDictionary();
        foreach (var (particleType, prefab) in prefabs)
        {
            _pools.Set(particleType, new(prefab, _maxParticlesPerType, transform));
        }
    }

    public GameObject SpawnParticle(ParticleType type, Vector3 position)
    {
        return SpawnParticle(type, position, Quaternion.identity);
    }

    public GameObject SpawnParticle(ParticleType type, Vector3 position, Quaternion rotation)
    {
        ObjectPool pool = _pools[type];
        if (pool.IsFull()) return null;
        Transform t = pool.ActivateObject().transform;
        t.SetPositionAndRotation(position, rotation);
        return t.gameObject;
    }
}
