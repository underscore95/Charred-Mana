using TMPro;
using UnityEngine;

public class GameVersionTextDisplay : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo;
    [SerializeField] private bool _includeName = true;
    [SerializeField] private bool _longVersion = true;

    private void Awake()
    {
        UpdateVersion();
    }

    private void UpdateVersion()
    {
        GetComponent<TextMeshProUGUI>().text = (_includeName ? _gameInfo.GameName + " " : "") + (_longVersion ? _gameInfo.GetLongVersion() : _gameInfo.GetShortVersion());
    }
}
