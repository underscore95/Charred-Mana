using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SpellListUi : MonoBehaviour
{
    struct Element
    {
        public PlayerSpell Spell;
        public RectTransform Rect;
        public Action OnClick;
    }

    [SerializeField] private GameObject _spellUiPrefab;
    [SerializeField] private InputActionReference _clickAction;

    private ScrollableArea _scrollableArea;
    private readonly List<Element> _spellList = new();
    public UnityAction<PlayerSpell> OnElementAdd = spell => { };
    public UnityAction<PlayerSpell> OnElementRemove = spell => { };

    private void Awake()
    {
        _scrollableArea = GetComponentInChildren<ScrollableArea>();
        Assert.IsNotNull(_scrollableArea);
        Assert.IsNotNull(_spellUiPrefab);
        Assert.IsNotNull(_spellUiPrefab.GetComponentInChildren<IHasSpell>());
        Assert.IsNotNull(_spellUiPrefab.GetComponent<RectTransform>());
    }

    private void Update()
    {
        if (!_clickAction.ToInputAction().WasPressedThisFrame()) return;

        Vector2 mousePos = Input.mousePosition;
        for (int i = 0; i < _spellList.Count; ++i)
        {
            int count = _spellList.Count;
            var element = _spellList[i];
            if (!RectTransformUtility.RectangleContainsScreenPoint(element.Rect, mousePos)) continue;
            element.OnClick.Invoke();
            if (count < _spellList.Count) --i;
        }
    }

    public void AddSpell(PlayerSpell spell, Action onClick)
    {
        GameObject ui = Instantiate(_spellUiPrefab, _scrollableArea.transform);
        ui.GetComponent<IHasSpell>().SetSpell(spell);

        Element e = new()
        {
            Spell = spell,
            Rect = ui.GetComponent<RectTransform>(),
            OnClick = onClick
        };

        _spellList.Add(e);
        OnElementAdd.Invoke(spell);
    }

    public void RemoveSpell(PlayerSpell spell)
    {
        for (int i = 0; i < _spellList.Count; i++)
        {
            if (_spellList[i].Spell == spell)
            {
                Destroy(_spellList[i].Rect.gameObject);
                _spellList.RemoveAt(i);
                break;
            }
        }
        OnElementRemove.Invoke(spell);
    }

    public bool ContainsSpell(PlayerSpell spell)
    {
        foreach (var element in _spellList)
        {
            if (element.Spell == spell) return true;
        }
        return false;
    }

    public void ClearSpells()
    {
        _spellList.Clear();
    }

    public int Size()
    {
        return _spellList.Count;
    }
}
