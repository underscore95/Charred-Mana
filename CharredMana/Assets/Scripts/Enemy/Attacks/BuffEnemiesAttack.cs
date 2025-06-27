using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BuffEnemiesAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float _range = 1.5f;
    [SerializeField] private GameObject _buffAnimationPrefab;
    [SerializeField] private List<SerializableEffect> _effects = new();
    private EffectManager _effectManager;

    private void Awake()
    {
        _effectManager = FindAnyObjectByType<EffectManager>();
        Assert.IsTrue(_effects.Count > 0);
    }

    public void HandleAttack()
    {
        Collider2D[] nearbyEnemies = Enemy.GetNearbyEnemies(transform.position, _range);

        bool appliedToEnemy = false;
        foreach (Collider2D col in nearbyEnemies)
        {
            if (col.gameObject == gameObject) continue;
            Assert.IsTrue(col.gameObject.activeInHierarchy);
            if (Vector3.Magnitude((Vector2)transform.position - (Vector2)col.gameObject.transform.position) > _range + 0.01f) continue;

            appliedToEnemy |= EffectManager.AppliedAnyNewEffect(_effectManager.ApplyEffects(col.GetComponent<ILivingEntity>(), _effects));
        }
        if (!appliedToEnemy) return;

        Transform animationTransform = Instantiate(_buffAnimationPrefab).transform;
        animationTransform.position = transform.position + Vector3.forward * 0.1f/*render slightly behind*/;
        animationTransform.localScale = _range * 2 * Vector3.one;
    }
}
