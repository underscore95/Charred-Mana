using UnityEngine;

public class PlayerEnemyTargeter : MonoBehaviour, IEnemyTargeter
{
    private Player _player;
    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
    }

    public ILivingEntity GetTarget()
    {
        return _player;
    }
}
