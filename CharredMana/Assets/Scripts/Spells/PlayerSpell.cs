using System;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class PlayerSpell : MonoBehaviour
{
    [SerializeField] private int _spellCooldown = 1;
    [SerializeField] private int _manaCost = 10;
    public int ManaCost { get { return _manaCost; } }
    [SerializeField] private string _optionalDescription = string.Empty;
    public string OptionalDescription { get { return _optionalDescription; } }
    public int SpellCooldown { get { return _spellCooldown; } }
    public int CurrentCooldown { get; private set; }

    protected void Awake()
    {
        CurrentCooldown = 0;

        Assert.IsTrue(_spellCooldown >= 0, "Spell cooldown cannot be negative");

        FindAnyObjectByType<TurnManager>().OnTurnChange += () => { if (CurrentCooldown > 0) CurrentCooldown--; };
    }

    public abstract void OnTrigger(SpellTriggerInfo info);
    public abstract void OnPreTrigger(SpellTriggerInfo info);

    public void StartCooldown()
    {
        CurrentCooldown = SpellCooldown;
    }

    public bool IsOnCooldown()
    {
        return CurrentCooldown > 0;
    }
}
