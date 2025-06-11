using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMana PlayerMana { get; private set; }
    public Camera Camera {  get; private set; }
    public Vector3 PositionAtFrameStart { get; set; }

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();
        PlayerMana = GetComponentInChildren<PlayerMana>();
    }
}
