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
    }

    //Exit the application
    public void ExitOnClick()
    {
        Application.Quit();
    }
}
