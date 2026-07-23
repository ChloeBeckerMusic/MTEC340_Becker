using UnityEngine;

public class WaffleBehavior : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private float _leftEdge;


    private void Start()
    {
        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        if (GameBehavior.Instance.State != Utilities.GameState.Play)
            return;

        transform.position += Vector3.left * _speed * Time.deltaTime;

        if (transform.position.x < _leftEdge)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameBehavior.Instance.PlayerCollectedWaffle();

        Destroy(gameObject);
    }
}