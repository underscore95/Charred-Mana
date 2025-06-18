using System.Collections;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public float Amplification { get; set; }
    public int Duration { get; set; }

    private TurnManager _turnManager;
    internal EffectType _effectType;
    private EffectManager _effectManager;
    private bool _isInPool = false;
    private bool _isFirstEnable = true;

    private void Awake()
    {
        _turnManager = FindAnyObjectByType<TurnManager>();
        _effectManager = FindAnyObjectByType <EffectManager>(); 
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

        _effectManager.ReleaseFromPoolIfActive(this);

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
            gameObject.SetActive(false);
            return;
        }
        Duration--;
        OnTurnChange();
        if (Duration < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
