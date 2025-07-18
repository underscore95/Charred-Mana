using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

// Settings for InteractableObject
public class InteractableSettings : MonoBehaviour
{
    [SerializeField] private InputActionReference _action;
    public InputAction Action { get { return _action; } }

    [SerializeField] private GameObject _uiPrefab;
    public GameObject UiPrefab { get { return _uiPrefab; } }

    private void Awake()
    {
        Assert.IsNotNull(_action);
        Assert.IsNotNull(_uiPrefab);
    }
}
