using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SpellQueue : MonoBehaviour
{
    private struct TriggeredSpell
    {
        public PlayerSpell Spell;
        public SpellTriggerInfo TriggerInfo;
    }

    [SerializeField] private TextMeshProUGUI _text;

    private PlayerMana _mana;
    private readonly List<TriggeredSpell> _queuedSpells = new();

    private void Awake()
    {
        _mana = FindAnyObjectByType<PlayerMana>();

        FindAnyObjectByType<TurnManager>().OnTurnChange += UseSpellsOnTurnChange;
        UpdateText();
    }

    public void QueueTrigger(PlayerSpell spell)
    {
        if (spell.IsOnCooldown()) return;
        if (_mana.Mana < spell.ManaCost) return;

        TriggeredSpell triggered = new()
        {
            Spell = spell,
            TriggerInfo = new()
        };
        _queuedSpells.Add(triggered);
        spell.StartCooldown();
        _mana.RemoveMana(spell.ManaCost);
        UpdateText();
    }

    private void UseSpellsOnTurnChange()
    {
        foreach (var spell in _queuedSpells)
        {
            spell.Spell.OnTrigger(spell.TriggerInfo);
        }
        _queuedSpells.Clear();
        UpdateText();
    }

    private void UpdateText()
    {
        StringBuilder sb = new(_queuedSpells.Count > 0 ? "Queued Spells:\n" : "");
        foreach (var spell in _queuedSpells)
        {
            sb.AppendLine(spell.Spell.name);
        }

        _text.text = sb.ToString();
    }
}
