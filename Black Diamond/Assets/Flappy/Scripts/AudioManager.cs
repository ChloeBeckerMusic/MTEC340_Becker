using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip fly;
    [SerializeField] private AudioClip score;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip die;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void Fly()
    {
        audioSource.PlayOneShot(fly);
    }

    public void Score()
    {
        audioSource.PlayOneShot(score);
    }

    public void Hit()
    {
        audioSource.PlayOneShot(hit);
    }

    public void Die()
    {
        audioSource.PlayOneShot(die);
    }
}