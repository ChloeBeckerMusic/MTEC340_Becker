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


    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            Debug.Log("New instance initialized...");
		
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        ResetGame();

        // Set initial state
        State = Utilities.GameState.Play;
    }

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

    private void ResetGame()
    {
        foreach (Player p in _players)
        {
            if (p != null)
            {
                p.Score = 0;
            }
            else
            {
                Debug.LogError("A Player reference is missing in the Players array!");
            }
        }

        SpawnBall();
    }


    private void SpawnBall()
    {
        Instantiate(_ballPrefab);

    }

    public void Score(int playerNum)
    {
        _players[playerNum - 1].Score++;
        CheckWinner();
    }


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
}

