using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private float _launchForce = 7.0f;
    [SerializeField] private float _speedIncrement = 1.1f;

    private float _steepnessThresholdY = 0.75f;

    private Rigidbody2D _rb;
    
    [SerializeField, Range(0.0f, 1.0f)] private float _paddleInfluence = 0.4f;

    private AudioSource _source;
    [SerializeField] private AudioClip _wallHit;
    [SerializeField] private AudioClip _paddleHit;
    [SerializeField] private AudioClip _scorePoint;

     // 000000000000000000000000000000000000000000000000000000000000000000000000000000000000 //


     // -----------------------------------------------------------------------    START


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
       _source = GetComponent<AudioSource>();
       
      ResetBall();

    }


    // -----------------------------------------------------------------------  UPDATE


    void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play; 
        // simulated = boolean that is computed from the current game state 
    }


    // ----------------------------------------------------------------------- OnCollisionEnter2D


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            // Check if the paddle is moving
            if (!Mathf.Approximately(other.rigidbody.linearVelocity.y, 0.0f))
            {
                // Weighted sum using one-minus to calculate weights
                Vector2 direction = _rb.linearVelocity * (1.0f - _paddleInfluence)
                                    + other.rigidbody.linearVelocity * _paddleInfluence;
                
                _rb.linearVelocity = _rb.linearVelocity.magnitude * direction.normalized * _speedIncrement;
            }

            // Load sound
            _source.clip = _paddleHit;
        }
        else
        {
            // Load sound
            _source.clip = _wallHit;
        }
        
        // Randomize sound properties
        _source.pitch = Random.Range(0.9f, 1.1f);
        
        // Play sound
        _source.Play();
    }


    // ----------------------------------------------------------------------- OnTriggerEnter2D


    private void OnTriggerEnter2D(Collider2D other)
    {
        GameBehavior.Instance.Score(
            Mathf.Sign(transform.position.x) > 0 ? 1 : 2
        );
        
        // Load and play sound
        _source.clip = _scorePoint;
        _source.pitch = 1.0f;
        _source.Play();
        
        // The second argument of Destroy (optional) is used to delay
        // the action by a given amount
        
        Destroy(gameObject, _source.clip.length);
    }


    // ----------------------------------------------------------------------- RESET BALL


    private void ResetBall()
    {
        _rb.linearVelocity = Vector2.zero;
        
        transform.position = Vector3.zero;
        // transform.position = new Vector3(0.0f, 0.0f, transform.position.z);
        
        // Make sure that direction has a vector length of 1
        Vector2 direction = Random.insideUnitCircle.normalized;

        if (Mathf.Abs(direction.y) > _steepnessThresholdY)
        {
            direction.y -= _steepnessThresholdY * Mathf.Sign(direction.y);
            direction.Normalize();
        }
        
        _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
    }


    // -----------------------------------------------------------------------


}
