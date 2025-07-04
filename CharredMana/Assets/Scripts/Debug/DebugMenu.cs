using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Button _nextMusicButton;
    [SerializeField] private Button _goToMenuButton;
    [SerializeField] private Button _goToGameButton;

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

        // Buttons
        // Player
        Player player = FindAnyObjectByType<Player>();
        _levelUpButton.enabled = player != null;
        _levelUp10Button.enabled = player != null;
        _killPlayerButton.enabled = player != null;
        if (player != null)
        {
            _levelUpButton.onClick.AddListener(() => { print("[DebugMenu] Levelled player up once"); player.PlayerLevel.ForceLevelUp(); });
            _levelUp10Button.onClick.AddListener(() =>
                {
                    print("[DebugMenu] Levelled player up 10 times");
                    for (int i = 0; i < 10; i++) player.PlayerLevel.ForceLevelUp();
                });

            _killPlayerButton.onClick.AddListener(() => { print("[DebugMenu] Damaged player for " + player.EntityStats.MaxHealth); LivingEntity.Damage(player, player.EntityStats.MaxHealth); });
        }

        // Music
        _nextMusicButton.onClick.AddListener(() => { print("[DebugMenu] Next music track"); FindAnyObjectByType<MusicPlayer>().PlayNextMusic(); });

        // Scenes
        _goToGameButton.onClick.AddListener(() => { print("[DebugMenu] Loaded game scene"); SceneManager.LoadScene("Game", LoadSceneMode.Single); });
        _goToMenuButton.onClick.AddListener(() => { print("[DebugMenu] Loaded main menu scene"); SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); });
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
