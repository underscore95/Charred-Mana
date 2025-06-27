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

    public enum EffectResult
    {
        Applied, // effect didn't already exist
        OverwroteWeakerEffect, // effect did already exist but had a lower amplifier
        OverwroteShorterEqualEffect, // effect did already exist but had a lower duration and equal amplifier
        NotAppliedTooWeak, // effect already existed with a higher amplifier
        NotAppliedTooShort // effect already existed with an equal amplifier and higher duration
    }

    public static bool WasEffectNotApplied(EffectResult result)
    {
        return result == EffectResult.NotAppliedTooWeak || result == EffectResult.NotAppliedTooShort;
    }

    public static bool WasEffectNotApplied(EffectResult[] results)
    {
        foreach (EffectResult result in results)
        {
            if (!WasEffectNotApplied(result)) return false;
        }
        return true;
    }

    public static bool AppliedAnyNewEffect(EffectResult[] results)
    {
        foreach (EffectResult result in results)
        {
            if (result == EffectResult.Applied) return true;
        }
        return false;
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
    // duration in turns
    public EffectResult ApplyEffect(ILivingEntity target, EffectType effectType, int duration, float amplifier = 1.0f)
    {
        Assert.IsNotNull(target);
        Assert.IsTrue(duration > 0);
        Assert.IsTrue(amplifier > 0);

        // Does the entity already have the effect?
        Dictionary<GameObject, Effect> objectsWithEffect = _effectsOnObjects[(int)effectType];
        bool effectAlreadyExisted = false;
        if (objectsWithEffect.TryGetValue(target.GetGameObject(), out Effect effect))
        {
            effectAlreadyExisted = true;

            // Set amplification and duration
            if (Mathf.Approximately(effect.Amplification, amplifier))
            {
                // we're equal, pick max duration
                if (duration > effect.Duration)
                {
                    effect.Duration = duration;
                    return EffectResult.OverwroteShorterEqualEffect;
                }
                return EffectResult.NotAppliedTooShort;
            }
            else if (effect.Amplification > amplifier) return EffectResult.NotAppliedTooWeak; // we're weaker
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
        return effectAlreadyExisted ? EffectResult.OverwroteWeakerEffect : EffectResult.Applied;
    }

    public EffectResult ApplyEffect(ILivingEntity target, SerializableEffect effect)
    {
        return ApplyEffect(target, effect.Type, effect.Duration, effect.Amplifier);
    }

    public EffectResult[] ApplyEffects(ILivingEntity entity, List<SerializableEffect> effects)
    {
        EffectResult[] results = new EffectResult[effects.Count];
        for (int i = 0; i < effects.Count; ++i)
        {
            results[i] = ApplyEffect(entity, effects[i]);
        }
        return results;
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
