using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverableSpellUi : MonoBehaviour, IHasSpell
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _manaCost;
    [SerializeField] private TextMeshProUGUI _desc;
    [SerializeField] private TextMeshProUGUI _cooldown;
    [SerializeField] private RectTransform _hoverArea;
    [SerializeField] private GameObject _enableOnHover;
    [SerializeField] private Color _normalColor = Color.blue;
    [SerializeField] private Color _hoverColor = Color.blue * 0.8f;
    [SerializeField] private Image _background;

    private PlayerSpell _spell;
    public PlayerSpell Spell
    {
        get { return _spell; }
        set
        {
            _spell = value;
            _name.text = _spell == null ? "None" : _spell.name;
            _manaCost.text = _spell == null ? "" : ("Mana Cost: " + _spell.ManaCost);
            _desc.text = _spell == null ? "" : _spell.OptionalDescription;
            if (_spell == null) _cooldown.text = "";
            else
            {
                if (_spell.SpellCooldown > 0) _cooldown.text = string.Format("Cooldown: {0} Turn{1}", _spell.SpellCooldown, _spell.SpellCooldown > 1 ? "s" : "");
                else _cooldown.text = "No Cooldown";
            }
        }
    }

    private void Awake()
    {
        _enableOnHover.SetActive(false);
    }

    private void Update()
    {
        if (Spell == null)
        {
            _enableOnHover.SetActive(false);
            return;
        }

        bool hovered = RectTransformUtility.RectangleContainsScreenPoint(_hoverArea, Input.mousePosition);
        _background.color = hovered ? _normalColor : _hoverColor;
        _enableOnHover.SetActive(hovered);
    }

    public void SetSpell(PlayerSpell spell)
    {
        Spell = spell;
    }
}
