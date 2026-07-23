using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource _audioSource;

// --------------------------------------------------------------------------------------------- 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

// --------------------------------------------------------------------------------------------- 

    public void PlayMusic()
    {
        if (_audioSource.isPlaying)
            return;

        _audioSource.loop = true;
        _audioSource.Play();
    }
// --------------------------------------------------------------------------------------------- 
}