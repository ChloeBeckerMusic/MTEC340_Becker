using UnityEngine;

public class WaffleBehavior : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private float _leftEdge;
    private Spawner _spawner;

// --------------------------------------------------------------------------------------------- 


    private void Start()
    {
        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;

        _spawner = FindFirstObjectByType<Spawner>();
    }

// --------------------------------------------------------------------------------------------- 

    private void Update()
    {
        if (GameBehavior.Instance.State != Utilities.GameState.Play)
            return;

        transform.position += _speed * Time.deltaTime * Vector3.left;           // be aware of the "inefficient" order

        if (transform.position.x < _leftEdge)
        {
            _spawner.WaffleMissed();
            Destroy(gameObject);                                             // we don't need that shit anymore
        }
    }

// --------------------------------------------------------------------------------------------- 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
                                                                                // sending out the signal
                                                                                // that player collected the waffle 
        GameBehavior.Instance.PlayerCollectedWaffle();                  
        _spawner.WaffleCollected();
        Destroy(gameObject);
    }

// --------------------------------------------------------------------------------------------- END BRACKET
}