using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private float _duration = 3f;

    private void Start()
    {
        AudioManager.Instance.PlayMusic();
        Invoke(nameof(LoadMenu), _duration);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
