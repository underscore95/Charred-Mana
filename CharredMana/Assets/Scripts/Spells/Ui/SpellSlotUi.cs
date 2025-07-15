using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class SpellSlotUi : MonoBehaviour
{
    [SerializeField] private GameObject _uiParent;
    [SerializeField] private TextMeshProUGUI _manaCostText;
    [SerializeField] private TextMeshProUGUI _keybindText;
    [SerializeField] private TextMeshProUGUI _spellTitle;
    [SerializeField] private InputActionReference _triggerInput;
    [SerializeField] private GameObject _cooldownParent;
    [SerializeField] private TextMeshProUGUI _cooldownText;

    private PlayerSpell _spell;
    public PlayerSpell Spell
    {
        get { return _spell; }
        set
        {
            _spell = value;
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
        if (_spell == null) Spell = null; // Make sure ui is correct, if _spell isn't null then it has already been set and must be correct

        _cooldownParent.SetActive(false);

        _keybindText.text = "Key: " + _triggerInput.action.GetBindingDisplayString();

        FindAnyObjectByType<TurnManager>().OnLateTurnChange += UpdateCooldownDisplay;
    }

    private void Update()
    {
        _uiParent.SetActive(enabled && _spell != null);
    }

    public void CastSpellIfInputPressed()
    {
        Assert.IsTrue(enabled);
        if (Spell == null) return;

        if (_triggerInput.ToInputAction().WasPressedThisFrame())
        {
            _spellQueue.QueueTrigger(Spell);
            UpdateCooldownDisplay();
        }
    }

    private void UpdateCooldownDisplay()
    {
        if (!enabled) return;

        if (_spell == null)
        {
            _cooldownParent.SetActive(false);
            return;
        }

        // Display cooldown
        int cd = _spell.CurrentCooldown;
        if (cd > 0)
        {
            _cooldownText.text = cd + " Turn" + (cd == 1 ? "" : "s");
        }
        _cooldownParent.SetActive(cd > 0);
    }
}
