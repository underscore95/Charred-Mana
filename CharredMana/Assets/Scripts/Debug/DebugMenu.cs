using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    private static readonly bool DEBUG = true;
    private static readonly string PATH = "DebugOptions.json";

    public class DebugOptions
    {
        public bool CanDie = true;
    }

    [SerializeField] private GameObject _debugBuildText;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Toggle _canDie;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _levelUp10Button;
    [SerializeField] private Button _killPlayerButton;

    public DebugOptions Options { get; private set; }

    private void Awake()
    {
        if (_debugBuildText)
        {
            _debugBuildText.SetActive(DEBUG);
        }

        _canvas.SetActive(false);

        if (DEBUG)
        {
            Options = JsonUtils.Load<DebugOptions>(PATH);
        }
        Options ??= new();

        _canDie.isOn = Options.CanDie;

        Player player = FindAnyObjectByType<Player>();
        _levelUpButton.enabled = player != null;
        _levelUp10Button.enabled = player != null;
        if (player != null)
        {
            _levelUpButton.onClick.AddListener(() => player.PlayerLevel.ForceLevelUp());
            _levelUp10Button.onClick.AddListener(() =>
                {
                    for (int i = 0; i < 10; i++) player.PlayerLevel.ForceLevelUp();
                });
        }

        _killPlayerButton.onClick.AddListener(() => LivingEntity.Damage(player, player.EntityStats.MaxHealth));
    }

    private void OnDestroy()
    {
        if (DEBUG)
        {
            JsonUtils.Save(PATH, Options);
        }
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.H))
        {
            _canvas.SetActive(!_canvas.activeInHierarchy);
        }

        if (!_canvas.activeInHierarchy) return;

        Options.CanDie = _canDie.isOn;
    }
}
