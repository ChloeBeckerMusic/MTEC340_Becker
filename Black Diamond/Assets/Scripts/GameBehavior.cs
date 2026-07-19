using System.Collections;
using UnityEngine;
using TMPro;

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
     }

 }


 [SerializeField] private TMP_Text _message;
 private float _durationBetweenPoints = 0.3f;


 [SerializeField] private AudioSource _audioSource;
 [SerializeField] private AudioClip _skiFenceHit;

 private GameObject _currentPlayer;
 [SerializeField] private GameObject _playerPrefab;
 [SerializeField] private TextMeshProUGUI _scoreTextUI;
 
 
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
                Destroy(gameObject);
        }
    }
    // --------------------------------------------------------------------------------------------- UPDATE
    private void Start()
    {
        ResetGame();
        
        _audioSource = GetComponent<AudioSource>();
        // State = Utilities.GameState.Play;
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
    private void IncreaseScore()
    {
        _score++;
    }


    // --------------------------------------------------------------------------------------------- UPDATE 
    public void ResetGame()
    {
        Debug.Log("Game reset!");

        if (_currentPlayer != null)
        {
            Destroy(_currentPlayer);
        }

        Score = 0;
        Invoke(nameof(SpawnPlayer), _durationBetweenPoints);
    }


// --------------------------------------------------------------------------------------------- COROUTINES

    public IEnumerator ResetAfterFenceHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        ResetGame();
    }

    public IEnumerator ResetAfterChairliftHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        ResetGame();
    }

    public IEnumerator ResetAfterRockTowerHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        ResetGame();
    }
// --------------------------------------------------------------------------------------------- END BRACKET
}
