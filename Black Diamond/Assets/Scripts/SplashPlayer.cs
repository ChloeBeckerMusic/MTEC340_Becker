using UnityEngine;

public class SplashPlayer : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    private SpriteRenderer _spriteRenderer;
    private int _spriteIndex;

// --------------------------------------------------------------------------------------------- 

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
// --------------------------------------------------------------------------------------------- 

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }
// --------------------------------------------------------------------------------------------- 

    private void AnimateSprite()
    {
        _spriteIndex++;

        if (_spriteIndex >= _sprites.Length)
            _spriteIndex = 0;

        _spriteRenderer.sprite = _sprites[_spriteIndex];
    }
// --------------------------------------------------------------------------------------------- END BRACKET
}