using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Timeline;

public class GameBehavior : MonoBehaviour
{

    public static GameBehavior Instance;

    private Utilities.GameState _state;

    public Utilities.GameState State
    {
        get => _state;

        set
        {
            _state = value;

            _playButtonHome.SetActive(State == Utilities.GameState.Menu);

            _playButtonGameOver.SetActive(State == Utilities.GameState.GameOver);

            _gameOverButton.SetActive(State == Utilities.GameState.GameOver);

            _gameOverBackground.SetActive(State == Utilities.GameState.GameOver);
            
            _pauseImage.SetActive(State == Utilities.GameState.Pause);

            _spawner.enabled = State == Utilities.GameState.Play;

            _characterImage.gameObject.SetActive(State == Utilities.GameState.GameOver);


        }
    }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _skiFenceHit;
    [SerializeField] private AudioClip _waffleEat;


    [SerializeField] private GameObject _pauseImage;
   
    [SerializeField] private Image _dialogueImageLong;
    [SerializeField] private Image _dialogueImageShort;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Image _waffleIcon;

    [SerializeField] private Sprite _wholeWaffle;
    [SerializeField] private Sprite _oneBiteWaffle;
    [SerializeField] private Sprite _twoBiteWaffle;


    private Sprite _currentClosed;
    private Sprite _currentOpen;

    [SerializeField] private Sprite _defaultClosed;
    [SerializeField] private Sprite _defaultOpen;
    [SerializeField] private Sprite _defaultDialogue;

    [SerializeField] private Sprite _passerbyClosed;
    [SerializeField] private Sprite _passerbyOpen;
    [SerializeField] private Sprite _passerbyDialogue;

    [SerializeField] private Sprite _sisterClosed;
    [SerializeField] private Sprite _sisterOpen;
    [SerializeField] private Sprite _sisterDialogue;

    [SerializeField] private Sprite _lilbroClosed;
    [SerializeField] private Sprite _lilbroOpen;
    [SerializeField] private Sprite _lilbroDialogue;


    private GameObject _currentPlayer;
    [SerializeField] public GameObject _playerPrefab;
    [SerializeField] private TextMeshProUGUI _gameplayScoreText;
    [SerializeField] private TextMeshProUGUI _gameOverScoreText;
    [SerializeField] private GameObject _gameOverBackground;

    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameObject _playButtonGameOver;
    [SerializeField] private GameObject _playButtonHome;
    [SerializeField] private GameObject _gameOverButton;

    private int _nextWaffleGate = 21 + 7;
    private bool _waffleExists = false;
    
    private bool _hasWaffle = false; // does the player have a waffle rn

    public bool HasWaffle => _hasWaffle;

    public bool UseWaffle()
    {
        if (!_hasWaffle)
            return false;
      
        _hasWaffle = false;
       
        Player player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            player.SetWaffleBoost(false);
        }

        _audioSource.PlayOneShot(_waffleEat,0.35f);

        StartCoroutine(EatWaffleAnimation());

        return true;
    }

    private int _score;
    
     public int Score
    {
        get { return _score; }
        set 
        { 
            _score = value;      
            _gameplayScoreText.text = "Score: " + _score;
            _gameOverScoreText.text = "Score: " + _score;
        }
    }

//0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//

    // --------------------------------------------------------------------------------------------- UPDATE

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("New instance initialized...");
    
            DontDestroyOnLoad(gameObject);
        }
    
        else
        {
                Destroy(gameObject);
        }
    }
    // --------------------------------------------------------------------------------------------- UPDATE
    private void Start()      
    {
        _audioSource = GetComponent<AudioSource>();
        
        _dialogueImageLong.gameObject.SetActive(false);
        _dialogueImageShort.gameObject.SetActive(false);

        State = Utilities.GameState.Menu;
    }

