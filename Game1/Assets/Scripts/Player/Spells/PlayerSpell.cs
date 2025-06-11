using System;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class PlayerSpell : MonoBehaviour
{
    [SerializeField] private int _spellCooldown = 1;
    public int SpellCooldown { get { return _spellCooldown; } }
    public int CurrentCooldown { get; private set; }
    public int ManaCost { get; private set; } = 10;

    protected void Awake()
    {
        CurrentCooldown = 0;

        Assert.IsTrue(_spellCooldown > 0);

        TurnManager.Instance().OnTurnChange += () => { if (CurrentCooldown > 0) CurrentCooldown--; };
    }

    public abstract void OnTrigger(SpellTriggerInfo info);

    public void StartCooldown()
    {
        CurrentCooldown = SpellCooldown;
    }

    public bool IsOnCooldown()
    {
        return CurrentCooldown > 0;
    }
}
