using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class EffectManager : MonoBehaviour
{
    [Serializable]
    public class EffectPrefab
    {
        public EffectType EffectType;
        public GameObject Prefab;
    }

    [SerializeField] private int _capacityPerPool = 100;
    [SerializeField] private List<EffectPrefab> _effectTypePrefabs = new(); // prefabs for each effect type
    private readonly List<ObjectPool> _pools = new(); // uses effecttype as index
    private readonly List<Dictionary<GameObject, Effect>> _effectsOnObjects = new(); // tracks which effects are on which objects, uses effecttype as index

    private void Awake()
    {
        var vals = Enum.GetValues(typeof(EffectType));
        foreach (EffectType effectType in vals)
        {
            Assert.IsTrue((int)effectType == _pools.Count);

            // create the object pool
            foreach (EffectPrefab ep in _effectTypePrefabs)
            {
                if (ep.EffectType != effectType) continue;
                _pools.Add(new ObjectPool(ep.Prefab, _capacityPerPool));
                foreach (var obj in _pools.Last().AliveAndDead)
                {
                    obj.GetComponent<Effect>()._effectType = effectType;
                    obj.transform.parent = transform;
                }
            }

            // setup list
            _effectsOnObjects.Add(new(_capacityPerPool));
        }
    }

    private void OnValidate()
    {
        var seen = new HashSet<EffectType>();
        foreach (var entry in _effectTypePrefabs)
        {
            if (entry.Prefab == null)
            {
                Debug.LogWarningFormat("Effect prefab {0} for type {1} is null", entry.Prefab, entry.EffectType);
                continue;
            }

            if (!seen.Add(entry.EffectType))
            {
                Debug.LogErrorFormat("Duplicate EffectType in _effectTypePrefabs: {0}", entry.EffectType);
            }

            if (!entry.Prefab.TryGetComponent<Effect>(out var _))
            {
                Debug.LogErrorFormat("Effect prefab {0} for type {1} doesn't have Effect script", entry.Prefab, entry.EffectType);
            }
        }
    }

    // Apply effect to entity, this does nothing if the entity already has the same effect with higher amplifier, this overwrites effects with lower amplifiers. Equal amplifiers will use the maximum duration.
    public void ApplyEffect(ILivingEntity target, EffectType effectType, int duration, float amplifier = 1.0f)
    {
        Assert.IsNotNull(target);
        Assert.IsTrue(duration > 0);
        Assert.IsTrue(amplifier > 0);

        // Does the entity already have the effect?
        Dictionary<GameObject, Effect> objectsWithEffect = _effectsOnObjects[(int)effectType];
        if (objectsWithEffect.TryGetValue(target.GetGameObject(), out Effect effect))
        {
            // Set amplification and duration
            if (Mathf.Approximately(effect.Amplification, amplifier))
            {
                effect.Duration = Mathf.Max(effect.Duration, duration); // we're equal
                return;
            }
            else if (effect.Amplification > amplifier) return; // we're weaker
            // else we're stronger, fall through
        }
        else
        {
            // Create a new effect game object
            GameObject poolObj = _pools[(int)effectType].ActivateObject(obj =>
            {
                obj.transform.SetParent(target.GetGameObject().transform, false);
            });

            effect = poolObj.GetComponent<Effect>();
            objectsWithEffect.Add(target.GetGameObject(), effect);
        }

        // Set amplification and duration
        effect.Amplification = amplifier;
        effect.Duration = duration;
    }

    public void ApplyEffect(ILivingEntity target, SerializableEffect effect)
    {
        ApplyEffect(target, effect.Type, effect.Duration, effect.Amplifier);
    }

    public void ApplyEffects(ILivingEntity entity, List<SerializableEffect> effects)
    {
        foreach (SerializableEffect effect in effects)
        {
            ApplyEffect(entity, effect);
        }
    }

    internal void ReleaseFromPoolIfActive(Effect effect)
    {
        Assert.IsNotNull(effect);

        Dictionary<GameObject, Effect> objectsWithEffect = _effectsOnObjects[(int)effect._effectType];
        objectsWithEffect.Remove(effect.transform.parent.gameObject);

        var pool = _pools[(int)effect._effectType];
        if (pool.IsActive(effect.gameObject))
        {
            pool.ReleaseObject(effect.gameObject);
            transform.parent = transform;
        }

    }
}
