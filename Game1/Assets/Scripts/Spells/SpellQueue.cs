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
    private Player _player;

    private void Awake()
    {
        _mana = FindAnyObjectByType<PlayerMana>();
        _player = FindAnyObjectByType<Player>();

        FindAnyObjectByType<TurnManager>().OnTurnChange += UseSpellsOnTurnChange;
    }

    private void Start()
    {
        UpdateText();
    }

    public void QueueTrigger(PlayerSpell spell)
    {
        if (_queuedSpells.Count >= _player.Stats.Focus) return;
        if (spell.IsOnCooldown()) return;
        if (_mana.Mana < spell.ManaCost) return;

        TriggeredSpell triggered = new()
        {
            Spell = spell,
            TriggerInfo = new()
        };

        spell.OnPreTrigger(triggered.TriggerInfo);
        _queuedSpells.Add(triggered);
        spell.StartCooldown();
        _mana.Mana -= spell.ManaCost;
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
        StringBuilder sb = new(string.Format("{0} / {1} Queued Spells{2}", _queuedSpells.Count, _player.Stats.Focus, _queuedSpells.Count > 0 ? ":\n" : ""));
        foreach (var spell in _queuedSpells)
        {
            sb.AppendLine(spell.Spell.name);
        }

        _text.text = sb.ToString();
    }
}
