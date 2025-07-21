using UnityEngine;

public class Ui : MonoBehaviour
{
    [SerializeField] private Canvas _hubUi;

    private void Update()
    {
        if (_hubUi)
        {
            _hubUi.enabled = !ShouldHideHubUi();
        }
    }

    private bool ShouldHideHubUi()
    {
        return UIState.IsAnyUiOpen();
    }
}
