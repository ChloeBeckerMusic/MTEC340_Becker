using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonClickSound;
    [SerializeField] private AudioSource _UIAudioSource;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayClick);
    }

    private void PlayClick()
    {
        if (_UIAudioSource == null || _buttonClickSound == null)
            return;

        _UIAudioSource.PlayOneShot(_buttonClickSound, 0.5f);
    }
}