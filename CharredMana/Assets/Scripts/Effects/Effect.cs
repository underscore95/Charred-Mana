using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Effect : MonoBehaviour
{
    public int Amplifier { get; set; }
    public int Duration { get; set; }

    private TurnManager _turnManager;
    internal EffectType _effectType;
    private EffectManager _effectManager;
    private bool _isInPool = false;
    private bool _isFirstEnable = true;

    private void Awake()
    {
        _turnManager = FindAnyObjectByType<TurnManager>();
        _effectManager = FindAnyObjectByType<EffectManager>();
    }

    private void OnEnable()
    {
        if (_isFirstEnable)
        {
            _isFirstEnable = false;
            return;
        }

        _isInPool = true;
        _turnManager.OnTurnChange += HandleTurnChange;
        OnPoolEnter();
    }

    private void OnDisable()
    {
        if (!_isInPool) return;
        _isInPool = false;

        if (_effectManager != null)
        {
            _effectManager.ReleaseFromPoolIfActive(this); // null when exiting game
        }

        _turnManager.OnTurnChange -= HandleTurnChange;
        OnPoolLeave();
    }

    public abstract void OnPoolEnter();
    public abstract void OnTurnChange();
    public abstract void OnPoolLeave();

    private void HandleTurnChange()
    {
        if (Duration <= 0)
        {
            RemoveEffect();
            return;
        }
        Duration--;
        OnTurnChange();
        if (Duration < 0)
        {
            RemoveEffect();
        }
    }

    private void RemoveEffect()
    {
        LivingEntity ent = GetComponentInParent<LivingEntity>();
        Assert.IsNotNull(ent);
        _effectManager.RemoveEffect(ent, _effectType);
    }
}
