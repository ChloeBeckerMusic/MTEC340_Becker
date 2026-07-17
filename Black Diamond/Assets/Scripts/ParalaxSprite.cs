using UnityEngine;

public class ParalaxSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _animationSpeed = 1f;

//0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//


// --------------------------------------------------------------------------------------------- AWAKE

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // yo the background needs to know your variable 
    }

// --------------------------------------------------------------------------------------------- UPDATE
    private void Update()
    {
        _spriteRenderer.material.mainTextureOffset += new Vector2(_animationSpeed * Time.deltaTime, 0);
        // offset is what is moving it right to left
    }


// --------------------------------------------------------------------------------------------- END BRACKET
}
