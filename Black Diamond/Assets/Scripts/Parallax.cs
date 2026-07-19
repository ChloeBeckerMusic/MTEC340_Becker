using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _animationSpeed = 1f;

//0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//
// --------------------------------------------------------------------------------------------- AWAKE

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
                                        // yo the background needs to know your variable 
    }

// --------------------------------------------------------------------------------------------- UPDATE
    private void Update()
    {
        _meshRenderer.material.mainTextureOffset += new Vector2(_animationSpeed * Time.deltaTime, 0);
                                        // meshRenderer is talking about the backgroundQuad
                                        // offset is what is moving it right to left
        
    }


// --------------------------------------------------------------------------------------------- END BRACKET
}
