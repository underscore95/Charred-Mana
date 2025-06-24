using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private string _gameScene;

    public void ShowMainWidget()
    {
        _main.SetActive(true);
        _credits.SetActive(false);
        _tutorial.SetActive(false);
    }

    public void ShowCreditsWidget()
    {
        _credits.SetActive(true);
        _main.SetActive(false);
    }

    public void ShowTutorialWidget()
    {
        _tutorial.SetActive(true);
        _main.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(_gameScene, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
