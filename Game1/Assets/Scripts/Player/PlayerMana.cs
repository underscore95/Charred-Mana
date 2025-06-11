using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _manaBarText;

    public float MaxMana { get; private set; } = 100;
    public float Mana { get; private set; }

    private void Awake()
    {
        Mana = MaxMana;
        UpdateText();
    }

    public void RemoveMana(float amount)
    {
        Assert.IsTrue(amount > 0);
        Mana -= amount;

        UpdateText();
    }

    private void UpdateText()
    {
        _manaBarText.text = string.Format("Mana: {0} / {1}", Mathf.RoundToInt(Mana), Mathf.RoundToInt(MaxMana));
    }
}
