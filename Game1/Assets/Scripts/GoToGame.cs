using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
