using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;

public class GameBehavior : MonoBehaviour
{

    public static GameBehavior Instance { get; private set; }

    [SerializeField] private TMP_Text _message;
    private float _durationBetweenPoints = 0.3f;


    

    public Player currentPlayer;
    public GameObject playerPrefab;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _gameOver;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _skiFenceHit;
 
    [SerializeField] private TextMeshProUGUI _scoreTextUI;
    private int _score;


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
        Instantiate(playerPrefab);
    }
    
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

        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        Score = 0;
        Invoke(nameof(SpawnPlayer), _durationBetweenPoints);
    }


// --------------------------------------------------------------------------------------------- COROUTINES

    public IEnumerator ResetAfterFenceHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

    public IEnumerator ResetAfterChairliftHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }

    public IEnumerator ResetAfterRockTowerHit()
    {
        _audioSource.PlayOneShot(_skiFenceHit, 0.35f);
        yield return new WaitForSeconds(_skiFenceHit.length);

        GameOver();
    }
// --------------------------------------------------------------------------------------------- END BRACKET
}
