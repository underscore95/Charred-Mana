using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    // Inputs to trigger the selected spells
    [SerializeField] private List<InputActionReference> _spellTriggers;
    // Parent object to all the spells
    [SerializeField] private GameObject _spellsContainer;
    [SerializeField] private List<int> _selectedSpells = new();
    private readonly List<PlayerSpell> _spells = new();
    private SpellQueue _spellQueue;

    private void Awake()
    {
        _spellQueue = GetComponentInChildren<SpellQueue>();

        foreach (Transform obj in _spellsContainer.transform)
        {
            if (obj.TryGetComponent<PlayerSpell>(out var spell))
            {
                _spells.Add(spell);
            }
        }
        Assert.IsTrue(_spellTriggers.Count > 0);
        Assert.IsTrue(_spellTriggers.Count <= _spells.Count);
        Assert.IsTrue(_selectedSpells.Count == _spellTriggers.Count, "Selected spells and spell triggers must be same length!");
        HashSet<int> temp = new();
        foreach (int v in _selectedSpells) Assert.IsTrue(temp.Add(v), "Spell " + _spells[v] + " was selected twice (index: " + v + ")");
    }

    private void Update()
    {
        for (int i = 0; i < _spellTriggers.Count; i++)
        {
            if (_spellTriggers[i].ToInputAction().WasPressedThisFrame())
            {
                _spellQueue.QueueTrigger(_spells[_selectedSpells[i]]);
            }
        }
    }


}
