using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public static GameManager instance;
    public float timeMultiplier = 1.0f;
    public GameObject player;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            player = GameObject.FindGameObjectWithTag("alive");
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void Update()
    {
        //Pause the game when 
        //escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                SetPause(false, 1);
            }
            else
            {
                SetPause(true, 0);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    { 

        
    }

    void SetPause(bool isPaused, int timeScale)
    {
        paused = isPaused;
        Time.timeScale = timeScale;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }
    }
}
