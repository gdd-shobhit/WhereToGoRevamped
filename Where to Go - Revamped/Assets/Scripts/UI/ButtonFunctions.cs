using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    //Load a scene
    //by name when the button is clicked
    public void LoadOnClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    //Exit the application
    public void ExitOnClick()
    {
        Application.Quit();
    }
}
