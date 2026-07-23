using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("BlackDiamond");
    }

// --------------------------------------------------------------------------------------------- 

    // private void Update()                                    // this was me trying to deal with quitting
                                                                // the game with a key push 
    // {
    //     if (Input.GetKey(KeyCode.Q))
    //     {
    //         Debug.Log("Holding Q");
    //     }
    //
    //     if (Input.GetKey(KeyCode.Escape))
    //     {
    //         Debug.Log("Holding Escape");
    //     }
    // }

// --------------------------------------------------------------------------------------------- 
                                                        // yes this is duplicated code, but it's a
                                                        // small game and I'm making the active choice
                                                        // to be okay with it
    public void Quit()                                                 
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

// --------------------------------------------------------------------------------------------- 
}