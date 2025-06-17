using UnityEngine;

public class SpellReward : Reward
{
    [SerializeField] private PlayerSpell _spell;
    private SpellManager _spellManager;

    private void Start()
    {
        _spellManager = FindAnyObjectByType<SpellManager>();

        if (Utils.StringNotEmpty(_title) || Utils.StringNotEmpty(_description))
        {
            Debug.LogWarningFormat("{0} spell reward had a title or description, this will be set automatically to the spell title/description and should be left blank in inspector.", gameObject.name);
        }

        _title = _spell.gameObject.name + " Spell";
        _description = _spell.OptionalDescription;
    }

    public override void Give()
    {
        _spellManager.UnlockSpell(_spellManager.GetSpellIndex(_spell));
    }

    public override bool CanGive()
    {
        bool unlocked = _spellManager.IsSpellUnlocked(_spell);
        return !unlocked;
    }
}
