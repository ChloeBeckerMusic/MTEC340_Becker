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

    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private Player[] _players = new Player[2];
    [SerializeField] private int _targetScore = 3; 

    [SerializeField] private GameObject _ballPrefab;

    private float _durationBetweenPoints = 1.0f; 


               // GameBehavior here is like an access point (because of the word static)
               // static means that hey this line of code belongs to the CLASS,
               // not to the instance-- there is only ONE instance

// 000000000000000000000000000000000000000000000000000000000000000000000000000000000000 //


// --------------------------------------------------------------------------------- AWAKE
    private void Awake()
    {
        // Software Design Patterns
        // Singleton Pattern: Enforces that there is only ever one class
        // throughout the whole execution of the program
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

// --------------------------------------------------------------------------------- START
    private void Start()
    {
        ResetGame();

        // Set initial state
        State = Utilities.GameState.Play;
    }

// --------------------------------------------------------------------------------- UPDATE
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // ternary operator 
            State = State == Utilities.GameState.Play ?     // condition 
                Utilities.GameState.Pause :            // passing condition 
                Utilities.GameState.Play;                // failing condition 
        }
    }

// --------------------------------------------------------------------------------- RESET GAME
    private void ResetGame()
    {
        foreach (Player p in _players)
        {
            p.Score = 0;
        }

        SpawnBall();
    }

// --------------------------------------------------------------------------------- SPAWN BALL
    private void SpawnBall()
    {
        Instantiate(_ballPrefab);

    }

// --------------------------------------------------------------------------------- SCORE
    public void Score(int playerNum)
    {
        _players[playerNum - 1].Score++;
        CheckWinner();
    }

// --------------------------------------------------------------------------------- CHECK WINNER
    private void CheckWinner()
    {
        foreach (Player p in _players)
        {
            if (p.Score >= _targetScore)
            {
                ResetGame();
                return;
            }
        }
       
        //apply a delay when a player scores to give it a respite 
        Invoke(nameof(SpawnBall), _durationBetweenPoints);

    }

// --------------------------------------------------------------------------------- DONE

}

