using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("BlackDiamond");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Holding Q");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Holding Escape");
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}