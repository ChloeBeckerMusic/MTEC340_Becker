using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Timeline;

public class GameBehavior : MonoBehaviour
{                                                                  // ... she's a mammoth, of course

    public static GameBehavior Instance;
    private Utilities.GameState _state;

    public Utilities.GameState State
    {
        get => _state;
        set                                                                      // setting which images/buttons can be
                                                                                 // active/inactive when we're in a certain state 
        {
            _state = value;
           // _playButtonHome.SetActive(State == Utilities.GameState.Menu);
            _playButtonGameOver.SetActive(State == Utilities.GameState.GameOver);
            _gameOverButton.SetActive(State == Utilities.GameState.GameOver);
            _gameOverBackground.SetActive(State == Utilities.GameState.GameOver);
            _pauseImage.SetActive(State == Utilities.GameState.Pause);
            _spawner.enabled = State == Utilities.GameState.Play;
            _characterImage.gameObject.SetActive(State == Utilities.GameState.GameOver);
        }
    }


    // AUDIO HERE: 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _skiFenceHit;
    [SerializeField] private AudioClip _waffleEat;
    [SerializeField] private AudioClip _waffleGet;
    [SerializeField] private AudioClip _chairliftHit;
    [SerializeField] private AudioClip _rockTowerHit;

   
    // UI IMAGES HERE: 
    [SerializeField] private GameObject _pauseImage;
   
    [SerializeField] private Image _dialogueImageLong;
    [SerializeField] private Image _dialogueImageShort;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Image _waffleIcon;

    
    // SPRITES HERE: 

                       //waffle
    [SerializeField] private Sprite _wholeWaffle;
    [SerializeField] private Sprite _oneBiteWaffle;
    [SerializeField] private Sprite _twoBiteWaffle;


                      // dialogue
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


    // GAME OBJECTS HERE: 
    private GameObject _currentPlayer;
    [SerializeField] public GameObject _playerPrefab;

    [SerializeField] private GameObject _gameOverBackground;
    [SerializeField] private GameObject _playButtonGameOver;
    [SerializeField] private GameObject _playButtonHome;
    [SerializeField] private GameObject _gameOverButton;


    // TEXT: 
    [SerializeField] private TextMeshProUGUI _gameplayScoreText;
    [SerializeField] private TextMeshProUGUI _gameOverScoreText;


    
    // WOULD YOU BELIEVE IT?? MORE WAFFLE. 
    [SerializeField] private Spawner _spawner;
    
    private bool _hasWaffle = false; // does the player have a waffle rn?

    public bool HasWaffle => _hasWaffle;   // if someone asks for HasWaffle, give them the value of _hasWaffle

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


    //SCORE: 

    private int _score;
    
     public int Score
    {
        get { return _score; }
        set 
        { 
            _score = value;      
            _gameplayScoreText.text = "Score: " + _score;
        }
    }

    // --------------------------------------------------------------------------------------------- 

    private void Awake()
    {
            Instance = this;
    }

    // --------------------------------------------------------------------------------------------- 

    private void Start()
    {
        _dialogueImageLong.gameObject.SetActive(false);
        _dialogueImageShort.gameObject.SetActive(false);

        State = Utilities.GameState.Play;

        Score = 0;
        SpawnPlayer();
    }

// --------------------------------------------------------------------------------------------- 

    public void StartGame()
    {
        State = Utilities.GameState.Play;

        _dialogueImageLong.gameObject.SetActive(false);
        _dialogueImageShort.gameObject.SetActive(false);

        if (_currentPlayer != null)
        {
            Destroy(_currentPlayer);
        }

        Score = 0;

        Spawner spawner = FindFirstObjectByType<Spawner>();

        if (spawner != null)
        {
            spawner.ResetSpawner();
        }

        SpawnPlayer();
    }

// --------------------------------------------------------------------------------------------- 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            State = State == Utilities.GameState.Play ? 
                Utilities.GameState.Pause : 
                Utilities.GameState.Play;
        }
    }
    
    // --------------------------------------------------------------------------------------------- 
    private void SpawnPlayer()
    {
        Debug.Log("Spawning player...");
        _currentPlayer = Instantiate(_playerPrefab);
        Debug.Log(_currentPlayer);
    }
    
    // --------------------------------------------------------------------------------------------- 

    public void IncreaseScore()
    {
        int points = 1;

        if (_hasWaffle)
        {
            points = 2;
        }

        Score += points;
    }

// --------------------------------------------------------------------------------------------- 

    public void PlayerCollectedWaffle()                                       // Waffle Power up Step 1: collect
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

    // --------------------------------------------------------------------------------------------- 

    public void GameOver()       
    {
        State = Utilities.GameState.GameOver;
        
        ShowGameOverDialogue();

        if (_currentPlayer != null)                             // DON'T FORGET THIS ONE !!! 
        {
            Destroy(_currentPlayer);
        }
        
        SkiPipes[] pipes = FindObjectsByType<SkiPipes>(FindObjectsSortMode.None);                // Array! Hurray!

        foreach (SkiPipes pipe in pipes)
        {
            Destroy(pipe.gameObject);
        }
    }

// --------------------------------------------------------------------------------------------- 
                                                                    // there's definitely be a better way to
                                                                    // do this but since it's a small
                                                                    // game, it's fine for now
    private void ShowGameOverDialogue()                                          
    {
        if (Score <= 10)
        {
            _currentClosed = _defaultClosed;
            _currentOpen = _defaultOpen;
            _dialogueImageShort.sprite = _defaultDialogue;
            _dialogueImageShort.gameObject.SetActive(true);                     
            _dialogueImageLong.gameObject.SetActive(false);                 // I could probably set this up
                                                                            // in the state machine for future
                                                                            // chloe reference 
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

// ---------------------------------------------------------------------------------------------   COROUTINES
                                                                        // important: so you the
                                                                        // audio doesn't get cut
                                                                        // off prematurely 

                                                                        // also helpful for the "speaking" so we can
                                                                        // wait for the character to finish speaking
                                                                        // before moving on
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

// --------------------------

    private IEnumerator EatWaffleAnimation()
    {
        _waffleIcon.sprite = _oneBiteWaffle;
        yield return new WaitForSeconds(0.25f);

        _waffleIcon.sprite = _twoBiteWaffle;
        yield return new WaitForSeconds(0.25f);

        _waffleIcon.gameObject.SetActive(false);
    }

// --------------------------

    public IEnumerator GameOverAfterFenceHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);  
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

    public IEnumerator GameOverAfterChairliftHit()
    {
        _audioSource.PlayOneShot(_chairliftHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

    public IEnumerator GameOverAfterRockTowerHit()
    {
        _audioSource.PlayOneShot(_rockTowerHit, 1f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

// --------------------------------------------------------------------------------------------- 

    public void Quit()
    {
        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame ()
    {
        yield return new WaitForSeconds(0.2f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

// --------------------------------------------------------------------------------------------- END BRACKET
}
