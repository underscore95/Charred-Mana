using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo;

    public GameSave Save { get; private set; } = null;
    private UnityAction _runAfterSaveLoaded = () => { };

    private string GetSaveFileName()
    {
        return $"Saves/GameData_{_gameInfo.GetVersionIdentifier()}.json";
    }

    private void Awake()
    {
        Save = JsonUtils.Load<GameSave>(GetSaveFileName(), false);
        Save ??= new();
        _runAfterSaveLoaded.Invoke();
        _runAfterSaveLoaded = null;
    }

    private void OnDestroy()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        JsonUtils.Save(GetSaveFileName(), Save);
    }

    /// <summary>
    /// Run the action after the save is loaded, if the save is already loaded then run the action right away
    /// </summary>
    public void RunAfterSaveLoaded(UnityAction action)
    {
        if (Save == null) _runAfterSaveLoaded += action;
        else action.Invoke();
    }
}
