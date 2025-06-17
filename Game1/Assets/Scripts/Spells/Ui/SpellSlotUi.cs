using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellSlotUi : MonoBehaviour
{
    [SerializeField] private GameObject _uiParent;
    [SerializeField] private TextMeshProUGUI _manaCostText;
    [SerializeField] private TextMeshProUGUI _keybindText;
    [SerializeField] private TextMeshProUGUI _spellTitle;
    [SerializeField] private InputActionReference _triggerInput;

    private PlayerSpell _spell;
    public PlayerSpell Spell
    {
        get { return _spell; }
        set
        {
            _spell = value;
            _uiParent.SetActive(_spell != null);
            if (_spell != null)
            {
                _manaCostText.text = "Mana: " + _spell.ManaCost;
                _spellTitle.text = _spell.name;
            }
        }
    }

    private SpellQueue _spellQueue;

    private void Awake()
    {
        _spellQueue = FindAnyObjectByType<SpellQueue>();
        Spell = null;
    }

    private void Update()
    {
        if (Spell == null) return;
        if (!_triggerInput.ToInputAction().WasPressedThisFrame()) return;

        _spellQueue.QueueTrigger(Spell);
    }
}
