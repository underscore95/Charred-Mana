using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class SelectSpellUi : MonoBehaviour
{
    [SerializeField] private SpellListUi _active;
    [SerializeField] private SpellListUi _unlocked;
    [SerializeField] private TextMeshProUGUI _activeSpellsText;
    [SerializeField] private string _activeSpellsTextText = "Active: {0} / {1}";
    [SerializeField] private InputActionReference _closeUiInput;
    private SpellManager _spellManager;

    private void Awake()
    {
        _spellManager = FindAnyObjectByType<SpellManager>();
    }

    private void Start()
    {
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

    private void Update()
    {
        if (_closeUiInput.ToInputAction().WasPressedThisFrame())
        {
            CloseUi();
        }
    }

    private void RecreateSpellListsContents()
    {
        _active.SilentlyClearSpells();
        _unlocked.SilentlyClearSpells();

        foreach (int spellIndex in _spellManager.GetUnlockedSpellsIndices())
        {
            PlayerSpell spell = _spellManager.GetSpell(spellIndex);
            if (_spellManager.IsSpellIndexSelected(spellIndex, out int _))
            {
                _active.AddSpell(spell, () => MoveToUnlocked(spell), true);
            }
            else
            {
                _unlocked.AddSpell(spell, () => MoveToActive(spell), true);
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

    public void OpenUi()
    {
        UIState.IsLevelUpRewardsUiOpen = true;
        gameObject.SetActive(true);
        RecreateSpellListsContents();
    }

    private void CloseUi()
    {
        UIState.IsLevelUpRewardsUiOpen = false;
        gameObject.SetActive(false);
    }
}
