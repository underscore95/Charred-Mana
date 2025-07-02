using UnityEngine;

// Allows entity to display a red overlay when it is hurt
public class DisplayHurtOverlay : DisplayOverlay
{
    private static readonly float DURATION = 0.25f;
    private static readonly float TRANSITION_DURATION = 0.15f;
    private static readonly Color COLOR = new(0.8f, 0.0f, 0.0f);

    private void Awake()
    {
        GetComponent<LivingEntity>().OnDamaged() += _ => ApplyOverlay(COLOR, DURATION, TRANSITION_DURATION, TRANSITION_DURATION);
    }
}
