using UnityEngine;

public class Pipes : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float _leftEdge;

    private void Start()
    {
_leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }
    private void Update()
    {
        transform.position += speed * Vector3.left * Time.deltaTime;

        if (transform.position.x < _leftEdge)
        {
            Destroy(gameObject);
        }
    }
}

