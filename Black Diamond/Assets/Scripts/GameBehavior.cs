using System.Collections;
using UnityEngine;
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
            _message.enabled = State == Utilities.GameState.Pause;

            _playButton.SetActive(State == Utilities.GameState.Menu ||
                                  State == Utilities.GameState.GameOver);

            _gameOver.SetActive(State == Utilities.GameState.GameOver);
        }
    }


    [SerializeField] private TMP_Text _message;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _skiFenceHit;

    private GameObject _currentPlayer;
    [SerializeField] public GameObject _playerPrefab;
    [SerializeField] private TextMeshProUGUI _scoreTextUI;
    
    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _gameOver;

    private int _score;
    
     public int Score
    {
        get { return _score; }
        set 
        { 
            _score = value;      
            _scoreTextUI.text = "Score: " + _score.ToString();
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
                DestroyImmediate(gameObject);
        }
    }
    // --------------------------------------------------------------------------------------------- UPDATE
    private void Start()      
    {
        _audioSource = GetComponent<AudioSource>();

        State = Utilities.GameState.Menu;
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
        // i want this to be called when they press play
    // so to call it I'd probably want Invoke(nameof(SpawnPlayer))
    {
        _currentPlayer = Instantiate(_playerPrefab);
    }
    

// --------------------------------------------------------------------------------------------- UPDATE
    // public void ResetGame()
    // {
    //     Debug.Log("Game Reset!");
    //
    //     if (_currentPlayer != null)
    //     {
    //         Destroy(_currentPlayer);
    //     }
    //
    //     Score = 0;
    // }

    // not sure i need this cause I'm going to have a start game and pause screen and those
    // will be able to say score = 0 and destroy current player if there is one and reinstatiate one (?)

    // --------------------------------------------------------------------------------------------- UPDATE 
    public void IncreaseScore()
    {
        _score++;
    }

    // --------------------------------------------------------------------------------------------- UPDATE 
    public void GameOver()       
    {
        _playButton.SetActive(true);
        _gameOver.SetActive(true);
        
        Debug.Log("Game over!");

        if (_currentPlayer != null)
        {
            Destroy(_currentPlayer);
        }

        // more here, right? say hey we need the pause screen to come up 
        // and I need the score to say 0 but different place so diff game object 
        // i think, and i also need to pause everything ??? 
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
