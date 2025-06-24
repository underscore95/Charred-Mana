using System.Collections;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private float _seconds = 5;

    private void Start()
    {
        StartCoroutine(DestroyLater());
    }

    private IEnumerator DestroyLater()
    {
        yield return new WaitForSeconds(_seconds);
        Destroy(gameObject);
    }
}
