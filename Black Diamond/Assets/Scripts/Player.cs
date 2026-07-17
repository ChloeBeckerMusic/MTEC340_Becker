using UnityEngine;

public class Player : MonoBehaviour
{

    private Vector3 _direction;
    public float Gravity = -9.81f;
    public float Strength = 5f;

    private SpriteRenderer _spriteRenderer;
    public Sprite[] _sprites;
    private int _spriteIndex;

//0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//

// --------------------------------------------------------------------------------------------- AWAKE

 private void Awake()
 {
_spriteRenderer = GetComponent<SpriteRenderer>();
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
        
                                    // this would be useful if you were doing this on a touch screen:

                                           // if (Input.touchCount > 0)
                                           // {
                                           // Touch touch = Input.GetTouch(0);
                                           // if touch.phase == TouchPhase.Began)
                                           //{
                                           //   direction = Vector3.up * strength;
                                           //}
                                           // }

       _direction.y += Gravity * Time.deltaTime;
        transform.position += _direction * Time.deltaTime;
        
                                            // this makes sure that gravity is an acceleration
                                            // which is why we're multiplying it both times

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

// ---------------------------------------------------------------------------------------------




// --------------------------------------------------------------------------------------------- END BRACKET
}

