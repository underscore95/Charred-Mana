using System;
using System.Collections;
using System.Collections.Generic;
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
        public CurrencyType SelectedCurrency = CurrencyType.Essence;
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
    [SerializeField] private CurrencyDisplay _currencyDisplay;
    [SerializeField] private TMP_Dropdown _selectedCurrency;
    [SerializeField] private Button _setCurrency0;
    [SerializeField] private Button _addCurrency1;
    [SerializeField] private Button _addCurrency10;
    [SerializeField] private Button _addCurrency100;
    [SerializeField] private Button _addCurrency1000;
    [SerializeField] private Button _addCurrency10000;
    [SerializeField] private Button _subtractCurrency1;
    [SerializeField] private Button _subtractCurrency10;
    [SerializeField] private Button _subtractCurrency100;
    [SerializeField] private Button _subtractCurrency1000;
    [SerializeField] private Button _subtractCurrency10000;
    [SerializeField] private TMP_Dropdown _selectedSpellToUnlock;
    [SerializeField] private Button _unlockSpellButton;
    [SerializeField] private TextMeshProUGUI _playedRunsText;
    [SerializeField] private TMP_InputField _playedRunsInput;
    [SerializeField] private Button _playedRunsButton;
    [SerializeField] private Button _sendPlayerMessageButton;
    [SerializeField]
    private List<string> _playerMessages = new() {
        "Kinda sorta a little long laceholder message 1",
        "Very very very very very very very very very very very very very very very very very very very very very long placeholder message 2",
        "msg 3"
    };

    public DebugOptions Options { get; private set; }
    private bool _wasDebugMenuOpenLastFrame = false;
    private FloorManager _floorManager;
    private CurrencyManager _currencyManager;
    private SpellManager _spellManager;
    private readonly List<PlayerSpell> _lockedSpells = new();
    private RunManager _runManager;
    private PlayerMessageManager _playerMessageManager;

    private void Awake()
    {
        _floorManager = FindAnyObjectByType<FloorManager>();
        _spellManager = FindAnyObjectByType<SpellManager>();
        _currencyManager = FindAnyObjectByType<CurrencyManager>();
        _runManager = FindAnyObjectByType<RunManager>();
        _playerMessageManager = FindAnyObjectByType<PlayerMessageManager>();

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

        // Num played runs
        if (_runManager != null)
        {
            _playedRunsText.text = _runManager.GetPlayedRuns() + " Played Runs";
            _playedRunsInput.text = _runManager.GetPlayedRuns().ToString();
            _playedRunsButton.onClick.AddListener(() =>
            {
                if (int.TryParse(_playedRunsInput.text, out int run))
                {
                    if (run < 0)
                    {
                        Debug.LogError($"[DebugMenu] {_playedRunsInput.text} must be >= 0 to set current played runs");
                        return;
                    }

                    _runManager.SetPlayedRuns(run);
                    Debug.Log($"[DebugMenu] You now have {_playedRunsInput.text} played runs.");
                }
                else
                {
                    Debug.LogError($"[DebugMenu] Failed to parse {_playedRunsInput.text} as an int");
                }
            });
        }

        // Music
        _nextMusicButton.onClick.AddListener(() => { print("[DebugMenu] Next music track"); FindAnyObjectByType<MusicPlayer>().PlayNextMusic(); });

        // Scenes
        _goToGameButton.onClick.AddListener(() => { print("[DebugMenu] Loaded game scene"); SceneManager.LoadScene("Game", LoadSceneMode.Single); });
        _goToMenuButton.onClick.AddListener(() => { print("[DebugMenu] Loaded main menu scene"); SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); });

        // Floors
        if (_floorManager != null)
        {
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

            StartCoroutine(GoToStartingFloorIfUsing());

            // Player messages
            if (_playerMessageManager != null)
            {
                _sendPlayerMessageButton.onClick.AddListener(() =>
                {
                    string message = _playerMessages.Count > 0 ? _playerMessages[UnityEngine.Random.Range(0, _playerMessages.Count)] : "Debug Player Message";
                    _playerMessageManager.SendPlayerMessage(message);
                    Debug.Log($"[DebugMenu] Sent player message '{message}'.");
                });
            }
        }

        // Spells
        #region UnlockSpell
        if (_spellManager != null)
        {
            RefreshUnlockSpellDropdown();

            _unlockSpellButton.onClick.RemoveAllListeners();
            _unlockSpellButton.onClick.AddListener(() =>
            {
                if (_lockedSpells.Count == 0) return;

                int selectedIndex = _selectedSpellToUnlock.value;
                PlayerSpell selectedSpell = _lockedSpells[selectedIndex];

                if (_spellManager.IsSpellUnlocked(selectedSpell))
                {
                    Debug.Log("[DebugMenu] Spell already unlocked: " + selectedSpell);
                    return;
                }

                int slot = _spellManager.GetUnusedSelectedSlot();
                if (slot < 0)
                {
                    Debug.LogWarning("[DebugMenu] No available spell slot.");
                    return;
                }

                _spellManager.UnlockSpell(_spellManager.GetSpellIndex(selectedSpell));
                _spellManager.SelectSpell(slot, selectedSpell);
                Debug.Log("[DebugMenu] Unlocked and assigned spell: " + selectedSpell + " to slot " + slot);

                RefreshUnlockSpellDropdown();
            });

            _spellManager.OnSpellUnlock += RefreshUnlockSpellDropdown;
        }
        #endregion

        // Currency
        if (_currencyManager != null)
        {
            #region Currency
            List<TMP_Dropdown.OptionData> currencyDropdownOptions = new();
            foreach (CurrencyType currency in Enum.GetValues(typeof(CurrencyType)))
            {
                currencyDropdownOptions.Add(new(currency.ToString()));
            }
            _selectedCurrency.AddOptions(currencyDropdownOptions);
            _selectedCurrency.SetValueWithoutNotify((int)Options.SelectedCurrency);
            _selectedCurrency.onValueChanged.AddListener(newCurrency =>
            {
                Options.SelectedCurrency = (CurrencyType)newCurrency;
                _currencyDisplay.Currency = Options.SelectedCurrency;
                print("[DebugMenu] Selected currency" + Options.SelectedCurrency);
            });
            _currencyDisplay.Currency = Options.SelectedCurrency;

            #region Currency Buttons

            _setCurrency0.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Set {Options.SelectedCurrency} to 0");
                _currencyManager.Set(Options.SelectedCurrency, 0);
            });

            #region Add Currency
            _addCurrency1.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Add 1 to {Options.SelectedCurrency}");
                _currencyManager.Add(Options.SelectedCurrency, 1);
            });
            _addCurrency10.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Add 10 to {Options.SelectedCurrency}");
                _currencyManager.Add(Options.SelectedCurrency, 10);
            });
            _addCurrency100.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Add 100 to {Options.SelectedCurrency}");
                _currencyManager.Add(Options.SelectedCurrency, 100);
            });
            _addCurrency1000.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Add 1000 to {Options.SelectedCurrency}");
                _currencyManager.Add(Options.SelectedCurrency, 1000);
            });
            _addCurrency10000.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Add 10000 to {Options.SelectedCurrency}");
                _currencyManager.Add(Options.SelectedCurrency, 10000);
            });
            #endregion

            #region Subtract Currency
            _subtractCurrency1.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Subtract 1 from {Options.SelectedCurrency}");
                _currencyManager.Remove(Options.SelectedCurrency, 1);
            });
            _subtractCurrency10.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Subtract 10 from {Options.SelectedCurrency}");
                _currencyManager.Remove(Options.SelectedCurrency, 10);
            });
            _subtractCurrency100.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Subtract 100 from {Options.SelectedCurrency}");
                _currencyManager.Remove(Options.SelectedCurrency, 100);
            });
            _subtractCurrency1000.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Subtract 1000 from {Options.SelectedCurrency}");
                _currencyManager.Remove(Options.SelectedCurrency, 1000);
            });
            _subtractCurrency10000.onClick.AddListener(() =>
            {
                Debug.Log($"[DebugMenu] Subtract 10000 from {Options.SelectedCurrency}");
                _currencyManager.Remove(Options.SelectedCurrency, 10000);
            });
            #endregion

            #endregion
        }
        #endregion

    }

    private IEnumerator GoToStartingFloorIfUsing()
    {
        if (DEBUG && Options.StartOnFloor && Options.StartingFloor != 0)
        {
            yield return new WaitForEndOfFrame();
            print("[DebugMenu] Starting on floor " + Options.StartingFloor);
            while (_floorManager.CurrentFloor != Options.StartingFloor)
            {
                yield return new WaitForEndOfFrame();
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

    private void RefreshUnlockSpellDropdown()
    {
        _lockedSpells.Clear();
        _selectedSpellToUnlock.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new();
        foreach (PlayerSpell spell in _spellManager.GetAllSpells())
        {
            if (!_spellManager.IsSpellUnlocked(spell))
            {
                _lockedSpells.Add(spell);
                options.Add(new(spell.ToString()));
            }
        }

        _selectedSpellToUnlock.AddOptions(options);
        _selectedSpellToUnlock.SetValueWithoutNotify(0);
        _unlockSpellButton.interactable = _lockedSpells.Count > 0;
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
