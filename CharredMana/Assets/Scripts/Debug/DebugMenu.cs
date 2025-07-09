using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    private static readonly bool DEBUG = true;
    private static readonly string PATH = "DebugOptions.json";
    private static bool _debugMenuWasOpenOnLastScene = false;

    public class DebugOptions
    {
        public bool CanDie = true;
        public bool StartOnFloor = false;
        public int StartingFloor = 0;
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
    [SerializeField] private Toggle _startOnFloorToggle;
    [SerializeField] private TMP_InputField _startingFloorInput;
    [SerializeField] private Button _nextFloorButton;

    public DebugOptions Options { get; private set; }
    private bool _wasDebugMenuOpenLastFrame = false;
    private FloorManager _floorManager;

    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();

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
    }

    private void Start()
    {
        // Player
        Player player = FindAnyObjectByType<Player>();
        _canDie.SetIsOnWithoutNotify(Options.CanDie);
        _canDie.onValueChanged.AddListener(on => Options.CanDie = on);
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

        // Floors
        _nextFloorButton.onClick.AddListener(() => { _floorManager.NextFloor(); });

        _startOnFloorToggle.SetIsOnWithoutNotify(Options.StartOnFloor);
        _startOnFloorToggle.onValueChanged.AddListener(on => Options.StartOnFloor = on);
        Options.StartingFloor = Mathf.Max(0, Options.StartingFloor);
        _startingFloorInput.SetTextWithoutNotify(Options.StartingFloor.ToString());
        _startingFloorInput.onValueChanged.AddListener(newText =>
        {
            // Get rid of any spaces, etc
            string cleanedInput = new string(newText
            .Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c) && c != '\u200B')
            .ToArray());

            // Parse as int
            if (int.TryParse(cleanedInput, out int startingFloor))
            {
                if (startingFloor >= 0)
                {
                    Options.StartingFloor = startingFloor;
                    print($"[DebugMenu] You will start on floor {startingFloor} next time you launch the game. (assuming the checkbox is checked)");
                }
                else
                {
                    Debug.LogWarning($"[DebugMenu] Starting floor ({startingFloor}) cannot be negative, using {Options.StartingFloor} as the starting floor.");
                }
            }
            else
            {
                Debug.LogWarning($"[DebugMenu] Failed to parse '{cleanedInput}' as an integer");
            }
        });

        if (DEBUG && Options.StartOnFloor && Options.StartingFloor != 0)
        {
            print("[DebugMenu] Starting on floor " + Options.StartingFloor);
            while (_floorManager.CurrentFloor != Options.StartingFloor)
            {
                _floorManager.NextFloor();
            }
        }
    }

    private void OnDestroy()
    {
        if (DEBUG)
        {
            _debugMenuWasOpenOnLastScene = _wasDebugMenuOpenLastFrame;
            JsonUtils.Save(PATH, Options);
        }
    }

    private void Update()
    {
        if (!DEBUG) return;
        _wasDebugMenuOpenLastFrame = _canvas.activeInHierarchy;

        bool isCtrlHPressed = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.H);
        if (_debugMenuWasOpenOnLastScene || isCtrlHPressed)
        {
            _debugMenuWasOpenOnLastScene = false;
            _canvas.SetActive(!_canvas.activeInHierarchy);
        }

        if (!_canvas.activeInHierarchy) return;
    }
}
