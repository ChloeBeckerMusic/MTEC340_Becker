using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;

    private Vector3 _direction;
    public float Gravity = -9.81f;
    public float Strength = 5f;

    private SpriteRenderer _spriteRenderer;
    public Sprite[] _sprites;
    private int _spriteIndex;
    
    private AudioSource _source; // this is the regular audio source
    [SerializeField] private AudioSource _audioSource;    // this is the one for the coroutine
    [SerializeField] private AudioClip _skiFenceHit; 
    [SerializeField] private AudioClip _chairliftHit;
    [SerializeField] private AudioClip _rockTowerHit;
    [SerializeField] private AudioClip _scoreArea;
    
    //0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//
// --------------------------------------------------------------------------------------------- AWAKE

 private void Awake()
 {
_spriteRenderer = GetComponent<SpriteRenderer>();
_source = GetComponent<AudioSource>();
_rb = GetComponent<Rigidbody2D>();
 }
 
// --------------------------------------------------------------------------------------------- START
 private void Start()
 {

     InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
 }
                                         // time = how long Start waits before the first invocation
                                         // repeat rate is interval in seconds between repeat rates


// --------------------------------------------------------------------------------------------- UPDATE

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            _direction = Vector3.up * Strength;
        }
        
        _direction.y += Gravity * Time.deltaTime;
        transform.position += _direction * Time.deltaTime;
        
                                            // this makes sure that gravity is an acceleration
                                            // which is why we're multiplying it both times

        _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play;
    }

// --------------------------------------------------------------------------------------------- ANIMATE SPRITE

    private void AnimateSprite()
    {
        _spriteIndex++;
        {
            if (_spriteIndex >= _sprites.Length)
            {
                _spriteIndex = 0;
                                            // moving between sprites, and when you get to the end frame,
                                            // (aka spriteIndex is greater than the length)
                                            // go back to zero to restart the animation
            }
            
            _spriteRenderer.sprite = _sprites[_spriteIndex];
        }
    }

// ---------------------------------------------------------------------------------------------  AUDIO
     private void OnCollisionEnter2D(Collision2D other)
     {
         
         if (other.gameObject.CompareTag("ScoreArea"))  // while playing: ping!
         {
             _source.clip = _scoreArea;
         }  

         _source.pitch = Random.Range(0.9f, 1.1f);
         _source.Play();
     }

    // ---------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)     // these are the sounds before reset game
    {
    if (other.CompareTag("SkiFence"))             // losing SFX-- this is going to be a longer sound effect
        {
            Debug.Log("You hit the fence!");
            _audioSource.PlayOneShot(_skiFenceHit, 1f);
            StartCoroutine(GameBehavior.Instance.ResetAfterFenceHit());
            
        }
    else if (other.gameObject.CompareTag("Chairlift"))  //angry "hey!!"
        {
            Debug.Log("You hit the chairlift!");
            _audioSource.PlayOneShot(_chairliftHit, 1f);
            StartCoroutine(GameBehavior.Instance.ResetAfterChairliftHit());
        } 

    if (other.gameObject.CompareTag("RockTower"))  // rock crumble 
        {
            Debug.Log("You hit the chairlift!");
            _audioSource.PlayOneShot(_chairliftHit, 1f);
            StartCoroutine(GameBehavior.Instance.ResetAfterRockTowerHit());
        }


    }

// --------------------------------------------------------------------------------------------- END BRACKET

    }

