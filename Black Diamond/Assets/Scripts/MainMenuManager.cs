using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(0.1f); 
        SceneManager.LoadScene("BlackDiamond");
    }

    public void Quit()
    {
        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame ()
    {
        yield return new WaitForSeconds(0.2f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}