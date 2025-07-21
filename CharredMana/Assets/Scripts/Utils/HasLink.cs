using UnityEngine;

public class HasLink : MonoBehaviour
{
    [SerializeField] private string _link = "";

    public void OpenLink()
    {
        Application.OpenURL(_link);
    }
}
