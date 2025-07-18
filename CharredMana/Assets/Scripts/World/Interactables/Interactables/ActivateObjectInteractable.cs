using UnityEngine;

public class ActivateObjectInteractable : InteractableObject
{
    [SerializeField] private GameObject[] _activateWhenInteractedWith;
    [SerializeField] private bool _toggleActivationState = false; // If true, toggle activation state, if false, always activate

    protected new void Awake()
    {
        base.Awake();
        if (_activateWhenInteractedWith == null || _activateWhenInteractedWith.Length == 0)
        {
            Debug.LogWarning($"{name} won't activate any objects when interacted with, add an object using the inspector or remove this component if it isn't necessary");
        }
    }

    protected override void OnInteract()
    {
        if (_activateWhenInteractedWith != null)
        {
            foreach (GameObject go in _activateWhenInteractedWith)
            {
                go.SetActive(_toggleActivationState ? !go.activeInHierarchy : true);
            }
        }
    }
}
