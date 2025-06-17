using TMPro;
using UnityEngine;

public class SelectSpellUi : MonoBehaviour
{
    [SerializeField] private SpellListUi _active;
    [SerializeField] private SpellListUi _unlocked;
    [SerializeField] private TextMeshProUGUI _activeSpellsText;
    [SerializeField] private string _activeSpellsTextText = "Active: {0} / {1}";
    private SpellManager _spellManager;

    private void Awake()
    {
        _spellManager = FindAnyObjectByType<SpellManager>();
        RecreateSpellListsContents();

        _spellManager.OnSpellUnlock += RecreateSpellListsContents;

        _active.OnElementAdd += spell =>
        {
            _spellManager.SelectSpell(_spellManager.GetUnusedSelectedSlot(), spell);
        };

        _active.OnElementRemove += spell =>
        {
            _spellManager.DeselectSpell(spell);
        };
    }

    private void RecreateSpellListsContents()
    {
        _active.ClearSpells();
        _unlocked.ClearSpells();

        foreach (int spellIndex in _spellManager.GetUnlockedSpellsIndices())
        {
            PlayerSpell spell = _spellManager.GetSpell(spellIndex);
            if (_spellManager.IsSpellIndexSelected(spellIndex, out int _))
            {
                MoveToActive(spell);
            }
            else
            {
                MoveToUnlocked(spell);
            }
        }

        _activeSpellsText.text = string.Format(_activeSpellsTextText, _active.Size(), _spellManager.GetMaximumSelectedSpells());
    }

    private void MoveToUnlocked(PlayerSpell spell)
    {
        _active.RemoveSpell(spell);
        _unlocked.AddSpell(spell, () => MoveToActive(spell));
    }

    private void MoveToActive(PlayerSpell spell)
    {
        if (_active.Size() >= _spellManager.GetMaximumSelectedSpells()) return;

        _unlocked.RemoveSpell(spell);
        _active.AddSpell(spell, () => MoveToUnlocked(spell));
    }
}