// --------------------------------------------------------------------------------------------- UPDATE
    public void StartGame()
    {
        _dialogueImageLong.gameObject.SetActive(false);
        _dialogueImageShort.gameObject.SetActive(false);

        Score = 0;

        Spawner spawner = FindFirstObjectByType<Spawner>();

        if (spawner != null)
        {
            spawner.ResetSpawner();
        }

        SpawnPlayer();

        State = Utilities.GameState.Play;


    }
// --------------------------------------------------------------------------------------------- UPDATE

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            State = State == Utilities.GameState.Play ? 
                Utilities.GameState.Pause : 
                Utilities.GameState.Play;
        }
    }

    
    // --------------------------------------------------------------------------------------------- UPDATE
    private void SpawnPlayer()     
    {
        _currentPlayer = Instantiate(_playerPrefab);
    }
    
    // --------------------------------------------------------------------------------------------- UPDATE 
    public void IncreaseScore()
    {
        int points = 1;

        if (_hasWaffle)
        {
            points = 2;
        }

        Score += points;
    }

    public void PlayerCollectedWaffle()
    {
        _hasWaffle = true;

        Player player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            player.SetWaffleBoost(true);
        }

        _waffleIcon.sprite = _wholeWaffle;
        _waffleIcon.gameObject.SetActive(true);


        Debug.Log("Waffle collected!");
    }

    private IEnumerator EatWaffleAnimation()
    {
        _waffleIcon.sprite = _oneBiteWaffle;
        yield return new WaitForSeconds(0.25f);

        _waffleIcon.sprite = _twoBiteWaffle;
        yield return new WaitForSeconds(0.25f);

        _waffleIcon.gameObject.SetActive(false);
    }


    // --------------------------------------------------------------------------------------------- UPDATE 
    public void GameOver()       
    {
        State = Utilities.GameState.GameOver;
        
        ShowGameOverDialogue();

        if (_currentPlayer != null)
        {
            Destroy(_currentPlayer);
        }
        
        SkiPipes[] pipes = FindObjectsByType<SkiPipes>(FindObjectsSortMode.None);

        foreach (SkiPipes pipe in pipes)
        {
            Destroy(pipe.gameObject);
        }
    }

    private void ShowGameOverDialogue()
    {
        
        if (Score <= 10)
        {
            _currentClosed = _defaultClosed;
            _currentOpen = _defaultOpen;
            _dialogueImageShort.sprite = _defaultDialogue;
            _dialogueImageShort.gameObject.SetActive(true);
            _dialogueImageLong.gameObject.SetActive(false);
        }
        else if (Score <= 35)
        {
            _currentClosed = _passerbyClosed;
            _currentOpen = _passerbyOpen;
            _dialogueImageShort.sprite = _passerbyDialogue;
            _dialogueImageShort.gameObject.SetActive(true);
            _dialogueImageLong.gameObject.SetActive(false);
        }
        else if (Score <= 50)    
        {
            _currentClosed = _sisterClosed;
            _currentOpen = _sisterOpen;
            _dialogueImageLong.sprite = _sisterDialogue;
            _dialogueImageShort.gameObject.SetActive(false);
            _dialogueImageLong.gameObject.SetActive(true);
        }
        else     // 51+
        {
            _currentClosed = _lilbroClosed;
            _currentOpen = _lilbroOpen;
            _dialogueImageLong.sprite = _lilbroDialogue;
            _dialogueImageShort.gameObject.SetActive(false);
            _dialogueImageLong.gameObject.SetActive(true);
        }

        _characterImage.sprite = _currentClosed;

        StartCoroutine(AnimateDialogue());
    }


    private IEnumerator AnimateDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
        {
            _characterImage.sprite = _currentOpen;
            yield return new WaitForSeconds(0.2f);

            _characterImage.sprite = _currentClosed;
            yield return new WaitForSeconds(0.2f);
        }

        _characterImage.sprite = _currentClosed;
    }

// --------------------------------------------------------------------------------------------- COROUTINES

    public IEnumerator GameOverAfterFenceHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);  
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

    public IEnumerator GameOverAfterChairliftHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

    public IEnumerator GameOverAfterRockTowerHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

// --------------------------------------------------------------------------------------------- END BRACKET
}
