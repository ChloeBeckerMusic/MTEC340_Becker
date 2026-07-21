using UnityEngine;

public class SkiPipes : MonoBehaviour
{

    public Transform top;
    public Transform bottom;

    public float speed = 5f;
    public float gap = 3f;
    private float _leftEdge;

//0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//
// --------------------------------------------------------------------------------------------- START
    private void Start()
    {

        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;  
                                        // calling our camera view!!!
                                        // the -1f is so that we don't see the pipe disappear since the
                                        // pipe is centered around the center of the object
                                        // also we only care about the x-axis here
        top.position += Vector3.up * gap / 2f;
        bottom.position += Vector3.down * gap / 2f;
    }

// --------------------------------------------------------------------------------------------- UPDATE
    private void Update()
    {
        if (GameBehavior.Instance.State != Utilities.GameState.Play)
            return;

        transform.position += speed * Time.deltaTime * Vector3.left;

            if (transform.position.x < _leftEdge)
            {
                Destroy(gameObject);
            }
    }
}

