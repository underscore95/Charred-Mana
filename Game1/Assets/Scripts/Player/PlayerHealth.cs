using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthBarText;

    public float MaxHealth { get; private set; } = 100;
    public float Health { get; private set; }

    private void Awake()
    {
        Health = MaxHealth;
        UpdateText();
    }

    public void Damage(float damage)
    {
        Assert.IsTrue(damage > 0);
        Health -= damage;

        UpdateText();
    }

    private void UpdateText()
    {
        _healthBarText.text = string.Format("Health: {0} / {1}", Mathf.RoundToInt(Health), Mathf.RoundToInt(MaxHealth));
    }
}
