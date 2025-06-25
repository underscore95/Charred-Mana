using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class StatsText : MonoBehaviour
{
    [SerializeField] private string _mainMenu = "MainMenu";
    private TextMeshProUGUI _statsText;
    private string _statsTextOriginalContents;

    private void OnEnable()
    {
        if (_statsText == null)
        {
            _statsText = GetComponent<TextMeshProUGUI>();
            _statsTextOriginalContents = _statsText.text;
        }

        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        _statsText.text = "";

        yield return new WaitForEndOfFrame();
        Player player = FindAnyObjectByType<Player>();
        TurnManager turnManager = FindAnyObjectByType<TurnManager>();

        _statsText.text = string.Format(
          _statsTextOriginalContents,
           turnManager.CurrentTurn,
            player.MonstersKilled,
            player.PlayerLevel.TotalExperienceGained,
            player.SpellsCast
        );
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(_mainMenu, LoadSceneMode.Single);
    }
}
