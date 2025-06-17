using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    // Parent object to all the spells
    [SerializeField] private Transform _spellsContainer;
    [SerializeField] private Transform _spellUisContainer;
    [SerializeField] private List<int> _unlockedSpells = new();
    private List<SpellSlotUi> _spellUis = new();
    private List<PlayerSpell> _spells = new();
    private readonly List<Vector3> _originalSpellUiPositions = new();
    public UnityAction OnSpellUnlock = delegate () { };

    private void Awake()
    {
        TransformUtils.GetChildrenWithComponent(ref _spells, _spellsContainer);
        TransformUtils.GetChildrenWithComponent(ref _spellUis, _spellUisContainer);

        Assert.IsTrue(_spells.Count > 0);

        for (int i = 0; i < _spellUis.Count; i++)
        {
            _spellUis[i].Spell = i == 0 ? _spells[0] : null;
            _originalSpellUiPositions.Add(_spellUis[i].transform.position);
        }

        UnlockSpell(0);
    }

    private void Update()
    {
        // if () return; // if spells not changed

        // set y positions so no gaps if some middle slots aren't used
        int numSelectedSpells = 0;
        foreach (SpellSlotUi ui in _spellUis)
        {
            if (ui.Spell == null) continue;
            ui.transform.position = _originalSpellUiPositions[numSelectedSpells];
            numSelectedSpells++;
        }
    }

    public PlayerSpell GetSelectedSpell(int slot)
    {
        Assert.IsTrue(slot >= 0);
        Assert.IsTrue(slot < _spellUis.Count);
        return _spellUis[slot].Spell;
    }

    public void SelectSpell(int spellSlot, PlayerSpell spell)
    {
        for (int i = 0; i < _spellUis.Count && i != spellSlot; ++i)
        {
            if (_spellUis[i].Spell == spell)
            {
                Debug.LogWarningFormat("Attempted to put spell {0} in slot {1} but it was already selected in slot {2}", spell, spellSlot, i);
                return;
            }
        }

        Assert.IsTrue(spellSlot >= 0);
        Assert.IsTrue(spellSlot < _spellUis.Count);
        _spellUis[spellSlot].Spell = spell;
    }

    public IReadOnlyList<int> GetUnlockedSpellsIndices()
    {
        return _unlockedSpells;
    }

    public bool IsSpellIndexSelected(int spellIndex, out int selectedIndex)
    {
        for (int i = 0; i < _spellUis.Count; ++i)
        {
            var spellSlot = _spellUis[i];
            if (spellSlot.Spell == _spells[spellIndex])
            {
                selectedIndex = i;
                return true;
            }
        }
        selectedIndex = -1;
        return false;
    }

    public PlayerSpell GetSpell(int spellIndex)
    {
        return _spells[spellIndex];
    }

    public void UnlockSpell(int spellIndex)
    {
        _unlockedSpells.Add(spellIndex);
        OnSpellUnlock();
    }

    // Returns the first slot which has no spell, returns -1 if every slot is used
    public int GetUnusedSelectedSlot()
    {
        for (int i = 0; i < _spellUis.Count; i++)
        {
            if (_spellUis[i].Spell == null) return i;
        }
        return -1;
    }

    public void DeselectSpell(PlayerSpell spell)
    {
        foreach (var selected in _spellUis)
        {
            if (selected.Spell == spell)
            {
                selected.Spell = null;
                return;
            }
        }
        Debug.LogWarningFormat("Attempted to deselect spell {0} but it wasn't selected", spell);
    }

    public int GetMaximumSelectedSpells()
    {
        return _spellUis.Count;
    }
}
