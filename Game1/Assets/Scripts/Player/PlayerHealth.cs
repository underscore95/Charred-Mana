using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthBarText;

    private Stats _stats;

    private void Start()
    {
        _stats = transform.parent.GetComponent<IHasStats>().GetStats();
    }

    private void Update()
    {
        int currentHealth = Mathf.CeilToInt(_stats.CurrentHealth);
        _healthBarText.text = string.Format("Health: {0} / {1}", currentHealth, Mathf.RoundToInt(_stats.Get(StatType.MaxHealth)));
    }
}
