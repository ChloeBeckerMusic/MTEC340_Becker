using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private float hideXPosition = -10f;

    private PipeSpawner spawner;

    private ScoreZone scoreZone;

    private void Awake()
    {
        scoreZone = GetComponentInChildren<ScoreZone>();
    }

    private void OnEnable()
    {
        scoreZone.ResetZone();
    }

    public void Initialize(PipeSpawner pipeSpawner)
    {
        spawner = pipeSpawner;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing)
            return;

        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < hideXPosition)
        {
            spawner.ReturnToPool(this);
        }
    }
   
}