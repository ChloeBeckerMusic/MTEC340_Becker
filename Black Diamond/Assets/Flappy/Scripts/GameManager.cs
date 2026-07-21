using System.Collections;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Start Screen")]
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject playbutton;

    [Header("Score")]
    [SerializeField] private TMP_Text score;

    [Header("Game Ready Section")]
    [SerializeField] private GameObject gameReadyPanel;

    [Header("Game Over Section")]
    [SerializeField] private GameObject gameOverPanel;
    
    [SerializeField] private TMP_Text gameOverScore;
    [SerializeField] private TMP_Text gameOverBestScore;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeAmount = 0.05f;


    [Space(10)]

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private PipeSpawner pipeSpawner;

    private const string BEST_SCORE_KEY = "BestScore";

    private GameState gameState = GameState.Home;
    public GameState GameState => gameState;

    private int currentScore;
    private Camera mainCamera;
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            score.text = currentScore.ToString();
        }
    }

    public int BestScore
    {
        get => PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        set
        {
            PlayerPrefs.SetInt(BEST_SCORE_KEY, value);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        mainCamera = Camera.main;
    }

    private void Start()
    {
        gameState = GameState.Home;
        playbutton.SetActive(true);
        logo.SetActive(true);
        gameOverPanel.SetActive(false);
        score.gameObject.SetActive(false);
        gameReadyPanel.SetActive(false);
    }

    public void PlayButton()
    {
        gameState = GameState.GetReady;
        CurrentScore = 0;

        logo.SetActive(false);
        playbutton.SetActive(false);

        gameOverPanel.SetActive(false);
        gameReadyPanel.SetActive(true);
                           
        score.gameObject.SetActive(true);
        ResetGame();
    }
    private void ResetGame()
    {
        pipeSpawner.ResetSpawner();
        player.ResetPlayer();
    }
    public void GamePlay()
    {
        gameState = GameState.Playing;
        gameReadyPanel.SetActive(false);
    }
    public void GameOver()
    {
        gameState = GameState.GameOver;

        StartCoroutine(ShakeCamera());
        gameOverPanel.SetActive(true);
        playbutton.SetActive(true);
        score.gameObject.SetActive(false);

        gameOverScore.text = CurrentScore.ToString();

        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }

        gameOverBestScore.text = BestScore.ToString();


    }
   
    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = mainCamera.transform.position;
        float timer = 0;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);

            mainCamera.transform.position = originalPos + new Vector3(x, y, 0);

            yield return null;
        }

        mainCamera.transform.position = originalPos;
    }
    public void AddScore()
    {
        if (gameState != GameState.Playing) return;

        CurrentScore++;

        AudioManager.Instance.Score();
    }
}