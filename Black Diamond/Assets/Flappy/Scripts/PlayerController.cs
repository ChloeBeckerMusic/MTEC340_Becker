using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatAmplitude = 0.1f;
    [SerializeField] private float floatSpeed = 4f;

    [Header("Movement")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 20f;


    private Rigidbody2D rb;
    private Vector3 startPosition;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    

    private void Start()
    {
        startPosition = Vector3.zero;
        rb.simulated = false;
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (GameManager.Instance.GameState == GameState.GameOver || GameManager.Instance.GameState == GameState.Home) return;

        if (GameManager.Instance.GameState == GameState.GetReady)
        {
            GameManager.Instance.GamePlay();
            rb.simulated = true;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        AudioManager.Instance.Fly();
    }
    private void FloatIdle()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Home || GameManager.Instance.GameState == GameState.GetReady)
        {
            FloatIdle();
        }
        else
        {
            RotateBird();
        }
    }

    private void RotateBird()
    {
        float angle = Mathf.Clamp(rb.linearVelocity.y * 5f, -90f, 30f);
       // Debug.Log($"angle : {angle} , linery : {rb.linearVelocity.y}, linery5: { rb.linearVelocity.y * 5f}");
        
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            rotationSpeed * Time.deltaTime
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.GameState == GameState.GameOver) return;

        if (!collision.collider.CompareTag("Obstacle"))
            return;

        AudioManager.Instance.Hit();

        StartCoroutine(DieSoundDelay());
        GameManager.Instance.GameOver();
    }

    private IEnumerator DieSoundDelay()
    {
        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.Die();
    }
    public void ResetPlayer()
    {
        rb.simulated = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        startPosition = new Vector3(-0.5f, 0f, 0f);
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
    }
}
