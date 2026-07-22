using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Sprite[] Sprites;
    [SerializeField] private Sprite _deathSprite;
    public float Strength = 5f;
    public float Gravity = -9.81f;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _direction;
    private int _spriteIndex;
    
    // dead and dead physics: 
    private bool _isDead = false;
    private Transform _attachedObject;
    private Vector3 _attachmentOffset;

    private Rigidbody2D _rb;
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

    private void OnEnable()
    {
        _isDead = false;
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        _direction = Vector3.zero;
    }

    
    // --------------------------------------------------------------------------------------------- UPDATE

    private void Update()
    {
        bool playing = GameBehavior.Instance.State == Utilities.GameState.Play;

        if (_isDead)
        {
            if (_attachedObject != null)
            {
                transform.position = _attachedObject.position + _attachmentOffset;
            }

            return;
        }

        if (_isDead)
            return;

        _rb.simulated = playing;

        if (!playing)
            return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                _direction = Vector3.up * Strength;
            }

            // apply gravity
            _direction.y += Gravity * Time.deltaTime;
            transform.position += _direction * Time.deltaTime;

    }

// --------------------------------------------------------------------------------------------- START


 private void Start()
 {
     if (GameBehavior.Instance.State == Utilities.GameState.Play)
     {
         InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
     }
 }
                                         // time = how long Start waits before the first invocation
                                         // repeat rate is interval in seconds between repeat rates



// --------------------------------------------------------------------------------------------- ANIMATE SPRITE

private void AnimateSprite()
        {
            if (GameBehavior.Instance.State != Utilities.GameState.Play)
                return;
        
        _spriteIndex++;

        {
            if (_spriteIndex >= Sprites.Length)
            {
                _spriteIndex = 0;
                                            // moving between sprites, and when you get to the end frame,
                                            // (aka spriteIndex is greater than the length)
                                            // go back to zero to restart the animation
            }

            if (_spriteIndex < Sprites.Length && _spriteIndex >= 0)
            {
                _spriteRenderer.sprite = Sprites[_spriteIndex];
            }
        }
    }

    public void Die(Transform parent)
    {
        _isDead = true;

        _spriteRenderer.sprite = _deathSprite;    // switch to death sprite 

        CancelInvoke(nameof(AnimateSprite));    // stop animation loop 

        _direction = Vector3.zero;            // stop custom movement 
        _rb.linearVelocity = Vector2.zero;           // stop rigidbody movement 
        _rb.simulated = false;                      // freeze physics 
        
        _attachedObject = parent;               // stick to the parent object (collision place)   (making sure it doesn't
                                                // inherit the rotation or other transform properties 

        _attachmentOffset = transform.position - parent.position;    // make sure the player freezes EXACTLY where it stopped 

    }

    // ---------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)     // these are the sounds before reset game
    {
        if (other.gameObject.CompareTag("ScoreArea"))  // while playing: ping!
        {
            _source.clip = _scoreArea;
            GameBehavior.Instance.IncreaseScore();
        }  
        
        
        if (other.CompareTag("SkiFence"))             // losing SFX-- this is going to be a longer sound effect
        {
            Debug.Log("You hit the fence!");
            Die(other.transform);
            StartCoroutine(GameBehavior.Instance.GameOverAfterFenceHit());
            
        }                        
    else if (other.gameObject.CompareTag("Chairlift"))  //angry "hey!!"
        {
            Debug.Log("You hit the chairlift!");
            Die(other.transform);
            StartCoroutine(GameBehavior.Instance.GameOverAfterChairliftHit());
        } 

    if (other.gameObject.CompareTag("RockTower"))  // rock crumble 
        {
            Debug.Log("You hit the chairlift!");
            Die(other.transform);
            StartCoroutine(GameBehavior.Instance.GameOverAfterRockTowerHit());
        }

        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.Play();

    }

// --------------------------------------------------------------------------------------------- END BRACKET

    }

