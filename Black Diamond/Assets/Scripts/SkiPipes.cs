using UnityEngine;

public class SkiPipes : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float _leftEdge;
    private Rigidbody2D _rb;

//0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//
// --------------------------------------------------------------------------------------------- START
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;  
                                        // calling our camera view!!!
                                        // the -1f is so that we don't see the pipe disappear since the
                                        // pipe is centered around the center of the object
                                        // also we only care about the x-axis here
    }

// --------------------------------------------------------------------------------------------- UPDATE
    private void Update()
    {
        transform.position += speed * Vector3.left * Time.deltaTime;           // guys why is this inefficient :')

        if (transform.position.x < _leftEdge)
        {
            Destroy(gameObject);
        }
    }
}

