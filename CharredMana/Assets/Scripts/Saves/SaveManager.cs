using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    private static readonly string SAVE_FILE = "GameData.json";

    public GameSave Save { get; private set; } = null;
    private UnityAction _runAfterSaveLoaded = () => { };

    private void Awake()
    {
        Save = JsonUtils.Load<GameSave>(SAVE_FILE, false);
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
        JsonUtils.Save(SAVE_FILE, Save);
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
